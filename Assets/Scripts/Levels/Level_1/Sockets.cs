using System;
using UnityEngine;

public class Sockets : MonoBehaviour
{
    public Color color;
    public EventHandler OnConnected;

    public enum Color {
        Red,
        Blue,
        Green,
        Yellow,
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other);

        if(other.GetComponent<PointControl>() != null)
        {
            PointControl pointControl = other.GetComponent<PointControl>();
            Debug.Log((int)pointControl.color + (int)color);

            if((int)pointControl.color == (int)color && pointControl.type == PointControl.Type.End)
            {
                if(Input.GetKeyUp(KeyCode.Mouse0))
                {
                    OnConnected?.Invoke(this, EventArgs.Empty);
                    pointControl.transform.position = transform.position;
                    pointControl.enabled = false;
                    enabled = false;
                }
            }
        }
    }
}
