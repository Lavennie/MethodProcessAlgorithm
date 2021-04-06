using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IntegratedParameterWheel : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int angleOffset = 0;
    public bool flip = false;
    public UnityEvent<Vector2, float> onChanged;
    [SerializeField] private Transform pointer;

    private bool hovered = false;

    private void Update()
    {
        if (hovered && Input.mouseScrollDelta.y != 0)
        {
            PointToDirection(Quaternion.Euler(0, 0, 45 * -Input.mouseScrollDelta.y) * pointer.right);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointToMouse();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
        PointToMouse();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void PointToMouse()
    {
        PointToDirection((Vector2)Input.mousePosition - (Vector2)pointer.position);
    }
    public void PointToDirection(Vector2 pointDirection)
    {
        if (flip)
        {
            PointToAngle(-((int)(Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg) - angleOffset));
        }
        else
        {
            PointToAngle((int)(Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg) - angleOffset);
        }

    }
    public void PointToAngle(int angle)
    {
        // snap it to 45 deg angles
        angle = (int)Mathf.Repeat(angle + 180, 360) - 180;
        angle = Mathf.RoundToInt(angle / 45.0f) * 45;
        int pointAngle = (angle * (flip ? -1 : 1) + angleOffset) ;

        Vector2 pointDirection = new Vector2(Mathf.Cos(pointAngle * Mathf.Deg2Rad), Mathf.Sin(pointAngle * Mathf.Deg2Rad));

        pointDirection.Normalize();
        pointer.right = pointDirection;

        onChanged.Invoke(pointDirection, angle);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }
}
