using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Renderer))]
public class MaterialRandomizer : ComponentRandomizer<Renderer>
{
    public override void Randomize()
    {
        Material material = GetComponent().material;

        // Clear old textures
        material.SetTexture("_BaseColorMap", null);
        material.SetTexture("_NormalMap", null);
        material.SetTexture("_HeightMap", null);
        material.SetTexture("_MaskMap", null);
        material.SetTexture("_EmissiveColorMap", null);

        // Randomly pick new one.
        string[] paths = AssetDatabase.GetSubFolders("Assets/ImageDeformer/Textures/Flooring");
        int idx = Random.Range (0, paths.Length - 1);
        string[] assets = AssetDatabase.FindAssets("", new[] {paths[idx]});
        for (int i = 0; i < assets.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(assets[i]);
            Texture texture = (Texture) AssetDatabase.LoadAssetAtPath<Texture>(path);
            if (path.Contains("Color"))
                material.SetTexture("_BaseColorMap", texture);
            else if (path.Contains("Normal"))
                material.SetTexture("_NormalMap", texture);
            else if (path.Contains("Displacement"))
                material.SetTexture("_HeightMap", texture);
            else if (path.Contains("MASK"))
                material.SetTexture("_MaskMap", texture);
            else if (path.Contains("_emi"))
                material.SetTexture("_EmissiveColorMap", texture);
        }
    }
}
