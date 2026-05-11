using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EX_Actor_Text : MonoBehaviour
{
    public TMP_Text TMPText;
    public string text;
    
    public void SetText(string text)
    {
        this.text = text;
        TMPText.text = this.text;
    }

    public void SetText()
    {
        TMPText.text = text;
    }
}
