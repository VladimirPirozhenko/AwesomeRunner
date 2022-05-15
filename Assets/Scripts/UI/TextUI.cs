using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))] 
public abstract class TextUIElement : MonoBehaviour
{
    protected TextMeshProUGUI textMeshUI;
    protected StringBuilder stringBuilder;
    protected int originalStringLength;
    void Awake()
    {
        textMeshUI = GetComponent<TextMeshProUGUI>();
        stringBuilder = new StringBuilder();
        stringBuilder.Append(textMeshUI.text);
        originalStringLength = stringBuilder.Length;
    }
}
