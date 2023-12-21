using UnityEngine;

public enum CookieType
{
    Honey,
    Caro,
    Normal,
    Chocolate,
}
public class Cookie : MonoBehaviour
{
    [SerializeField]
    private CookieType cookieType;
    private Vector2 defaultPos;

    [SerializeField]
    private Sprite[] cookiesImg;

    #region Getter
    public CookieType GetCookieType()
    {
        return cookieType;
    }
    #endregion

    #region Setter
    public void SetCookieType(CookieType cookieType)
    {
        this.cookieType = cookieType;
        this.GetComponent<SpriteRenderer>().sprite = cookiesImg[(int)cookieType];
    }
    #endregion

    public void SetDefaultPos(Vector2 defaultPos)
    {
        this.defaultPos = defaultPos;
    }

    public Vector2 GetDefaultPos()
    {
        return defaultPos;
    }

    public void BackToDefaultPos()
    {
        this.transform.position = defaultPos;
    }
}
