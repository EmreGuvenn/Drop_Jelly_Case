using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeRaycaster : MonoBehaviour
{
    private bool stop;
    public float rayLength = 1f;
    public LayerMask gridLayer;
    private CubeMovement _cubeMovement;
    private GridColorControl lastHitGrid;
    public GridColumnParent _gridColumnParent;

    private void Start()
    {
        _cubeMovement = GetComponent<CubeMovement>();
        _cubeMovement.StopIT += Stop;
    }

    private void OnDestroy()
    {
        _cubeMovement.StopIT -= Stop;

    }

    void Stop() => stop = true;
    void Update()
    {
        if (stop)
        {
            return;
        }
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength, gridLayer);

        // Tüm çarpan grid'lere işlem yap
        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                GridColorControl grid = hit.collider.GetComponent<GridColorControl>();
                if (grid != null)
                {
                 
                    _gridColumnParent = grid.transform.parent.GetComponent<GridColumnParent>();
                    grid.isRayHit = true;
                    grid.ChangeColor();
                 
                }
            }
        }

        if (hits.Length == 0 && lastHitGrid != null)
        {
            lastHitGrid.ResetColor();
            lastHitGrid = null;
        }
    }

    private void OnDrawGizmos()
    {
        // Ray'i görselleştirmek için bir çizgi çiz
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }
}