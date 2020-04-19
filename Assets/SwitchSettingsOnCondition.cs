using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchSettingsOnCondition : MonoBehaviour
{
    // Start is called before the first frame update
    public float amount = 0.25f;
    public float flick = 0.85f;
    private float t = 0;
    private bool boldEnable = false;
    private TextMeshProUGUI text;
    public Slider value;

    void Start()
    {
        t = 0;
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (value.value < amount)
        {
            if (t > flick)
            {
                if (boldEnable)
                {
                    text.fontStyle = FontStyles.Italic;
                    boldEnable = false;
                    t = 0;
                }
                else
                {
                    text.fontStyle = FontStyles.Bold;
                    text.fontStyle = FontStyles.Italic;
                    boldEnable = true;
                    t = 0;
                }
            }
            else
            {
                t += Time.deltaTime;
            }
                

        }

    }
}
