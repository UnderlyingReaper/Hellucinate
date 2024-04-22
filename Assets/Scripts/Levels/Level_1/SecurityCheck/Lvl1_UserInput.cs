using System;
using TMPro;
using UnityEngine;

public class Lvl1_UserInput : MonoBehaviour
{
    public bool allowuserInput = true;
    public TextMeshProUGUI userText;

    public event EventHandler<TextArgs> SendText;
    public class TextArgs : EventArgs {
        public string textWritten;
    }

    [Header("Settings")]
    string _inputString = "";
    public bool _isBackspaceHeld = false; // Flag to track if backspace is being held
    public float deleteDelay = 0.5f; // Delay before continuous deletion starts
    float _deleteTimer = 0f; // Timer for continuous deletion


    void Update()
    {
        if(_inputString != "" && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            SendText?.Invoke(this, new TextArgs { textWritten = userText.text });
            _inputString = "";
            userText.text = "";
            return;
        } 
        
        if(allowuserInput)
        {
            if (Input.anyKeyDown)
            {
                string keyPressed = Input.inputString;
                
                // Check if the input string is not empty
                if (!string.IsNullOrEmpty(keyPressed))
                {
                    if (keyPressed == "\b")
                    {
                        if (_inputString.Length > 0)
                            _inputString =_inputString.Substring(0, _inputString.Length - 1);
                    }
                    else _inputString += keyPressed; // Append the pressed key to the input string
                }

                userText.text = _inputString;
            }

            if (Input.GetKey(KeyCode.Backspace))
            {
                // Start the timer for continuous deletion
                if (!_isBackspaceHeld)
                {
                    _isBackspaceHeld = true;
                    _deleteTimer = Time.time + deleteDelay;
                }

                // Check if the continuous deletion timer has expired
                if (_isBackspaceHeld && Time.time > _deleteTimer)
                {
                    // Remove the last character from the input string
                    if (_inputString.Length > 0)
                    {
                        _inputString = _inputString.Substring(0, _inputString.Length - 1);
                        // Reset the continuous deletion timer
                        _deleteTimer = Time.time + deleteDelay;
                    }
                }

                userText.text = _inputString;
            }
            else _isBackspaceHeld = false; // Reset the flag and timer if backspace is released
        }
    }
}
