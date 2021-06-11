using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Randomizer : MonoBehaviour
{
    public bool triggerEnabled = true;
    public bool includeInAblations = true;
    public float triggerChance = 0.1f;

    public abstract void Default();
    public abstract void Randomize();

    public void ClearChildren()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
    }
}
