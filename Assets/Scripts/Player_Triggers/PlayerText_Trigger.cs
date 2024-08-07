using UnityEngine;

public class PlayerText_Trigger : MonoBehaviour
{
    public string text;
    public float initialDelay, displayDuration;


    PlayerTextDisplay _playerTextDisplay;

    void Start()
    {
        _playerTextDisplay = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTextDisplay>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!this.enabled) return;

        StartCoroutine(_playerTextDisplay.DisplayPlayerText(text, displayDuration, initialDelay));
        this.enabled = false;
    }
}
