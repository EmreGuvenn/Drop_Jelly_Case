using System.Collections.Generic;
using GridSystem;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GridSettings gridSettings; // ScriptableObject referansı
    [SerializeField] private GameObject cellPrefab; // Hücre prefab'ını atayacağınız yer
    [SerializeField] private float columnSpacing; // Columnlar arası boşluk
    [SerializeField] private List<GridCell> _gridRaycasy;

    void Start()
    {
        GenerateGrid();
        GridsFindNeighbors();
    }

    void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("Hücre prefab'i atanmadı!");
            return;
        }

        RectTransform parentRectTransform = GetComponent<RectTransform>();
        if (parentRectTransform == null)
        {
            Debug.LogError("Bu obje bir RectTransform içermiyor!");
            return;
        }

        // Grid'in toplam genişlik ve yüksekliği (padding dahil)
        float totalWidth = gridSettings.columns * gridSettings.cellSize + (gridSettings.columns - 1) * gridSettings.spacing;
        float totalHeight = gridSettings.rows * gridSettings.cellSize + (gridSettings.rows - 1) * gridSettings.spacing;

        // Başlangıç pozisyonunu hesapla
        Vector2 startPosition = new Vector2(
            -totalWidth / 2 + gridSettings.cellSize / 2, // Sol üst köşeyi merkeze göre kaydır
            totalHeight / 2 - gridSettings.cellSize / 2  // Sol üst köşeyi merkeze göre kaydır
        );

        // Sütun sayısına göre ortalama yapalım
        float halfWidth = totalWidth / 2; // toplam genişliğin yarısı
        float firstColumnX = -halfWidth + gridSettings.cellSize / 2; // İlk sütun için pozisyon
        float secondColumnX = halfWidth - gridSettings.cellSize / 2; // Son sütun için pozisyon

        // Sütunları oluştur ve parent'a ekle
        for (int column = 0; column < gridSettings.columns; column++)
        {
            // Her sütun için bir GameObject oluştur
            GameObject columnObject = new GameObject($"Column {column + 1}");
            columnObject.transform.SetParent(transform);  // Bu GameObject'i ana grid objesinin altına koy
            RectTransform columnRectTransform = columnObject.AddComponent<RectTransform>();
            columnRectTransform.pivot = new Vector2(0.5f, 0.5f); // Pivot'u ortalamak için
            columnRectTransform.anchorMin = new Vector2(0.5f, 0.5f); // Ortalanacak
            columnRectTransform.anchorMax = new Vector2(0.5f, 0.5f); // Ortalanacak

            // Sütunları yerleştirelim (ilk 3 sol, sonrakiler sağda olacak)
            if (column < gridSettings.columns / 2) // İlk yarı (sol)
            {
                columnRectTransform.localPosition = new Vector3(firstColumnX + (column * (gridSettings.cellSize + columnSpacing)), 0, 0);
            }
            else // İkinci yarı (sağ)
            {
                columnRectTransform.localPosition = new Vector3(secondColumnX - ((gridSettings.columns - column - 1) * (gridSettings.cellSize + columnSpacing)), 0, 0);
            }

            columnRectTransform.localScale = Vector3.one; // Scale'i 1 yap

            // Sütunun genişliğini ayarlayalım
            columnRectTransform.sizeDelta = new Vector2(gridSettings.cellSize, totalHeight);
            GridColumnParent columnParent = columnObject.AddComponent<GridColumnParent>();
            // Hücreleri oluştur ve sütunun altına yerleştir
            for (int row = 0; row < gridSettings.rows; row++)
            {
                // Hücre pozisyonunu hesapla
                Vector2 cellPosition = new Vector2(
                    0, // X ekseninde sütun düzeni zaten sütuna bağlı olacak
                    startPosition.y - row * (gridSettings.cellSize + gridSettings.spacing)
                );

                // Hücreyi oluştur
                GameObject cell = Instantiate(cellPrefab, columnObject.transform); // Hücreyi doğru sütuna yerleştir
                _gridRaycasy.Add(cell.GetComponent<GridCell>());
                RectTransform cellRectTransform = cell.GetComponent<RectTransform>();
                if (cellRectTransform == null)
                {
                    Debug.LogError("Hücre prefab'inizde RectTransform yok!");
                    return;
                }

                // Hücre boyutunu ayarla
                cellRectTransform.sizeDelta = new Vector2(gridSettings.cellSize, gridSettings.cellSize);
                // Hücre pozisyonunu ayarla
                cellRectTransform.anchoredPosition = cellPosition;

                // Hücreye ismini satır ve sütun numarasına göre ver
                cell.name = $"{column},{row}"; // Hücre ismini 0,0 formatında atıyoruz

                cell.transform.parent.GetComponent<GridColumnParent>()
                    .ArrangeGridCellsInColumn(cell.GetComponent<GridStatusCheck>());
            }
        }
    }

    void GridsFindNeighbors()
    {
        // Grid hücrelerinin her birini kontrol ediyoruz
        for (int column = 0; column < gridSettings.columns; column++)
        {
            for (int row = 0; row < gridSettings.rows; row++)
            {
                // Hücreyi alıyoruz
                GameObject cellObject = transform.GetChild(column).GetChild(row).gameObject;
               

                // GridCell referansı
                GridCell gridCell =  cellObject.GetComponent<GridCell>();

                // Sol komşu
                if (column > 0) 
                {
                    gridCell.leftNeighbor = transform.GetChild(column - 1).GetChild(row).GetComponent<GridStatusCheck>();
                }

                // Sağ komşu
                if (column < gridSettings.columns - 1)
                {
                    gridCell.rightNeighbor = transform.GetChild(column + 1).GetChild(row).GetComponent<GridStatusCheck>();
                }

                // Yukarı komşu
                if (row > 0)
                {
                    gridCell.upNeighbor = transform.GetChild(column).GetChild(row - 1).GetComponent<GridStatusCheck>();
                }

                // Aşağı komşu
                if (row < gridSettings.rows - 1)
                {
                    gridCell.downNeighbor = transform.GetChild(column).GetChild(row + 1).GetComponent<GridStatusCheck>();
                }
            }
        }
    }
}
