using System.Collections.Generic;
using UnityEngine;

public class CastObject : MonoBehaviour
{
    private GameObject hitObject;
    private List<GameObject> objectGroup;
    List<GameObject> neighborObject;
    private void Start()
    {
        objectGroup = new List<GameObject>();
        neighborObject = new List<GameObject>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.IsGamePause() 
            && !GameManager.instance.IsGameWin() && GameManager.instance.IsGameStart())
        {
            hitObject = CastRay();
            if (hitObject != null)
            {
                objectGroup = NeighborObject(hitObject);
            }
            else
            {
                objectGroup.Clear();
            }
        }
    }

    private List<GameObject> NeighborObject(GameObject start)
    {
        neighborObject.Clear();
        neighborObject.Add(start);

        for (int i = 0; i < neighborObject.Count; i++)
        {
            CheckNeighbor(neighborObject[i]);
        }

        return neighborObject;
    }

    private void CheckNeighbor(GameObject objToCheck)
    {
        Vector3Int startPosition = GridCellManager.instance.GetObjCell(objToCheck.transform.position);

        Vector3Int nextUp = startPosition + Vector3Int.up;
        Vector3Int nextDown = startPosition + Vector3Int.down;
        Vector3Int nextLeft = startPosition + Vector3Int.left;
        Vector3Int nextRight = startPosition + Vector3Int.right;

        CookieType checkingType = objToCheck.GetComponent<Cookie>().GetCookieType();
        Check(nextUp, checkingType);
        Check(nextDown, checkingType);
        Check(nextLeft, checkingType);
        Check(nextRight, checkingType);
    }

    private void Check(Vector3Int position, CookieType typeToCheck)
    {
        Vector2 posToCheck = GridCellManager.instance.PositonToMove(position);

        Collider2D hit = Physics2D.OverlapPoint(posToCheck);
        if (hit)
        {
            Cookie cookie = hit.GetComponent<Cookie>();
            if (cookie != null)
            {
                if (cookie.GetCookieType() == typeToCheck)
                {
                    if (!neighborObject.Contains(hit.gameObject))
                    {
                        neighborObject.Add(hit.gameObject);
                    }
                }
            }
        }
    }

    public List<GameObject> GetObjectGroup()
    {
        return objectGroup;
    }

    public void ResetObjecs()
    {
        objectGroup.Clear();
    }

    private GameObject CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
