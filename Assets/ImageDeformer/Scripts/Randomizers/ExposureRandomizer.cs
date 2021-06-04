using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class ExposureRandomizer : VolumeParameterRandomizer<Exposure>
{
    public Vector2 exposureRange = new Vector2(-1.0f, 6.0f);

    public override void Randomize()
    {
        Exposure exposure = GetVolumeParameter();
        exposure.active = true;
        exposure.mode.value = ExposureMode.Fixed;
        float amt = Random.Range(exposureRange.x, exposureRange.y);
        exposure.fixedExposure.value = amt;
    }
}
