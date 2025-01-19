using UnityEditor;
using UnityEngine;

public class GridCellEditWindow : EditorWindow
{
    private GridLevel.GridCell cell;

    public void Initialize(GridLevel.GridCell cell)
    {
        this.cell = cell;
        Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Hücre Düzenleme", EditorStyles.boldLabel);

        // Prefab Seçimi
        cell.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab Seç", cell.prefab, typeof(GameObject), false);

        // Child Renkleri
        if (cell.prefab != null)
        {
            int childCount = cell.prefab.transform.childCount;

            // Renk listesini child sayısına göre ayarla
            while (cell.childColors.Count < childCount)
            {
                cell.childColors.Add(Color.white);
            }
            while (cell.childColors.Count > childCount)
            {
                cell.childColors.RemoveAt(cell.childColors.Count - 1);
            }

            GUILayout.Label("Child Renkleri");
            for (int i = 0; i < childCount; i++)
            {
                cell.childColors[i] = EditorGUILayout.ColorField($"Child {i + 1}", cell.childColors[i]);
            }
        }

        if (GUILayout.Button("Kaydet"))
        {
            Close();
        }
    }
}