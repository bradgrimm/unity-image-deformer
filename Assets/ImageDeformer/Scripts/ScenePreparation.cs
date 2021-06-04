using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScenePreparation : MonoBehaviour
{
    public Transform parentForCreatedObjects;
    public BoxCollider visibleRegion;
    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    public bool modifyMaterialColor = true;
    public Material[] randomizeMaterials;

    private string[] assetPaths = new string[0];
    private float objScale = 1.0f;
    private Randomizer[] randomizers = new Randomizer[0];

    void Start()
    {
        InitAssetPaths();
        InitRandomizers();
    }

    void InitAssetPaths()
    {
        if (assetPaths.Length == 0)
        {
            string[] assetGuids = AssetDatabase.FindAssets("", new[] {"Assets/ImageDeformer/Models/First30"});
            assetPaths = new string[assetGuids.Length];
            for (int i = 0; i < assetGuids.Length; ++i)
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
        }
    }

    void InitRandomizers()
    {
        //if (randomizers.Length == 0)
        //{
        randomizers = Resources.FindObjectsOfTypeAll<Randomizer>();
        //}
    }

    public void GenerateScene()
    {
        InitRandomizers();
        //CreateModel();
        RandomizeEverything();
    }

    public void CreateModels()
    {
        for (int i = 0; i < 1000; ++i)
        {
            CreateSingleModel();
        }
    }

    public void CreateSingleModel()
    {
        InitAssetPaths();

        int idx = Random.Range (0, assetPaths.Length - 1);
        string assetPath = assetPaths[idx];
        //string assetPath = "Assets/ImageDeformer/Models/3001.obj";
        GameObject model = (GameObject) AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        model.transform.localScale = new Vector3(objScale, objScale, objScale);
        Vector3 startPoint = RandomPointInBounds(visibleRegion.bounds);
        GameObject obj = Instantiate(model, startPoint, Random.rotation, parentForCreatedObjects);

        // Create object color.
        if (modifyMaterialColor)
        {
            string shaderName = "HDRP/Lit";
            Material material = new Material(Shader.Find(shaderName));
            Color matColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            // material.SetFloat("_Smoothness", 0.9f);
            // if (isTransparent)
            // {
            //     matColor.a = Random.Range(0.1f, 0.5f);
            //     StandardShaderUtils.ChangeRenderMode(material, StandardShaderUtils.BlendMode.Transparent);
            //     material.SetFloat("_SurfaceType", 1.0f);
            // }

            var block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", matColor);
            obj.GetComponentInChildren<Renderer>().SetPropertyBlock(block);
        }

        obj.AddComponent<ObjectAnnotator>();

        // Assign material and make collidable.
        Rigidbody body = obj.AddComponent<Rigidbody>();
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // meshRenderer.material = material;
            MeshCollider collider = meshRenderer.gameObject.AddComponent<MeshCollider>();
            collider.convex = true;
        }
    }

    public void SetMaterial()
    {
        foreach (Material material in randomizeMaterials)
        {
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

    public void RandomizeEverything()
    {
        foreach (Randomizer randomizer in randomizers)
        {
            bool enabled = randomizer.triggerEnabled;
            float c = randomizer.triggerChance;
            float v = Random.Range(0.0f, 1.0f);
            if (enabled && c != 0.0f && v <= c)
                randomizer.Randomize();
            else
                randomizer.Default();
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
