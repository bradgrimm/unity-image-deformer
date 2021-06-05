using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    private static WorldData worldDataInstance;
    public static WorldData Instance
    {
        get {
            if (worldDataInstance == null)
                worldDataInstance = Object.FindObjectsOfType<WorldData>()[0];
            return worldDataInstance;
        }
    }

    public Texture[] textures;
    public string[] textureGroups;
    public GameObject[] models;

    public Texture RandomTexture()
    {
        return textures[Random.Range(0, textures.Length - 1)];
    }

    public Texture[] RandomTextureGroup()
    {
        string group = textureGroups[Random.Range(0, textureGroups.Length - 1)];
        List<Texture> groupTextures = new List<Texture>();
        foreach (Texture t in textures)
        {
            if (t.name.Contains(group))
                groupTextures.Add(t);
        }
        return groupTextures.ToArray();
    }

    public GameObject RandomModel()
    {
        return models[Random.Range(0, models.Length - 1)];
    }
}
