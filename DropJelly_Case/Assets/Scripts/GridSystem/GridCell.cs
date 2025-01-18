using UnityEngine;

public class GridCell : MonoBehaviour
{
    public GridStatusCheck leftNeighbor;
    public GridStatusCheck rightNeighbor;
    public GridStatusCheck upNeighbor;
    public GridStatusCheck downNeighbor;

    public GameObject topRightObj;
    public GameObject topLeftObj;
    public GameObject botRightObj;
    public GameObject botLeftObj;

    public void NullEveryObject()
    {
        topRightObj = null;
        topLeftObj = null;
        botRightObj = null;
        botLeftObj = null;
    }
}