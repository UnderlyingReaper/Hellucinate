using UnityEngine;

public class EnableSpawner : MonoBehaviour
{
    public GameObject desiredSpawner;

    public void EnableDesiredSpawner()
    {
        if(!desiredSpawner.activeSelf) desiredSpawner.SetActive(true);
    }
}
