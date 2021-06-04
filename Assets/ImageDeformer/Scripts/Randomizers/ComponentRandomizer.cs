using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class ComponentRandomizer<T> : Randomizer
{
    T component;

    public override void Default()
    {
        gameObject.SetActive(false);
    }

    public override void Randomize()
    {
        gameObject.SetActive(true);
    }

    public T GetComponent()
    {
        if (component == null)
            component = GetComponent<T>();
        return component;
    }
}
