using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class ChromaticAberrationRandomizer : VolumeParameterRandomizer<ChromaticAberration>
{
    public Vector2 intensityRange = new Vector2(0.1f, 1.0f);

    public override void Randomize()
    {
        ChromaticAberration aberration = GetVolumeParameter();
        aberration.active = true;
        aberration.intensity.value = Random.Range(intensityRange.x, intensityRange.y);
    }
}
