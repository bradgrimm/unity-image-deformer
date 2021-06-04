using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class BloomRandomizer : VolumeParameterRandomizer<Bloom>
{
    public Vector2 thresholdRange = new Vector2(0.0f, 1.0f);
    public Vector2 intensityRange = new Vector2(0.0f, 1.0f);

    public override void Randomize()
    {
        Bloom bloom = GetVolumeParameter();
        bloom.active = true;
        bloom.threshold.value = Random.Range(thresholdRange.x, thresholdRange.y);
        bloom.intensity.value = Random.Range(intensityRange.x, intensityRange.y);
    }
}
