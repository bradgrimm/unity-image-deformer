using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class FilmGrainRandomizer : VolumeParameterRandomizer<FilmGrain>
{
    public Vector2 typeRange = new Vector2Int(0, 9);
    public Vector2 intensityRange = new Vector2(0.0f, 1.0f);

    public override void Randomize()
    {
        FilmGrain filmGrain = GetVolumeParameter();
        filmGrain.active = true;
        filmGrain.type.value = (FilmGrainLookup) Random.Range(typeRange.x, typeRange.y+1);
        filmGrain.intensity.value = Random.Range(intensityRange.x, intensityRange.y);
    }
}
