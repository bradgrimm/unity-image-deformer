using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldData), true)]
public class WorldDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh World Data"))
        {
            WorldData worldData = target as WorldData;
            RefreshTextures(worldData);
            RefreshModels(worldData);
        }
    }

    public void RefreshTextures(WorldData worldData)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] {"Assets/ImageDeformer/Textures/Flooring"});
        HashSet<string> textureGroups = new HashSet<string>();
        List<Texture> textures = new List<Texture>();
        for (int i = 0; i < guids.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Texture texture = (Texture) AssetDatabase.LoadAssetAtPath<Texture>(path);
            if (texture != null)
            {
                string textureGroup = texture.name.Split('_')[0];
                textureGroups.Add(textureGroup);
                textures.Add(texture);
            }
        }
        worldData.textures = textures.ToArray();
        worldData.textureGroups = new string[textureGroups.Count];
        textureGroups.CopyTo(worldData.textureGroups);
    }

    public void RefreshModels(WorldData worldData)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] {"Assets/ImageDeformer/Models"});
        List<GameObject> models = new List<GameObject>();
        for (int i = 0; i < guids.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject obj = (GameObject) AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (obj != null)
                models.Add(obj);
        }
        worldData.models = models.ToArray();
    }
}
