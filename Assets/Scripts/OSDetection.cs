using UnityEngine;

public class OSDetection : MonoBehaviour
{
    public OSType currOS;
    public GameObject androidControls;
    public GameObject windowsSpeciciSettings;

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
            if(androidControls != null) androidControls.SetActive(true);
            if(windowsSpeciciSettings != null) windowsSpeciciSettings.SetActive(false);
        }
        else if(systemInfo.Contains("Windows")) currOS = OSType.Windows;
        else if(systemInfo.Contains("Linux")) currOS = OSType.Linux;
    }
}
