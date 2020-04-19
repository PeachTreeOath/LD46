using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchSettingsOnInterval : MonoBehaviour
{
    // Start is called before the first frame update
    public float interval = 0.5f;
    private bool boldEnable = false;
    private float t;
    private TextMeshProUGUI text;
    
    void Start()
    {
        t = 0;
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(t > interval)
        {
            if (boldEnable)
            {
                text.fontStyle = FontStyles.Normal;
                boldEnable = false;
                t = 0;
            }
            else
            {
                text.fontStyle = FontStyles.Bold;
                boldEnable = true;
                t = 0;
            }

        }
        else
        {
            t += Time.deltaTime;
        }

        if(transform.position.z > 0f)
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else
        {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        
    }
}
