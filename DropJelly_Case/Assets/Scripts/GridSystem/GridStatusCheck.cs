using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween için gerekli

public class GridStatusCheck : MonoBehaviour
{
    [SerializeField] private ParticleSystem _MatchParticle;
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
    private bool OneTime = false;

    void Start()
    {
        AssignNeighbors();
        GameManager.Instance.CheckAllGridForMatch += CheckNeighborColors;
        GameManager.Instance.CubeDestroy += CheckGridisEmpty;
        AssignLevelElementCubes();
    }

    private void OnDestroy()
    {
        GameManager.Instance.CheckAllGridForMatch -= CheckNeighborColors;
        GameManager.Instance.CubeDestroy -= CheckGridisEmpty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.LogError("Girdi");
          
        }
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

    void AssignColorsAgain()
    {
        if (tempCube != null)
        {
            CubeChildsBehaviour temp = tempCube.GetComponent<CubeChildsBehaviour>();
            topRight = temp.topRight;
            topLeft = temp.topLeft;
            botRight = temp.botRight;
            botLeft = temp.botLeft;
            cell.topRightObj = temp.topRightObj.gameObject;
            cell.topLeftObj = temp.topLeftObj.gameObject;
            cell.botRightObj = temp.botRightObj.gameObject;
            cell.botLeftObj = temp.botLeftObj.gameObject;
        }
    }

    void AssignNeighbors()
    {
        cell = GetComponent<GridCell>();
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
    if (leftNeighbor != null && leftNeighbor.cell.topRightObj != null && cell.topLeftObj != null)
    {
        // topRight ile topLeft kontrolü
        if (AreColorsSimilar(leftNeighbor.topRight, topLeft))
        {
            matchingObjects.Add(leftNeighbor.cell.topRightObj);
            matchingObjects.Add(cell.topLeftObj);
        }

        // botRight ile botLeft kontrolü
        if (AreColorsSimilar(leftNeighbor.botRight, botLeft) && leftNeighbor.cell.botRightObj != null && cell.botLeftObj != null)
        {
            matchingObjects.Add(leftNeighbor.cell.botRightObj);
            matchingObjects.Add(cell.botLeftObj);
        }
    }

    // Sağ komşu kontrolü
    if (rightNeighbor != null && rightNeighbor.cell.topLeftObj != null && cell.topRightObj != null)
    {
        // topLeft ile topRight kontrolü
        if (AreColorsSimilar(rightNeighbor.topLeft, topRight))
        {
            matchingObjects.Add(rightNeighbor.cell.topLeftObj);
            matchingObjects.Add(cell.topRightObj);
        }

        // botLeft ile botRight kontrolü
        if (AreColorsSimilar(rightNeighbor.botLeft, botRight) && rightNeighbor.cell.botLeftObj != null && cell.botRightObj != null)
        {
            matchingObjects.Add(rightNeighbor.cell.botLeftObj);
            matchingObjects.Add(cell.botRightObj);
        }
    }

    // Üst komşu kontrolü
    if (upNeighbor != null && upNeighbor.cell.botLeftObj != null && cell.topLeftObj != null)
    {
        // botLeft ile topLeft kontrolü
        if (AreColorsSimilar(upNeighbor.botLeft, topLeft))
        {
            matchingObjects.Add(upNeighbor.cell.botLeftObj);
            matchingObjects.Add(cell.topLeftObj);
        }

        // botRight ile topRight kontrolü
        if (AreColorsSimilar(upNeighbor.botRight, topRight) && upNeighbor.cell.botRightObj != null && cell.topRightObj != null)
        {
            matchingObjects.Add(upNeighbor.cell.botRightObj);
            matchingObjects.Add(cell.topRightObj);
        }
    }

    // Alt komşu kontrolü
    if (downNeighbor != null && downNeighbor.cell.topLeftObj != null && cell.botLeftObj != null)
    {
        // topLeft ile botLeft kontrolü
        if (AreColorsSimilar(downNeighbor.topLeft, botLeft))
        {
            matchingObjects.Add(downNeighbor.cell.topLeftObj);
            matchingObjects.Add(cell.botLeftObj);
        }

        // topRight ile botRight kontrolü
        if (AreColorsSimilar(downNeighbor.topRight, botRight) && downNeighbor.cell.topRightObj != null && cell.botRightObj != null)
        {
            matchingObjects.Add(downNeighbor.cell.topRightObj);
            matchingObjects.Add(cell.botRightObj);
        }
    }

    // Eşleşen nesneleri yok et
    if (matchingObjects.Count >= 2)
    {
        AnimateAndDestroy(matchingObjects);
    }
}
    private bool AreColorsSimilar(Color color1, Color color2, float threshold = 0.1f)
    {
        // RGB bileşenlerinin farklarını hesapla
        float rDifference = Mathf.Abs(color1.r - color2.r);
        float gDifference = Mathf.Abs(color1.g - color2.g);
        float bDifference = Mathf.Abs(color1.b - color2.b);

        // Eğer farklar eşik değerinden küçükse, renkler benzer kabul edilir
        return rDifference < threshold && gDifference < threshold && bDifference < threshold;
    }

    void AnimateAndDestroy(List<GameObject> objects)
    {
        Vector3 centerPoint = CalculateCenterPoint(objects);

        centerPoint.z -= 1f;

        ParticleSystem particleInstance = null;

        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;
            Color tempColor = obj.GetComponent<Renderer>().material.color;

            obj.transform.DOMove(centerPoint, 0.3f).SetEase(Ease.InOutQuad);
            obj.transform.DORotate(new Vector3(0, 0, 10), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad);

            obj.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                if (particleInstance == null)
                {
                    particleInstance = Instantiate(_MatchParticle, obj.transform.position, Quaternion.identity);
                    var mainModule = particleInstance.main;
                    mainModule.startColor = tempColor;
                    for (int i = 0; i < particleInstance.transform.childCount; i++)
                    {
                        var secModule = particleInstance.transform.GetChild(i).GetComponent<ParticleSystem>().main;
                        secModule.startColor = tempColor;
                    }

                    particleInstance.Play();
                }

                Destroy(obj);
                GameManager.Instance.CubeDestroy?.Invoke();
            });
        }


        StartCoroutine(WaitDestroy());
    }


    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(0.55f);
        if (tempCube != null)
        {
            if (tempCube.transform.childCount == 0)
            {
                GameManager.Instance.CheckGridList?.Invoke();
                Destroy(tempCube);
            }
        }

        yield return new WaitForSeconds(0.1f);
        if (tempCube == null)
        {
            isEmpty = true;
            topRight = Color.black;
            topLeft = Color.black;
            botRight = Color.black;
            botLeft = Color.black;
            GameManager.Instance.CheckGridList?.Invoke();
            StartCoroutine(MoveCubeDownIfEmpty());
        }
        else
        {
            tempCube.GetComponent<CubeChildsBehaviour>().ReconfigureCube();
            GameManager.Instance.CheckCubeResize?.Invoke();
            yield return new WaitForSeconds(0.15f);
            AssignColorsAgain();
            yield return new WaitForSeconds(0.15f);
            GameManager.Instance.CheckAllGridForMatch?.Invoke();
        }
    }

    IEnumerator MoveCubeDownIfEmpty()
    {
        if (tempCube == null)
        {
            if (upNeighbor != null && upNeighbor.tempCube != null)
            {
                tempCube = upNeighbor.tempCube;
                GridCell tempCell = upNeighbor.GetComponent<GridCell>();
                tempCell.NullEveryObject();
                upNeighbor.topRight = Color.black;
                upNeighbor.topLeft = Color.black;
                upNeighbor.botRight = Color.black;
                upNeighbor.botLeft = Color.black;
                upNeighbor.isEmpty = true;
                upNeighbor.tempCube = null;
                tempCube.transform.SetParent(transform);
                tempCube.transform.DOLocalMove(Vector3.zero,
                    0.05f).SetEase(Ease.OutBounce);

                isEmpty = false;
                GameManager.Instance.CheckGridList?.Invoke();
                AssignColorsAgain();
                GameManager.Instance.CheckCubeResize?.Invoke();
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.CheckGridList?.Invoke();
                GameManager.Instance.CheckAllGridForMatch?.Invoke();
                GameManager.Instance.GridParentsControlUpNeighbour?.Invoke();
            }
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

    public void CheckGridisEmpty()
    {
        if (tempCube == null)
        {
            isEmpty = true;
            topRight = Color.black;
            topLeft = Color.black;
            botRight = Color.black;
            botLeft = Color.black;
            GameManager.Instance.CheckGridList?.Invoke();
            StartCoroutine(MoveCubeDownIfEmpty());
        }
    }

    public void AssignLevelElementCubes()
    {
        if (transform.childCount > 0)
        {
            tempCube = transform.GetChild(0).gameObject;
            tempCube.GetComponent<CubeMovement>()._target = transform;
            tempCube.GetComponent<CubeRaycaster>()._gridColumnParent =
                transform.parent.GetComponent<GridColumnParent>();
            isEmpty = false;
            CubeChildsBehaviour temp = tempCube.GetComponent<CubeChildsBehaviour>();
            GetComponentInParent<GridColumnParent>().tempCube.Add(tempCube.GetComponent<CubeMovement>());
            cell.topRightObj = temp.topRightObj.gameObject;
            cell.topLeftObj = temp.topLeftObj.gameObject;
            cell.botRightObj = temp.botRightObj.gameObject;
            cell.botLeftObj = temp.botLeftObj.gameObject;
            topRight =  temp.topRightObj.material.color;
            topLeft = temp.topLeftObj.material.color;
            botRight = temp.botRightObj.material.color;
            botLeft =  temp.botLeftObj.material.color;
         
            GameManager.Instance.CheckGridList?.Invoke();
            GameManager.Instance.GridParentsControlUpNeighbour?.Invoke();
        }
    }

    void AssignColors()
    {
        Debug.Log("AssignColors function is called.");

        // Renkleri rastgele oluştur
        Color randomColor1 = GenerateRandomColor();
        Color randomColor2 = GenerateRandomColor();

        // Sonsuz döngüyü önlemek için deneme sayısını sınırlayalım
        int maxAttempts = 10;
        int attempts = 0;

        bool colorsAssigned = false;
        while (!colorsAssigned && attempts < maxAttempts)
        {
            attempts++;

            // Child sayısına göre renk atama
            if (cell.topRightObj.transform.childCount == 1 && cell.topLeftObj.transform.childCount == 1)
            {
                topRight = randomColor1;
                topLeft = randomColor1;
            }
            else
            {
                topRight = randomColor1;
                topLeft = randomColor2;
            }

            if (cell.botRightObj.transform.childCount == 1 && cell.botLeftObj.transform.childCount == 1)
            {
                botRight = randomColor1;
                botLeft = randomColor1;
            }
            else
            {
                botRight = randomColor2;
                botLeft = randomColor2;
            }

            // Eğer renkler birbirinden farklıysa, atamayı bitir
            if (topRight != topLeft && topRight != botRight && topRight != botLeft &&
                topLeft != botRight && topLeft != botLeft && botRight != botLeft)
            {
                colorsAssigned = true;
            }
            else
            {
                // Renkler eşleşiyorsa, yeni renkler üret
                randomColor1 = GenerateRandomColor();
                randomColor2 = GenerateRandomColor();
            }
        }

        if (!colorsAssigned)
        {
            Debug.LogWarning("Renkler eşleşmedi. Max deneme sayısına ulaşıldı.");
        }

        // Renkleri objelere uygula
        ApplyColors();
    }

    void ApplyColors()
    {
        cell.topRightObj.GetComponent<Renderer>().material.color = topRight;
        cell.topLeftObj.GetComponent<Renderer>().material.color = topLeft;
        cell.botRightObj.GetComponent<Renderer>().material.color = botRight;
        cell.botLeftObj.GetComponent<Renderer>().material.color = botLeft;
    }


    Color GenerateRandomColor()
    {
        int randomIndex = UnityEngine.Random.Range(0, CubeManager.Instance.cubeColors.Count);
        return CubeManager.Instance.cubeColors[randomIndex];
    }
}