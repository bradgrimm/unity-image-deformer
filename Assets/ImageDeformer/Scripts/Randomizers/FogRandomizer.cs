using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class FogRandomizer : Randomizer
{
    public Vector2 distanceRange = new Vector2(5.0f, 30.0f);

    private Volume globalVolume;

    public override void Randomize()
    {
        if (globalVolume == null)
            globalVolume = GetComponent<Volume>();
        List<VolumeComponent> components = globalVolume.profile.components;
        for (int i = 0; i < components.Count; i++)
        {
            string name = components[i].name;
            if(name.Contains("Fog"))
            {
                Fog fog = (Fog) components[i];
                fog.active = true;
                fog.enabled.value = true;
                fog.maxFogDistance.value = Random.Range(distanceRange.x, distanceRange.y);
                fog.enableVolumetricFog.value = true;
                fog.albedo.value = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
}
