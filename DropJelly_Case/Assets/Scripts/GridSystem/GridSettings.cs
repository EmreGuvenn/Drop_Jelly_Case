using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "ScriptableObjects/GridSettings", order = 0)]
    public class GridSettings : ScriptableObject
    {
        public int rows = 6; // Satır sayısı
        public int columns = 6; // Sütun sayısı
        public float cellSize = 1f;
        public float spacing = 0.1f; // Hücreler arası boşluk
    }
}