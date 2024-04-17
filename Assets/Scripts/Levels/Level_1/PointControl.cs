using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PointControl : MonoBehaviour
{
    public Camera inspectCamera;
    public Type type;
    public Color color;

    public enum Type {
        Corner,
        End
    }
    public enum Color {
        Red,
        Blue,
        Green,
        Yellow,
    }

    Vector3 _screenPoint;
    void OnMouseDrag()
    {
        if(!enabled) return;
        _screenPoint = inspectCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -inspectCamera.transform.position.z));
        transform.position = new Vector3(_screenPoint.x, _screenPoint.y, -0.001f);
    }
}
