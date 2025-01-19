using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGridLevel", menuName = "Grid/Level")]
public class GridLevel : ScriptableObject
{
    [System.Serializable]
    public class GridCell
    {
        public GameObject prefab; // Hücrede kullanılacak prefab
        public List<Color> childColors; // Prefabın child objelerinin renkleri
    }

    public List<GridCell> cells = new List<GridCell>(36); // 6x6 grid için 36 hücre

    private void OnValidate()
    {
        // Hücrelerin sayısını her zaman 36'ya sabitliyoruz
        while (cells.Count < 36)
        {
            cells.Add(new GridCell { prefab = null, childColors = new List<Color>() });
        }
        while (cells.Count > 36)
        {
            cells.RemoveAt(cells.Count - 1);
        }
    }
}