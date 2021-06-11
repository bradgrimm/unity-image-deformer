using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class LensDistortionRandomizer : VolumeParameterRandomizer<LensDistortion>
{
    public Vector2 intensityRange = new Vector2(-0.5f, 0.5f);

    public override void Randomize()
    {
        LensDistortion distort = GetVolumeParameter();
        distort.active = true;
        distort.intensity.overrideState = true;
        distort.intensity.value = Random.Range(intensityRange.x, intensityRange.y);
    }
}
