using UnityEngine;

public class CenterCell : MonoBehaviour
{
    private void Start()
    {
        SetCenterCell();
        this.gameObject.GetComponent<Cookie>().SetDefaultPos(this.transform.position);
    }
    private void SetCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(cellPos);
    }
}
