using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCheck : MonoBehaviour
{
    public bool isHackingConsole = false;
    public GameObject hand;
    public bool isHandOn;
    public Lvl1_UserInput customeInputField;
    public event EventHandler OnPuzzleComplete;
    public TextMeshProUGUI userText, consoleText;

    public List<string> functionNames;
    public List<string> functionDisplay;
    public List<bool> isOnFunctionPage;
    public List<string> commandNames;

    [Header("Puzzle")]
    public bool isAdmin;
    public bool isVerified;


    Inventory_System inv;


    void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
        customeInputField.SendText += OnUserInputEnter;
    }

    // Main stuff
    public IEnumerator StartupConsole()
    {
        yield return new WaitForSeconds(2f);

        consoleText.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);
        consoleText.text = "%cl'Starting System'";
        yield return new WaitForSeconds(2f);
        consoleText.text += "\n%dp 30%";
        yield return new WaitForSeconds(2f);
        consoleText.text += "\n%cl 'Starting User Input System'";
        yield return new WaitForSeconds(2f);

        consoleText.text += "\n%dp 100% \n%cl 'Startup Complete'";
        userText.text = "--YourText--";
        userText.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(1.5f);
        consoleText.text += "\n%sf clrscr";
        yield return new WaitForSeconds(2f);
        consoleText.text = "";
        userText.text = "";

        yield return new WaitForSeconds(3f);

        consoleText.text = "Please place your hand on the biometric scanner.";
    }

    public IEnumerator ScanHand()
    {
        yield return new WaitForSeconds(2);
        consoleText.text = "Scanning Hand...";
        yield return new WaitForSeconds(3);
        consoleText.text = "Welcome Jake Thompson.";
        consoleText.text += "\nPlease answer the following question";
        yield return new WaitForSeconds(2);
        consoleText.text += "\n --- \nWho is your wife?";
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
        }
    }
    // Main stuff

    public void OnUserInputEnter(object sender, Lvl1_UserInput.TextArgs e)
    {
        if(e.textWritten.Contains("%") || e.textWritten.Contains("$")) FindFakeFunction(e.textWritten);
        else StartCoroutine(ShowErrorMessage("Not valid!", Color.red));
    }

    private void FindFakeFunction(string text)
    {
        if(isOnFunctionPage[3] && !text.Contains("%")) // 3- admin page
        {
            if(consoleText.text != functionDisplay[3] && text == commandNames[0]) consoleText.text = functionDisplay[3]; // 0- $forceRequest@
            else if(consoleText.text == functionDisplay[3] && text == commandNames[3]) // 3- $forceSkip@
            {
                isAdmin = true;
                consoleText.text = "%cE 'Admin access granted!'";
            } 
            else StartCoroutine(ShowErrorMessage("%cE 'Cannot run command", Color.red));
            return;
        }

        // Functions
        int i = 0;
        if(text == functionNames[0]) // 1- %showFunc
        {
            consoleText.text = functionDisplay[0];
            i = 0;
        }
        else if(text == functionNames[1]) // 2- %byPass
        {
            if(!isAdmin || !isVerified)
            {
                StartCoroutine(ShowErrorMessage("%cE 'Not verified'", Color.red));
                return;
            }
        }
        else if(text == functionNames[2]) // 3- %vF
        {
            if(!isAdmin)
            {
                StartCoroutine(ShowErrorMessage("%cE 'Not an authorised admin", Color.red));
                return;
            }
        }
        else if(text == functionNames[3]) // 4- %gA
        {
            if(!isAdmin)
            {
                consoleText.text = "%cE 'Admin Access not allowed'";
                i = 3;
            }
            else StartCoroutine(ShowErrorMessage("%cL 'Already Admin", consoleText.color));
        }
        else
        {
            StartCoroutine(ShowErrorMessage("%cE 'Could not find command/function'", Color.red));
            return;
        }

        for(int j = 0; j < isOnFunctionPage.Count; j++) isOnFunctionPage[j] = false;
        isOnFunctionPage[i] = true;
    }

    public IEnumerator ShowErrorMessage(string text, Color color)
    {
        string _placeHolder = "";
        Color _pladeHolderColor = Color.white;

        _placeHolder = consoleText.text;
        _pladeHolderColor = consoleText.color;

        consoleText.text = text;
        consoleText.color = color;
        yield return new WaitForSeconds(3);

        consoleText.text = _placeHolder;
        consoleText.color = _pladeHolderColor;
        
    }
}
