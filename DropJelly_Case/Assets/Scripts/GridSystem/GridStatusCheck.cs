using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween için gerekli

public class GridStatusCheck : MonoBehaviour
{
    private GridCell cell;
    public bool isEmpty = true;
    public GameObject tempCube;
    private Vector2Int gridPosition; // Küpün grid pozisyonu
    private Vector2Int gridSize; // Küpün grid üzerindeki boyutları

    // Renk bilgileri
    public Color topRight;
    public Color topLeft;
    public Color botRight;
    public Color botLeft;

    // Komşu hücreler
    private GridStatusCheck leftNeighbor;
    private GridStatusCheck rightNeighbor;
    private GridStatusCheck upNeighbor;
    private GridStatusCheck downNeighbor;



    void Start()
    {
        AssignNeighbors();
        GameManager.Instance.CheckAllGridForMatch += CheckNeighborColors;
    }

    private void OnDestroy()
    {
        GameManager.Instance.CheckAllGridForMatch -= CheckNeighborColors;
    }

    public void AssignTempCube(GameObject cube)
    {
        tempCube = cube;

        // CubeChildsBehaviour üzerinden renkleri al
        CubeChildsBehaviour temp = tempCube.GetComponent<CubeChildsBehaviour>();
        topRight = temp.topRight;
        topLeft = temp.topLeft;
        botRight = temp.botRight;
        botLeft = temp.botLeft;

        cell.topRightObj = temp.topRightObj.gameObject;
        cell.topLeftObj = temp.topLeftObj.gameObject;
        cell.botRightObj = temp.botRightObj.gameObject;
        cell.botLeftObj = temp.botLeftObj.gameObject;
        // Komşu renkleri kontrol et
      //  CheckNeighborColors();
        GameManager.Instance.CheckAllGridForMatch?.Invoke();
    }

    void AssignNeighbors()
    {
       cell= GetComponent<GridCell>();
        if (cell != null)
        {
            leftNeighbor = cell.leftNeighbor;
            rightNeighbor = cell.rightNeighbor;
            upNeighbor = cell.upNeighbor;
            downNeighbor = cell.downNeighbor;
        }
    }

    void CheckNeighborColors()
    {
        List<GameObject> matchingObjects = new List<GameObject>();

        // Sol komşu kontrolü
        if (leftNeighbor != null)
        {
            if (leftNeighbor.cell.topRightObj != null && cell.topLeftObj != null &&
                leftNeighbor.topRight == topLeft)
            {
                matchingObjects.Add(leftNeighbor.cell.topRightObj);
                matchingObjects.Add(cell.topLeftObj);
            }
        }

        // Sağ komşu kontrolü
        if (rightNeighbor != null)
        {
            if (rightNeighbor.cell.topLeftObj != null && cell.topRightObj != null &&
                rightNeighbor.topLeft == topRight)
            {
                matchingObjects.Add(rightNeighbor.cell.topLeftObj);
                matchingObjects.Add(cell.topRightObj);
            }
        }

        // Üst komşu kontrolü
        if (upNeighbor != null)
        {
            if (upNeighbor.cell.botLeftObj != null && cell.topLeftObj != null &&
                upNeighbor.botLeft == topLeft)
            {
                matchingObjects.Add(upNeighbor.cell.botLeftObj);
                matchingObjects.Add(cell.topLeftObj);
            }
        }

        // Alt komşu kontrolü
        if (downNeighbor != null)
        {
            if (downNeighbor.cell.topLeftObj != null && cell.botLeftObj != null &&
                downNeighbor.topLeft == botLeft)
            {
                matchingObjects.Add(downNeighbor.cell.topLeftObj);
                matchingObjects.Add(cell.botLeftObj);
            }
        }

        // Eşleşen nesneler bulunduysa animasyon yap ve yok et
        if (matchingObjects.Count > 0)
        {
            AnimateAndDestroy(matchingObjects);
        }
    }

    void AnimateAndDestroy(List<GameObject> objects)
    {
        Vector3 centerPoint = CalculateCenterPoint(objects); // Objelerin merkez noktası

        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;

            // Objeyi merkeze doğru çekme
            obj.transform.DOMove(centerPoint, 0.3f).SetEase(Ease.InOutQuad);

            // Objeyi küçültme ve yok etme
            obj.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                Destroy(obj);
            });
        }
    }
    Vector3 CalculateCenterPoint(List<GameObject> objects)
    {
        Vector3 totalPosition = Vector3.zero;
        int count = 0;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                totalPosition += obj.transform.position;
                count++;
            }
        }

        return count > 0 ? totalPosition / count : Vector3.zero; // Ortalama pozisyonu döndür
    }

}