using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRKeyboard : MonoBehaviour
{
    // temporary code for XR integration
    private TouchScreenKeyboard keyboard;

    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
}
