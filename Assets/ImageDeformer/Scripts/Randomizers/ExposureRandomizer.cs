using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class ExposureRandomizer : Randomizer
{
    public Vector2 exposureRange = new Vector2(-1.0f, 6.0f);
    private Volume globalVolume;

    public override void Randomize()
    {
        if (globalVolume == null)
            globalVolume = GetComponent<Volume>();
        List<VolumeComponent> components = globalVolume.profile.components;
        for (int i = 0; i < components.Count; i++)
        {
            string name = components[i].name;
            if(name.Contains("Exposure"))
            {
                Exposure exposure = (Exposure) components[i];
                exposure.active = true;
                exposure.mode.value = ExposureMode.Fixed;
                float amt = Random.Range(exposureRange.x, exposureRange.y);
                exposure.fixedExposure.value = amt;
            }
        }
    }
}
