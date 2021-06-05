using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsRandomizer : Randomizer
{
    public Vector2 yRange = new Vector2(0.1f, 2.0f);
    public Vector2 ySpaceFactor = new Vector2(1.1f, 3.0f);
    public Vector2 blindSize = new Vector2(3.0f, 15.0f);
    public Vector2 yShift = new Vector2(-5.0f, 5.0f);
    public Vector2 zShift = new Vector2(-5.0f, 5.0f);
    public Vector2 blindCount = new Vector2(1, 10);
    public Vector2 shapeRange = new Vector2(0, 4);
    public float rotationChance = 0.35f;

    public override void Randomize()
    {
        Clear();
        gameObject.SetActive(true);
        float gy = Random.Range(yShift.x, yShift.y);
        float gz = Random.Range(zShift.x, zShift.y);
        float y = Random.Range(yRange.x, yRange.y);
        float ys = Random.Range(ySpaceFactor.x, ySpaceFactor.y);
        float zs = Random.Range(blindSize.x, blindSize.y);
        float z = Random.Range(blindSize.x, blindSize.y);
        float count = Random.Range(blindCount.x, blindCount.y);
        for (int i = 0; i < count; ++i)
        {
            GameObject cube = GameObject.CreatePrimitive((PrimitiveType) Random.Range(shapeRange.x, shapeRange.y));
            Collider[] colliders = cube.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
                DestroyImmediate(collider);
            cube.transform.parent = gameObject.transform;
            cube.transform.localPosition = new Vector3(0.0f, i * (ys * y) + gy, gz);
            cube.transform.localScale = new Vector3(0.1f, y, zs);
            if (Random.value < rotationChance)
                cube.transform.rotation = Random.rotation;
        }
    }

    public override void Default()
    {
        Clear();
        gameObject.SetActive(false);
    }

    void Clear()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
    }
}
