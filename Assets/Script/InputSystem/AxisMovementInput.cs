using UnityEngine;

public class AxisMovementInput : MonoBehaviour
{
    #region Variables
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private Vector2 moveDirection;
    #endregion

    private void Update()
    {
        if (!GameManager.instance.IsGamePause() && !GameManager.instance.IsGameWin() && GameManager.instance.IsGameStart())
        {
            InputHandler();
        }
    }

    #region Getters and Setters
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public void ResetDirection()
    {
        moveDirection = Vector2.zero;
    }
    #endregion

    #region Movement
    private void InputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(mouseDownPos, mouseUpPos) > 1f)
            {
                CalculateDirection(mouseDownPos, mouseUpPos);
            }
            else
            {
                moveDirection = Vector2.zero;
            }
        }
    }
    private void CalculateDirection(Vector2 startPos, Vector2 endPos)
    {
        if (Mathf.Abs(startPos.x - endPos.x) < .03f && Mathf.Abs(startPos.y - endPos.y) < .03f)
        {
            moveDirection = Vector2.zero;
            return;
        }
        if (Mathf.Abs(startPos.x - endPos.x) > Mathf.Abs(startPos.y - endPos.y))
        {
            if (startPos.x > endPos.x)
            {
                moveDirection = Vector2.left;
            }
            else
            {
                moveDirection = Vector2.right;
            }
        }
        else
        {
            if (startPos.y > endPos.y)
            {
                moveDirection = Vector2.down;
            }
            else
            {
                moveDirection = Vector2.up;
            }
        }
    }
    #endregion
}
