using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsRandomizer : Randomizer
{
    public BoxCollider visibleRegion;
    public Vector2 legoDensityRange = new Vector2(0.25f, 2.0f);
    public Vector2 smoothnessRange = new Vector2(0.0f, 1.0f);

    private float objScale = 1.0f;

    public override void Randomize()
    {
        ClearChildren();
        int nLegosToCreate = (int) (1000 * Random.Range(legoDensityRange.x, legoDensityRange.y));
        for (int i = 0; i < nLegosToCreate; ++i)
            CreateSingleModel();
    }

    public override void Default()
    {
        // Just leave the scene as is unless there isn't anything there.
        if (transform.childCount == 0)
            Randomize();
    }

    public void CreateSingleModel()
    {
        GameObject model = (GameObject) WorldData.Instance.RandomModel();
        model.transform.localScale = new Vector3(objScale, objScale, objScale);
        Vector3 startPoint = RandomPointInBounds(visibleRegion.bounds);
        GameObject obj = Instantiate(model, startPoint, Random.rotation, transform);

        // Create object color.
        string shaderName = "HDRP/Lit";
        Material material = new Material(Shader.Find(shaderName));
        Color matColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        var block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", matColor);
        block.SetFloat("_Smoothness", Random.Range(0.0f, 1.0f));
        block.SetFloat("_Metallic", Random.Range(0.0f, 1.0f));
        obj.GetComponentInChildren<Renderer>().SetPropertyBlock(block);

        // Assign material and make collidable.
        Rigidbody body = obj.AddComponent<Rigidbody>();
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            ObjectAnnotator annotator = meshRenderer.gameObject.AddComponent<ObjectAnnotator>();
            annotator.color = matColor;
            MeshCollider collider = meshRenderer.gameObject.AddComponent<MeshCollider>();
            collider.convex = true;
        }
    }

    void Clear()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
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
