using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;

public class GridColumnParent : MonoBehaviour
{
    public List<GridStatusCheck> childObject = new List<GridStatusCheck>();
    public List<CubeMovement> tempCube = new List<CubeMovement>();
    public GridStatusCheck targetGrid;
    public int ListCount;
    
    public float shakeStrength = 5f;  
    public float shakeDuration = .2f; 
    public int shakeVibrato = 3;
    public bool StopFail = false;
    private void Start()
    {
        GameManager.Instance.CheckGridList+=ClearAndAddedAgain;
        GameManager.Instance.CheckAllGridForMatch += CheckGridUpNeighbor;
    }

    private void OnDestroy()
    {
        GameManager.Instance.CheckGridList-=ClearAndAddedAgain;
        GameManager.Instance.CheckAllGridForMatch -= CheckGridUpNeighbor;
    }

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
                lastGrid.isEmpty = false;
                targetGrid = lastGrid;
                childObject.RemoveAt(childObject.Count - 1);
            }
        }
        else
        {
            StopFail = true;
            GameManager.Instance.LevelEnd(false);
        }

        ClearAndAddedAgain();
    }

    void ClearAndAddedAgain()
    {
        childObject.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentChild = transform.GetChild(i);
            GridStatusCheck gridStatus = currentChild.GetComponent<GridStatusCheck>();
            if (gridStatus != null && gridStatus.isEmpty)
            {
                childObject.Add(gridStatus);
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
            if (tempCube[i]!= null)
            {
                tempCube[i].gameObject.transform.DOShakePosition(.2f, shakeAmount, 1);
            }
        
        }
    }

    public void CheckGridUpNeighbor() => StartCoroutine(CheckGridUpNeighborForWait());
    public IEnumerator CheckGridUpNeighborForWait()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<GridStatusCheck>().CheckGridisEmpty();
            yield return new WaitForSeconds(.1f);
        }
    }
}