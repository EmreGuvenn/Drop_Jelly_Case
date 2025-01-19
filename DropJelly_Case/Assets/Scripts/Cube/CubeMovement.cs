using System;
using DG.Tweening;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public Action StopIT;
    private Vector3 startPosition; // Küpün başlangıç pozisyonu
    private Camera mainCamera; // Ana kamera referansı
    private CubeRaycaster _cubeRaycaster;
    public Transform _target;
    private bool StopAll;
    private bool Touch;

    void Start()
    {
        _cubeRaycaster = GetComponent<CubeRaycaster>();
        mainCamera = Camera.main;
    }

    public void StopCube()
    {
        StopAll = true;
        StopIT?.Invoke();
    }

    public void StartPosition() => startPosition = transform.position;

    void Update()
    {
        if (StopAll)
            return;


        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector3 touchPosition;
            Touch = true;
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
            }
            else
            {
                touchPosition = Input.mousePosition;
            }

            Vector3 worldPosition =
                mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane));

            transform.position = new Vector3(worldPosition.x, startPosition.y, startPosition.z);
        }

        if (Input.GetMouseButtonUp(0) && Touch)
        {
            Touch = false;
            if (GameManager.Instance.MoveCounter > 0)
                GameManager.Instance.MoveCounter--;
            GameManager.Instance.CountersChange?.Invoke();
            if (GameManager.Instance.MoveCounter <= 0)
            {
                GameManager.Instance.LevelEnd(false);
            }

            GameManager.Instance.CheckGridList?.Invoke();
            MoveToNearestEmptyGrid();
        }
    }

    public void MoveToNearestEmptyGrid()
    {
        if (_cubeRaycaster._gridColumnParent == null) return;
        StopIT?.Invoke();
        _cubeRaycaster._gridColumnParent.CheckAndAssignTargetGrid();
        _cubeRaycaster._gridColumnParent.tempCube.Add(GetComponent<CubeMovement>());
        if (_cubeRaycaster._gridColumnParent.StopFail)
        {
            enabled = false;
            return;
        }

        _target = _cubeRaycaster._gridColumnParent.targetGrid.transform;
        transform.parent = _target;
        LeanTween.delayedCall(gameObject, 0.05f * _cubeRaycaster._gridColumnParent.ListCount, CallFunction);
        transform.DOLocalMove(Vector3.zero, 0.15f * _cubeRaycaster._gridColumnParent.ListCount).SetEase(Ease.OutBounce)
            .OnComplete(
                () =>
                {
                    _cubeRaycaster._gridColumnParent.targetGrid.AssignTempCube(gameObject);
                    CubeManager.Instance.CreateCubeAgain?.Invoke();
                });

        _cubeRaycaster._gridColumnParent.targetGrid.isEmpty = false;
        StopAll = true;
    }

    void CallFunction() => _cubeRaycaster._gridColumnParent.ShakeIt();
}