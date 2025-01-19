using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridLevel))]
public class GridLevelEditor : Editor
{
    private GridLevel gridLevel;

    private void OnEnable()
    {
        gridLevel = (GridLevel)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("6x6 Grid Düzenleyici", EditorStyles.boldLabel);

        for (int row = 0; row < 6; row++)
        {
            GUILayout.BeginHorizontal();
            for (int col = 0; col < 6; col++)
            {
                int index = row * 6 + col;
                GridLevel.GridCell cell = gridLevel.cells[index];

                string buttonLabel = cell.prefab ? cell.prefab.name : "Empty";
                if (GUILayout.Button(buttonLabel, GUILayout.Width(60), GUILayout.Height(60)))
                {
                    OpenCellEditWindow(cell);
                }
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Kaydet", GUILayout.Height(30)))
        {
            EditorUtility.SetDirty(gridLevel);
        }
    }

    private void OpenCellEditWindow(GridLevel.GridCell cell)
    {
        GridCellEditWindow window = EditorWindow.GetWindow<GridCellEditWindow>();
        window.Initialize(cell);
    }
}