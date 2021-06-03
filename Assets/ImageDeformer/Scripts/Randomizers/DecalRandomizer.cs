using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;

[RequireComponent(typeof(DecalProjector))]
public class DecalRandomizer : Randomizer
{
    public Vector2 opacityRange = new Vector2(0.05f, 0.55f);

    private DecalProjector decal;

    public override void Randomize()
    {
        if (decal == null)
            decal = GetComponent<DecalProjector>();
        float value = Random.Range(opacityRange.x, opacityRange.y);
        Color color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        decal.material.SetFloat("_DecalBlend", value);
        decal.material.SetColor("_BaseColor", color);
        SetMaterial(decal.material);
    }

    void SetMaterial(Material material)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] {"Assets/ImageDeformer/Textures/Flooring"});
        string path = AssetDatabase.GUIDToAssetPath(guids[Random.Range(0, guids.Length - 1)]);
        Texture texture = (Texture) AssetDatabase.LoadAssetAtPath<Texture>(path);
        material.SetTexture("_BaseColorMap", texture);
    }
}
