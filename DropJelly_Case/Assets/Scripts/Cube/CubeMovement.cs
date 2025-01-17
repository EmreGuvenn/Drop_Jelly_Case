using System;
using DG.Tweening;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public Action StopIT;
    private Vector3 startPosition; // Küpün başlangıç pozisyonu
    private Camera mainCamera; // Ana kamera referansı
    private CubeRaycaster _cubeRaycaster;
    [SerializeField] private Transform _target;
    private bool StopAll;
    private bool Touch;
    void Start()
    {
        _cubeRaycaster = GetComponent<CubeRaycaster>();
        // Küpün başlangıç pozisyonunu kaydet
     

        // Ana kamera referansını al
        mainCamera = Camera.main;
    }
    public void StartPosition()=>   startPosition = transform.position;
    void Update()
    {
        if (StopAll)
            return;


        HandleTouchInput();
    }

    // Sürekli hareket için dokunmatik girdiyi işleyin
    void HandleTouchInput()
    {
        // Eğer ekrana dokunuluyorsa (mobil cihaz) veya fare basılıysa (masaüstü)
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector3 touchPosition;
            Touch = true;
            // Mobil için dokunma pozisyonunu al
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
            }
            // Masaüstü için fare pozisyonunu al
            else
            {
                touchPosition = Input.mousePosition;
            }

            // Dokunma pozisyonunu dünya koordinatlarına dönüştür
            Vector3 worldPosition =
                mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane));

            // Küpün pozisyonunu yalnızca X ekseninde güncelle
            transform.position = new Vector3(worldPosition.x, startPosition.y, startPosition.z);
        }

        if (Input.GetMouseButtonUp(0) && Touch)
        {
            Touch = false;
            MoveToNearestEmptyGrid();
        }
    }

    void MoveToNearestEmptyGrid()
    {
       
        if (_cubeRaycaster._gridColumnParent == null) return;
        StopIT?.Invoke();
        _cubeRaycaster._gridColumnParent.CheckAndAssignTargetGrid();
        _cubeRaycaster._gridColumnParent.tempCube.Add(GetComponent<CubeMovement>());
        _target = _cubeRaycaster._gridColumnParent.targetGrid.transform;
        transform.parent = _target;
        LeanTween.delayedCall(gameObject, 0.05f*_cubeRaycaster._gridColumnParent.ListCount, CallFunction);
        transform.DOLocalMove(Vector3.zero, 0.15f*_cubeRaycaster._gridColumnParent.ListCount).SetEase(Ease.OutBounce).OnComplete(
            () =>
            {
                _cubeRaycaster._gridColumnParent.targetGrid.AssignTempCube(gameObject);
                CubeManager.Instance.CreateCubeAgain?.Invoke();
            });
    
        _cubeRaycaster._gridColumnParent.targetGrid.isEmpty = false;
        StopAll = true;
    }
    void CallFunction()=>  _cubeRaycaster._gridColumnParent.ShakeIt();
}