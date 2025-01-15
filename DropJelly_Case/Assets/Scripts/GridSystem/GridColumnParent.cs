using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridColumnParent : MonoBehaviour
{
    [SerializeField] private List<GridStatusCheck> childObject = new List<GridStatusCheck>();
    public List<CubeMovement> tempCube = new List<CubeMovement>();
    public GridStatusCheck targetGrid;
    public int ListCount;
    
    public float shakeStrength = 5f;  
    public float shakeDuration = .2f; 
    public int shakeVibrato = 3;
    public void ArrangeGridCellsInColumn(GridStatusCheck grid)
    {
      
        childObject.Add(grid);
       
    }
    
    public void CheckAndAssignTargetGrid()
    {
        if (childObject.Count > 0)
        {
            ListCount = childObject.Count;
            GridStatusCheck lastGrid = childObject[childObject.Count - 1];
            if (lastGrid.isEmpty)
            {
                targetGrid = lastGrid;
                childObject.RemoveAt(childObject.Count - 1);
            }

          
        }
    }

    public void ShakeIt()
    {
        ApplyShakePosEffect();
      //  ApplyShakeEffect();
    }
    void ApplyShakeEffect()
    {
        for (int i = 0; i < tempCube.Count-1; i++)
        {
            tempCube[i].gameObject.transform.DOShakeRotation(shakeDuration, shakeStrength, shakeVibrato);
        }
    }
    void ApplyShakePosEffect()
    {
        for (int i = 0; i < tempCube.Count-1; i++)
        {
            Vector3 shakeAmount = new Vector3(0f, .1f, 0f);
            tempCube[i].gameObject.transform.DOShakePosition(.2f, shakeAmount, 1);
        }
    }
}