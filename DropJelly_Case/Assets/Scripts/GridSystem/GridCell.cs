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
}