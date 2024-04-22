using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SecurityCheck : MonoBehaviour
{
    public GameObject hand;
    public bool isHandOn;

    public event EventHandler OnPuzzleComplete;
    public TextMeshProUGUI userText, consoleText;


    Inventory_System inv;



    void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
    }



    public IEnumerator StartupConsole()
    {
        yield return new WaitForSeconds(2f);

        consoleText.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);
        consoleText.text = "%cl'Starting System';";
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
}
