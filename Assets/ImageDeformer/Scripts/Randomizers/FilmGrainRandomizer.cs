using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class FilmGrainRandomizer : Randomizer
{
    public Vector2 typeRange = new Vector2Int(0, 9);
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
            if(name.Contains("FilmGrain"))
            {
                FilmGrain filmGrain = (FilmGrain) components[i];
                filmGrain.active = true;
                filmGrain.type.value = (FilmGrainLookup) Random.Range(typeRange.x, typeRange.y+1);
                filmGrain.intensity.value = Random.Range(intensityRange.x, intensityRange.y);
            }
        }
    }
}
