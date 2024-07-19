using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime;
    public float lookAheadOffset;
    public float maxHeightOffset;

    Vector3 _velocity;
    Player_Movement _pm;
    float _baseYValue;
    bool _lockYAxis;
    

    void Start()
    {
        _pm = target.GetComponent<Player_Movement>();
    }
    void Update()
    {
        if(_pm.rb.velocity.x == 0) offset.x = 0;
        else if(_pm.isFacingRight) offset.x = lookAheadOffset;
        else if(!_pm.isFacingRight) offset.x = -lookAheadOffset;

        Vector3 targetPos = target.position + offset;

        float relativePos = target.position.y - transform.position.y;
        if(relativePos >= maxHeightOffset || relativePos <= -maxHeightOffset) _lockYAxis = false;

        if(_lockYAxis) targetPos.y = _baseYValue;
        else
        {
            _baseYValue = targetPos.y;
            _lockYAxis = true;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, smoothTime);
    }
}
