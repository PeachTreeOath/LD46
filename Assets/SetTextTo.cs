using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTextTo : MonoBehaviour
{
    public string defaultText;
    private TextMeshProUGUI tmp;


    private void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTextToString(string text)
    {
        tmp.text = text;
    }
}
