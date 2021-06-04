using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;

[RequireComponent(typeof(DecalProjector))]
public class DecalRandomizer : ComponentRandomizer<DecalProjector>
{
    public Vector2 opacityRange = new Vector2(0.05f, 0.55f);
    public Vector2 tilingRange = new Vector2(0.5f, 5.0f);
    public Vector2 offsetRange = new Vector2(0.0f, 1.0f);

    public override void Randomize()
    {
        base.Randomize();

        DecalProjector decal = GetComponent();
        float value = Random.Range(opacityRange.x, opacityRange.y);
        Color color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        decal.material.SetFloat("_DecalBlend", value);
        decal.material.SetColor("_BaseColor", color);

        float sx = Random.Range(tilingRange.x, tilingRange.y);
        float sy = Random.Range(tilingRange.x, tilingRange.y);
        decal.uvScale = new Vector2(sx, sy);

        float bx = Random.Range(offsetRange.x, offsetRange.y);
        float by = Random.Range(offsetRange.x, offsetRange.y);
        decal.uvBias = new Vector2(bx, by);
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
