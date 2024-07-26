using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class SecurityCheck : MonoBehaviour
{
    public Door door;
    public bool isHackingConsole = false;
    public GameObject hand;
    public bool isHandOn;
    public TMP_InputField inputField;
    public event EventHandler OnPuzzleComplete;
    public TextMeshProUGUI consoleText;

    public List<string> functionNames;
    public List<string> functionDisplay;
    public List<bool> isOnFunctionPage;
    public List<string> commandNames;

    [Header("Player Text")]
    public string displayText;
    public float displayTime;
    public string displayText2;
    public float displayTime2;

    [Header("Puzzle")]
    public bool isAdmin;
    public bool isVerified;
    public bool isbyPassed = true;


    bool _usedHand = false;
    PlayerTextDisplay _playerTextDisplay;
    Inventory_System inv;
    AudioSource _source;


    void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
        _playerTextDisplay = inv.GetComponent<PlayerTextDisplay>();
        _source = GetComponent<AudioSource>();
        
        inputField.onEndEdit.AddListener(OnUserInputEnter);
    }

    // Main stuff
    public IEnumerator StartupConsole()
    {
        yield return new WaitForSeconds(2f);

        consoleText.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);
        consoleText.text = "Starting System...";
        yield return new WaitForSeconds(3f);

        consoleText.text += "Startup Complete.";
        inputField.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(2f);

        consoleText.text = "Please place your hand on the biometric scanner.";
    }

    public IEnumerator ScanHand()
    {
        yield return new WaitForSeconds(2);
        consoleText.text = "Scanning Hand...";
        yield return new WaitForSeconds(3);
        consoleText.text = "Welcome Back Jake Thompson.";
        consoleText.text += "\nPlease answer the following security question set by you.";
        yield return new WaitForSeconds(2);
        consoleText.text += "\n --- \nWho is your wife?";

        StartCoroutine(_playerTextDisplay.DisplayPlayerText(displayText2, displayTime2));
    }
    void OnMouseDown()
    {
        bool doesHaveItem = inv.CheckForItem("Thompsons_Hand");
        if(doesHaveItem)
        {
            inv.RemoveItem("Thompsons_Hand");
            hand.SetActive(true);
            isHandOn = true;
            StartCoroutine(ScanHand());
            _usedHand = true;            
        }
        else if(!_usedHand)
        {
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(displayText, displayTime));
        }
    }
    // Main stuff

    public void OnUserInputEnter(string text)
    {
        if(text == "") return;
        if(isbyPassed) StartCoroutine(ShowErrorMessage("System down...", Color.red));
        else if(text.Contains("$")) FindFakeFunction(text);
        else StartCoroutine(ShowErrorMessage("Not valid!", Color.red));
    }

    private void FindFakeFunction(string text)
    {
        text = text.ToLower();

        if(isOnFunctionPage[3] && text.Contains("$")) // 3- admin page
        {
            if(consoleText.text != functionDisplay[3] && text.Contains(commandNames[0])) consoleText.text = functionDisplay[3]; // 0- $forceRequest
            else if(consoleText.text == functionDisplay[3] && text.Contains(commandNames[3])) // 3- $forceSkip
            {
                isAdmin = true;
                consoleText.text = "Admin access granted.";
            } 
            else StartCoroutine(ShowErrorMessage("Error: Cannot run command", Color.red));
            return;
        }
        else if(isOnFunctionPage[2] && text.Contains("$")) // 2- verification page
        {
            if(consoleText.text != functionDisplay[2] && text.Contains(commandNames[0])) consoleText.text = functionDisplay[2]; // 0- $forceReq

            else if(consoleText.text == functionDisplay[2] && text.Contains(commandNames[1])) consoleText.text = "Please enter your admin ID."; // 1- $yes
            else if(text.Contains(commandNames[2])) consoleText.text = "Process cancelled. Please return back or restart"; // 2- $no

            else if(consoleText.text == "Please enter your admin ID." && text.Contains(commandNames[3])) // 3- $forceSkip
            {
                consoleText.text = "Admin has been veiried for 1 hour only for security purpose's.";
                isVerified = true;
            }

            else StartCoroutine(ShowErrorMessage("Error: Cannot run command", Color.red));


            return;
        }
        else if(isOnFunctionPage[1] && text.Contains("$")) // 1- bypass page
        {
            if(consoleText.text != functionDisplay[1] && text.Contains(commandNames[1])) consoleText.text = functionDisplay[1]; // 1- $yes
            else if(text == commandNames[2]) consoleText.text = "Process cancelled"; // 2- $no

            else if(consoleText.text == functionDisplay[1] && text.Contains(commandNames[0])) consoleText.text = "Force Request Sent. \nPlease enter you name for verification."; // 0- $forceRequest
            else if(consoleText.text == "Force Request Sent. \nPlease enter you name for verification." && text.Contains(commandNames[3])) // 3- $forceSkip
            {
                consoleText.text = "System Bypassed. \nSystem files corrupted, shutting down.";
                OnPuzzleComplete?.Invoke(this, EventArgs.Empty);
                isbyPassed = true;

                door.OpenDoor();
                door.isLocked = true;
            }
            
            else StartCoroutine(ShowErrorMessage("Error: Cannot run command", Color.red));
            return;
        }

        // Functions
        int i = 0;
        if(text.Contains(functionNames[0])) // 1- $showCommands
        {
            consoleText.text = functionDisplay[0];
            i = 0;
        }
        else if(text.Contains(functionNames[1])) // 2- $bypass
        {
            if(!isAdmin || !isVerified)
            {
                StartCoroutine(ShowErrorMessage("Not authorised.", Color.red));
                return;
            }
            else
            {
                consoleText.text = "Are you sure you would like to bypass the current securit question? This could break the entire database.";
                i = 1;
            }
        }
        else if(text.Contains(functionNames[2])) // 3- $Verify
        {
            if(!isAdmin)
            {
                StartCoroutine(ShowErrorMessage("Not an authorised admin.", Color.red));
                return;
            }
            else if(isVerified)
            {
                StartCoroutine(ShowErrorMessage("You are verified for 1 hour.", consoleText.color));
                return;
            }
            else
            {
                consoleText.text = "Missing authorised Admin, Cannot send normal verification request.";
                i = 2;
            }
        }
        else if(text.Contains(functionNames[3])) // 4- $grantAdmin
        {
            if(!isAdmin)
            {
                consoleText.text = "Cannot send admin access request normally.";
                i = 3;
            }
            else
            {
                StartCoroutine(ShowErrorMessage("Error: Your Are Already Admin.", consoleText.color));
                return;
            }
        }
        else
        {
            StartCoroutine(ShowErrorMessage("Error: Could not run command/function.'", Color.red));
            return;
        }

        for(int j = 0; j < isOnFunctionPage.Count; j++) isOnFunctionPage[j] = false;
        isOnFunctionPage[i] = true;
    }

    public IEnumerator ShowErrorMessage(string text, Color color)
    {
        string _placeHolder = consoleText.text;
        Color _pladeHolderColor = consoleText.color;

        consoleText.text = text;
        consoleText.color = color;
        PlaySound();
        yield return new WaitForSeconds(3);

        consoleText.text = _placeHolder;
        consoleText.color = _pladeHolderColor;
    }

    void PlaySound()
    {
        _source.volume = UnityEngine.Random.Range(0.1f, 0.5f);
        _source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        _source.PlayOneShot(_source.clip);
    }
}
