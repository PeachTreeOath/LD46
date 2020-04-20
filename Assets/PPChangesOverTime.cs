using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPChangesOverTime : MonoBehaviour
{

 
    // properties of class
    public float bloom = 10f;
    public float saturation = 5f;

    Bloom bloomLayer = null;
    AmbientOcclusion ambientOcclusionLayer = null;
    ColorGrading colorGradingLayer = null;

    void Start()
    {
        /*PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        volume.profile.TryGetSettings(out ambientOcclusionLayer);
        volume.profile.TryGetSettings(out colorGradingLayer);

        // later in this class during handling and changing
        ambientOcclusionLayer.enabled.value = true;

        bloomLayer.enabled.value = true;
        bloomLayer.intensity.value = bloom;

        colorGradingLayer.enabled.value = true;
        colorGradingLayer.saturation.value = saturation;

        // ... with some more checks to disable it fully where
        // needed/ if 0*/
    }

    private void Update()
    {
        
    }

}
