using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class ShadowsMidtonesHighlightsRandomizer : VolumeParameterRandomizer<ShadowsMidtonesHighlights>
{
    public Vector2 shadowRange = new Vector2(0.5f, 3.0f);
    public Vector2 midRange = new Vector2(0.5f, 3.0f);
    public Vector2 highlightRange = new Vector2(0.5f, 3.0f);

    public override void Randomize()
    {
        ShadowsMidtonesHighlights smh = GetVolumeParameter();
        smh.active = true;
        //smh.shadows.value = 0.5f;
    }
}
