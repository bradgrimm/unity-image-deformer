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

    private string[] assetPaths;
    private float objScale = 1 / 63.3f;

    void Start()
    {
        string[] assetGuids = AssetDatabase.FindAssets("", new[] {"Assets/ImageDeformer/Models"});
        assetPaths = new string[assetGuids.Length];
        for (int i = 0; i < assetGuids.Length; ++i)
        {
            assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
        }
    }

    public void GenerateScene()
    {
        Debug.Log("Randomizing new scene.");
        CreateModel();
    }

    public void CreateModel()
    {
        //int idx = Random.Range (0, assetPaths.Length);
        //string assetPath = assetPaths[idx];
        string assetPath = "Assets/ImageDeformer/Models/3001.obj";
        GameObject model = (GameObject) AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        model.transform.localScale = new Vector3(objScale, objScale, objScale);
        Vector3 startPoint = RandomPointInBounds(visibleRegion.bounds);
        GameObject obj = Instantiate(model, startPoint, Random.rotation, parentForCreatedObjects);

        // Create object color.
        if (modifyMaterialColor)
        {
            // bool isHDRP = GraphicsSettings.renderPipelineAsset != null;
            // string shaderName = isHDRP ? "HDRP/Lit" : "Standard";
            string shaderName = "Standard";
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

    public Material testMaterial;
    public void SetMaterial()
    {
        string[] assets = AssetDatabase.FindAssets("", new[] {"Assets/ImageDeformer/Textures/Flooring/WoodFloor001_4K-JPG"});
        for (int i = 0; i < assets.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(assets[i]);
            Texture texture = (Texture) AssetDatabase.LoadAssetAtPath<Texture>(path);
            if (path.Contains("Color"))
                testMaterial.SetTexture("_Color", texture);
            else if (path.Contains("Normal"))
                testMaterial.SetTexture("_BumpMap", texture);
            else if (path.Contains("Roughness"))
                testMaterial.SetTexture("_Glossiness", texture);
            else if (path.Contains("Occlusion"))
                testMaterial.SetTexture("_OcclusionMap", texture);
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        float ex = bounds.extents.x;
        float ez = bounds.extents.z;
        float xPos = Random.Range(-ex, ex);
        float xNeg = -Random.Range(-ex, ex);
        float zPos = Random.Range(-ez, ez);
        float zNeg = Random.Range(-ez, ez);
        return new Vector3(
            Random.Range(bounds.min.x + xNeg, bounds.max.x + xPos),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}
