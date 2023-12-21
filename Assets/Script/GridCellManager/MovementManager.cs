using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;
    [SerializeField]
    private AxisMovementInput axisMovementInput;
    [SerializeField]
    private CastObject castObject;

    #region Variables
    [SerializeField]
    private Vector2 moveDirection;
    [SerializeField]
    private List<GameObject> objectGroup;
    [SerializeField]
    private GameObject cookiePrefab;
    [SerializeField]
    private GameObject containerPrefab;
    [SerializeField]
    private Transform cookieContainer;
    [SerializeField]
    private Dictionary<GameObject, Vector3Int> cookiesInit;
    [SerializeField]
    private int numberOfCookies;
    #endregion


    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        cookiesInit = new Dictionary<GameObject, Vector3Int>();
    }

    public void SetContainer(Transform container)
    {
        this.cookieContainer = container;
    }

    public void DevideTheCake()
    {
        float DelayTime = 1.5f / cookieContainer.childCount;
        for (int i = 0; i < cookieContainer.childCount; i++)
        {
            cookieContainer.GetChild(i).transform.position = new Vector3(0, -6, 0);
            cookieContainer.GetChild(i).transform.DOMove(cookieContainer.GetChild(i).GetComponent<Cookie>().GetDefaultPos(), 0.5f).SetDelay(DelayTime * i);
            numberOfCookies++;
        }
    }

    private void Update()
    {
        moveDirection = axisMovementInput.GetMoveDirection();
        objectGroup = castObject.GetObjectGroup();
        if (objectGroup.Count > 0 && moveDirection != Vector2.zero)
        {
            GameObject edgeCookie = getEdgeObject();
            UnFold(edgeCookie, moveDirection, objectGroup);
            GameManager.instance.DisableHand();
            CheckWin();

            axisMovementInput.ResetDirection();
            castObject.ResetObjecs();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GridCellManager.instance.GetObjCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }

    private void CheckWin()
    {
        if (GridCellManager.instance.GetCellsPosition().Count == numberOfCookies)
        {
            GameManager.instance.Win();
        }
    }

    private void UnFold(GameObject start, Vector2 direction, List<GameObject> unfoldObjs)
    {
        cookiesInit.Clear();
        int numberOfCookies = unfoldObjs.Count;
        Transform newContainer = Instantiate(containerPrefab, cookieContainer).transform;
        newContainer.position = start.transform.position;
        for (int i = 0; i < numberOfCookies; i++)
        {
            if (!CreateCookie(start, unfoldObjs[i], direction, newContainer))
            {
                DestoryCookies(cookiesInit);
                break;
            }
        }
        this.numberOfCookies += cookiesInit.Count;
        ProcessInitializedCookie(newContainer, direction);
    }

    private void ProcessInitializedCookie(Transform newContainer, Vector2 direction)
    {
        if (cookiesInit.Count > 0)
        {
            if (direction.x != 0)
            {
                newContainer.rotation = Quaternion.Euler(0, 180 * direction.x, 0);
            }
            else if (direction.y != 0)
            {
                newContainer.rotation = Quaternion.Euler(180 * direction.y, 0, 0);
            }
            newContainer.DORotate(new Vector3(0, 0, 0), 0.5f);
        }
        else
        {
            Destroy(newContainer.gameObject);
        }
    }

    private void DestoryCookies(Dictionary<GameObject, Vector3Int> cookies)
    {
        foreach (KeyValuePair<GameObject, Vector3Int> cookie in cookies)
        {
            GridCellManager.instance.RemovePlacedCell(cookie.Value);
            Destroy(cookie.Key);
        }
        cookies.Clear();
    }

    private bool CreateCookie(GameObject start, GameObject cookie, Vector2 direction, Transform container)
    {
        GameObject newCookie = Instantiate(cookiePrefab, container);
        newCookie.GetComponent<Cookie>().SetCookieType(cookie.GetComponent<Cookie>().GetCookieType());
        Vector3Int startPos = GridCellManager.instance.GetObjCell(start.transform.position);
        Vector3Int cookiePos = GridCellManager.instance.GetObjCell(cookie.transform.position);
        newCookie.name = "Clone of " + cookie.name;
        int newX = 0;
        int newY = 0;
        if (direction.x != 0)
        {
            newX = cookiePos.x - (cookiePos.x - startPos.x) * 2 + (int)direction.x;
            newY = cookiePos.y;
        }
        else if (direction.y != 0)
        {
            newX = cookiePos.x;
            newY = cookiePos.y - (cookiePos.y - startPos.y) * 2 + (int)direction.y;
        }

        Vector3Int newCookiePos = new Vector3Int(newX, newY, 0);
        newCookie.transform.position = GridCellManager.instance.PositonToMove(newCookiePos);
        newCookie.GetComponent<Cookie>().SetDefaultPos(newCookie.transform.position);

        if (GridCellManager.instance.IsPlaceableArea(newCookiePos) && !GridCellManager.instance.IsCellPaced(newCookiePos))
        {
            cookiesInit.Add(newCookie, newCookiePos);
            GridCellManager.instance.PlacedCell(newCookiePos);
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameObject getEdgeObject()
    {
        if (moveDirection.x != 0)
        {
            objectGroup = objectGroup.OrderBy(obj => obj.transform.position.x).ToList();
            if (moveDirection.x > 0)
            {
                return objectGroup[objectGroup.Count - 1];
            }
            else
            {
                return objectGroup[0];
            }
        }
        else if (moveDirection.y != 0)
        {
            objectGroup = objectGroup.OrderBy(obj => obj.transform.position.y).ToList();
            if (moveDirection.y > 0)
            {
                return objectGroup[objectGroup.Count - 1];
            }
            else
            {
                return objectGroup[0];
            }
        }

        return null;
    }

}
