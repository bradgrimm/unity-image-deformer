using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnnotator : MonoBehaviour
{
    public bool showBounds = false;
    public Color color;
    List<Ray> debugRays = new List<Ray>();

    public void Update()
    {
        foreach (Ray ray in debugRays)
            Debug.DrawRay(ray.origin, ray.direction);
    }

    public void OnGUI()
    {
        if (showBounds) 
        {
            Rect rect = CalculateBoundingBox();
            if (rect.height == 0 || rect.width == 0) return;
            UnityEditor.Handles.BeginGUI();
            UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.red);
            UnityEditor.Handles.EndGUI();
        }
    }

    public Rect CalculateBoundingBox()
    {
        Camera camera = Camera.main;
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        return CalculateBoundingBoxForCamera(camera, mesh);
    }

    public Rect CalculateBoundingBoxForCamera(Camera camera, Mesh mesh)
    {
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        Bounds bounds = collider.bounds;

        if (!IsObjectInViewFrustrum(camera, bounds))
            return Rect.zero;

        (Vector2 min, Vector2 max, Vector3[] extents) = FindPreciseMeshExtents(camera, mesh);

        List<Vector3> raysToCast = new List<Vector3>(extents);
        raysToCast.Add(bounds.center);
        if (IsBehindAnotherObject(camera.transform.position, raysToCast))
            return Rect.zero;

        Rect rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        rect.xMin = Mathf.Clamp(rect.xMin, 0, camera.pixelWidth);
        rect.xMax = Mathf.Clamp(rect.xMax, 0, camera.pixelWidth);
        rect.yMin = Mathf.Clamp(rect.yMin, 0, camera.pixelHeight);
        rect.yMax = Mathf.Clamp(rect.yMax, 0, camera.pixelHeight);
        return rect;
    }

    public (Vector2, Vector3, Vector3[]) FindPreciseMeshExtents(Camera camera, Mesh mesh)
    {
        // Transform to screen space.
        Vector3[] origVertices = mesh.vertices;
        Vector3[] vertices = new Vector3[origVertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            origVertices[i] = transform.TransformPoint(origVertices[i]);
            vertices[i] = camera.WorldToScreenPoint(origVertices[i]);
            vertices[i].y = camera.pixelHeight - vertices[i].y;
        }

        Vector3[] extentVerts = new Vector3[4];
        for (int i = 0; i < extentVerts.Length; ++i)
            extentVerts[i] = vertices[0];
        
        // Find min/max according to screen.
        Vector3 v = vertices[0];
        Vector2 min = new Vector2(v.x, v.y);
        Vector2 max = new Vector2(v.x, v.y);
        for (int i = 1; i < vertices.Length; i++)
        {
            v = vertices[i];
            if (v.x < min.x)
            {
                min.x = v.x;
                extentVerts[0] = origVertices[i];
            }
            if (v.y < min.y)
            {
                min.y = v.y;
                extentVerts[1] = origVertices[i];
            }
            if (v.x > max.x)
            {
                max.x = v.x;
                extentVerts[2] = origVertices[i];
            }
            if (v.y > max.y)
            {
                max.y = v.y;
                extentVerts[3] = origVertices[i];
            }
        }
        return (min, max, extentVerts);
    }

    public bool IsBehindAnotherObject(Vector3 lookAt, List<Vector3> checkVerts, bool showRays=false)
    {
        if (showRays)
            debugRays = new List<Ray>();
            
        int isClear = 0;
        foreach (Vector3 v in checkVerts)
        {
            Vector3 dir = lookAt - v;
            dir.Normalize();
            if (!Physics.Raycast(v, dir))
                isClear += 1;
            if (showRays)
                debugRays.Add(new Ray(v, dir));
        }
        return isClear < 3;
    }

    public bool IsObjectInViewFrustrum(Camera camera, Bounds bounds)
    {
        Vector3 screenPoint = camera.WorldToScreenPoint(bounds.center);
        if (screenPoint.z < 0)
            return false;

        Vector3 c = bounds.center;
        Vector3 e = bounds.extents;
        Vector2[] extentPoints = new Vector2[9] {
            screenPoint,
            camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y - e.y, c.z - e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y - e.y, c.z - e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y - e.y, c.z + e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y - e.y, c.z + e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y + e.y, c.z - e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y + e.y, c.z - e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y + e.y, c.z + e.z)),
            camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y + e.y, c.z + e.z))
        };

        int width = camera.pixelWidth;
        int height = camera.pixelHeight;
        foreach (Vector2 pt in extentPoints)
        {
            if ((pt.x >= 0) && (pt.x <= width) && (pt.y >= 0) && (pt.y <= height))
                return true;
        }
        return false;
    }
}
