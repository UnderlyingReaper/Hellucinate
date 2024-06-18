using UnityEngine;

public class OSDetection : MonoBehaviour
{
    public OSType currOS;
    public GameObject androidControls;

    public enum OSType {
        Android,
        Windows,
        Linux
    }


    void Start()
    {
        androidControls.SetActive(false);

        string systemInfo = SystemInfo.operatingSystem;
        Debug.Log(systemInfo);

        if(systemInfo.Contains("Android"))
        {
            currOS = OSType.Android;
            androidControls.SetActive(true);
        }
        else if(systemInfo.Contains("Windows")) currOS = OSType.Windows;
        else if(systemInfo.Contains("Linux")) currOS = OSType.Linux;
    }
}
