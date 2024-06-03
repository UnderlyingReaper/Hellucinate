using UnityEngine;

public class HellucinateCollisionTrigger : MonoBehaviour
{
    public bool allow = true;
    public Transform teleportPosition;
    public int numberOfBlinks, numberOfblinkToTeleport;

    Hellucinate _hellucinate;

    void Start()
    {
        _hellucinate = GameObject.FindGameObjectWithTag("Player").GetComponent<Hellucinate>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && allow)
        {
            allow = false;
            StartCoroutine(_hellucinate.StartHellucinate(numberOfBlinks, numberOfblinkToTeleport, teleportPosition.position));
        }
    }
}
