using UnityEngine;

public class EnableSpawner : MonoBehaviour
{
    public GameObject desiredSpawner;

    void Start()
    {
        desiredSpawner.SetActive(false);
    }

    public void EnableDesiredSpawner()
    {
        if(!desiredSpawner.activeSelf) desiredSpawner.SetActive(true);
    }
}
