using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FuelUIBar : MonoBehaviour
{
    private Slider slider;
    public float fillSpeed = 0;
    private float targetProgress = 0;
    private ParticleSystem ps;
    // Start is called before the first frame update

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        ps = GameObject.Find("FuelParticles").GetComponent<ParticleSystem>();
    }

    void Start()
    {
        //IncrementProgress(0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
            if(!ps.isPlaying)
            {
                ps.Play();
            }    
        }
        else
        {
            ps.Stop();
        }
    }

    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
}
