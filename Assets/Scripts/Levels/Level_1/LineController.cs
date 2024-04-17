using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lr;
    public Transform[] points;
    

    void Start()
    {
        lr.positionCount = points.Length;
    }

    void Update()
    {
        for(int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].localPosition);
        }
    }
}
