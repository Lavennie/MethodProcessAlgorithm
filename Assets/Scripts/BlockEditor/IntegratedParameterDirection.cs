using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class IntegratedParameterDirection : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform pointer;
    [SerializeField] private TextMeshProUGUI xText;
    [SerializeField] private TextMeshProUGUI yText;

    public void SetX(float value) { xText.text = string.Format("{0:0.0#}", value); }
    public void SetY(float value) { yText.text = string.Format("{0:0.0#}", value); }

    public float GetX()
    {
        // 8203 is some unwanted character that is at end
        return float.Parse(xText.text.Trim((char)8203)); 
    }
    public float GetY()
    {
        // 8203 is some unwanted character that is at end
        return float.Parse(yText.text.Trim((char)8203)); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pointDirection = (Vector2)Input.mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        // snap it to 45 deg angles
        angle = Mathf.RoundToInt(angle / 45) * 45;
        pointDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        pointer.right = pointDirection.normalized;
        SetX(pointer.right.x);
        SetY(pointer.right.y);
    }
}
