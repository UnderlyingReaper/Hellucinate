using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Lvl1_FuseCode : MonoBehaviour
{
    public Telephone telephone;
    public int code;
    public FuseCode_Arrow[] arrows;
    public TextMeshProUGUI[] texts;
    public event EventHandler OnPuzzle1Complete;



    void Start()
    {
        telephone.OnCodeGenerated += OnCodeGenerate;
        foreach(FuseCode_Arrow arrow in arrows)
        {
            arrow.OnValyeChange += OnValueChange;
        }
    }

    public void OnCodeGenerate(object sender, Telephone.CodeGeneratedArgs e)
    {
        code = e.code;
    }
    public void OnValueChange(object sender, EventArgs e)
    {
        Debug.Log("Clicked");
        CheckNumber();
    }

    public void CheckNumber()
    {
        string combinedText = "";

        foreach(TextMeshProUGUI text in texts)
        {
            combinedText += text.text;
        }

        int writtenCode = int.Parse(combinedText);
        Debug.Log(writtenCode);

        if(writtenCode == code)
        {
            OnPuzzle1Complete?.Invoke(this, EventArgs.Empty);
            transform.DOLocalMoveY(50, 5).SetEase(Ease.InOutSine);
        }
    }
}
