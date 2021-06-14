using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialRandomizer : ComponentRandomizer<Renderer>
{
    private bool madeChanges = false;

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
        Texture[] textures = WorldData.Instance.RandomTextureGroup();
        for (int i = 0; i < textures.Length; ++i)
        {
            string name = textures[i].name;
            if (name.Contains("Color"))
                material.SetTexture("_BaseColorMap", textures[i]);
            else if (name.Contains("Normal"))
                material.SetTexture("_NormalMap", textures[i]);
            else if (name.Contains("Displacement"))
                material.SetTexture("_HeightMap", textures[i]);
            else if (name.Contains("MASK"))
                material.SetTexture("_MaskMap", textures[i]);
            else if (name.Contains("_emi"))
                material.SetTexture("_EmissiveColorMap", textures[i]);
        }
        madeChanges = true;
    }

    public void OnDestroy()
    {
        if (madeChanges)
            Destroy(GetComponent().material);
    }
}
