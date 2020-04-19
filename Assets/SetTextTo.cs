using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTextTo : MonoBehaviour
{
    public string defaultText;
    public TextMeshProUGUI tmp;



    private void Start()
    {

    }

    public void SetTextToString(string text)
    {
        Debug.Log(text);
        tmp.text = text;
    }
}
