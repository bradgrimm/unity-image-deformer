using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class BloomRandomizer : Randomizer
{
    public Vector2 thresholdRange = new Vector2(0.0f, 1.0f);
    public Vector2 intensityRange = new Vector2(0.0f, 1.0f);

    private Volume globalVolume;

    public override void Randomize()
    {
        if (globalVolume == null)
            globalVolume = GetComponent<Volume>();
        List<VolumeComponent> components = globalVolume.profile.components;
        for (int i = 0; i < components.Count; i++)
        {
            string name = components[i].name;
            if(name.Contains("Bloom"))
            {
                Bloom bloom = (Bloom) components[i];
                bloom.active = true;
                bloom.threshold.value = Random.Range(thresholdRange.x, thresholdRange.y);
                bloom.intensity.value = Random.Range(intensityRange.x, intensityRange.y);
            }
        }
    }
}
