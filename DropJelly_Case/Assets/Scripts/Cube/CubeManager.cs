using System;
using System.Collections.Generic;
using _Reusable.Singleton;
using DG.Tweening;
using GridSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class CubeManager : NonPersistentSingleton<CubeManager>
{
    public Action CreateCubeAgain;
    public List<GameObject> cubePrefab; // Küp prefab'ları
    public List<Color> cubeColors; // Küp renkleri
    private Queue<Color> colorQueue; // Tekrarsız renk sıralaması için kullanılan kuyruk
    public GridSettings _GridSettings;
    public Transform gridParent;  // Küplerin yerleşeceği gridin parent'ı
    public float gridSize = 1f;   // Grid boyutu
    public LayerMask layerMask;   // Küplerin düşeceği yüzeyin maskesi
    public Transform cubeSpawnPoint;

    void Start()
    {
        InitializeColorQueue();
        CreateCube(cubeSpawnPoint);
        CreateCubeAgain += CreateAgain;
    }

    private void OnDestroy()
    {
        CreateCubeAgain -= CreateAgain;
    }

    // Renk kuyruğunu başlatma ve sıfırlama
    private void InitializeColorQueue()
    {
        colorQueue = new Queue<Color>(cubeColors);
        ShuffleColorQueue(); // Renkleri rastgele sırala
    }

    // Kuyruğu karıştıran bir yöntem
    private void ShuffleColorQueue()
    {
        List<Color> shuffledColors = new List<Color>(colorQueue);
        for (int i = 0; i < shuffledColors.Count; i++)
        {
            int randomIndex = Random.Range(0, shuffledColors.Count);
            (shuffledColors[i], shuffledColors[randomIndex]) = (shuffledColors[randomIndex], shuffledColors[i]);
        }
        colorQueue = new Queue<Color>(shuffledColors);
    }

    void CreateAgain() => CreateCube(cubeSpawnPoint);

    void CreateCube(Transform pos)
    {
        GameObject cube = Instantiate(cubePrefab[Random.Range(0, cubePrefab.Count)], pos.position + Vector3.up * 10f, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one * _GridSettings.cellSize;

        // Renk kuyruğunu kontrol et
        if (colorQueue.Count < cube.transform.childCount)
        {
            InitializeColorQueue(); // Yeterli renk yoksa kuyruğu sıfırla
        }

        // Her child için bir renk ata
        AssignColorsToChildren(cube);

        // Animasyonla düşür
        cube.transform.DOMove(new Vector3(0, cube.transform.position.y - 10f, 0), 0.25f)
            .SetEase(Ease.InBounce)
            .OnComplete(() =>
            {
                cube.GetComponent<CubeMovement>().StartPosition();
            });
    }

    // Küpe renk atama
    private void AssignColorsToChildren(GameObject parentCube)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parentCube.transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            if (colorQueue.Count == 0)
            {
                InitializeColorQueue(); // Kuyruk boşaldıysa yeniden doldur
            }

            Color assignedColor = colorQueue.Dequeue(); // Renk al
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                // Materyal oluştur veya mevcut materyali değiştir
                Material newMaterial = new Material(childRenderer.sharedMaterial);
                newMaterial.color = assignedColor;
                childRenderer.material = newMaterial;
            }
        }
    }
}
