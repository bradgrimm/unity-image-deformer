using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class WhiteBalanceRandomizer : VolumeParameterRandomizer<WhiteBalance>
{
    public Vector2 temperatureRange = new Vector2(-100.0f, 100.0f);
    public Vector2 tintRange = new Vector2(-100.0f, 100.0f);

    public override void Randomize()
    {
        WhiteBalance balance = GetVolumeParameter();
        balance.active = true;
        if (Random.value > 0.5)
        {
            balance.tint.overrideState = false;
            balance.temperature.overrideState = true;
            balance.temperature.value = Random.Range(temperatureRange.x, temperatureRange.y);
        }
        else
        {
            balance.temperature.overrideState = false;
            balance.tint.overrideState = true;
            balance.tint.value = Random.Range(tintRange.x, tintRange.y);
        }
    }
}
