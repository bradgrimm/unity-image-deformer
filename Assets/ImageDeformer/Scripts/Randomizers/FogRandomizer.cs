using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class FogRandomizer : VolumeParameterRandomizer<Fog>
{
    public Vector2 distanceRange = new Vector2(5.0f, 30.0f);

    public override void Randomize()
    {
        Fog fog = (Fog) GetVolumeParameter();
        fog.active = true;
        fog.enabled.value = true;
        fog.meanFreePath.value = Random.Range(distanceRange.x, distanceRange.y);
        fog.enableVolumetricFog.value = true;
        fog.albedo.value = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
    }
}
