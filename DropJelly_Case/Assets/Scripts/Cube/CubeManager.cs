using System;
using _Reusable.Singleton;
using DG.Tweening;
using GridSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeManager : NonPersistentSingleton<CubeManager>
{
    public Action CreateCubeAgain;
    public GameObject cubePrefab; // Küp prefab'ı
    public GridSettings _GridSettings;
    public Transform gridParent;  // Küplerin yerleşeceği gridin parent'ı
    public float gridSize = 1f;   // Grid boyutu
    public LayerMask layerMask;   // Küplerin düşeceği yüzeyin maskesi
    public Transform cubeSpawnPoint;
    void Start()
    {
        CreateCube(cubeSpawnPoint);
        CreateCubeAgain += CreateAgain;
    }

    private void OnDestroy()
    {
        CreateCubeAgain -= CreateAgain;
    }

    void CreateAgain() => CreateCube(cubeSpawnPoint);
    void CreateCube(Transform pos)
    {
        GameObject cube = Instantiate(cubePrefab, pos.position+Vector3.up*10f, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one*_GridSettings.cellSize;
        cube.transform.DOMove(new Vector3(0,cube.transform.position.y - 10f,0), 0.25f).SetEase(Ease.InBounce).OnComplete(
            () =>
            {
                cube.GetComponent<CubeMovement>().StartPosition();
            });
        
    }
}
