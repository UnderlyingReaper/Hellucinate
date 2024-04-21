using UnityEngine;

public class Camera_MouseFollow : MonoBehaviour
{
    public float moveSpeed;
    public float maxDistanceFromCenter;


    Vector3 _targetPosition;
    Camera _camera;
    Transform _camHolder;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _camHolder = GetComponentInParent<Transform>();
    }


    void Update()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camHolder.position.z));
        transform.position = new Vector3(mousePos.x, mousePos.y, -0.001f) * moveSpeed;
        Debug.Log(mousePos);
    }
}
