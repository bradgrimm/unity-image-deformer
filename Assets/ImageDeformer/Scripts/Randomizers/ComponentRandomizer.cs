using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class ComponentRandomizer<T> : Randomizer
{
    public bool disableObjectOnDefault = false;
    
    T component;

    public override void Default()
    {
        if (disableObjectOnDefault)
            gameObject.SetActive(false);
    }

    public override void Randomize()
    {
        if (disableObjectOnDefault)
            gameObject.SetActive(true);
    }

    public T GetComponent()
    {
        if (component == null)
            component = GetComponent<T>();
        return component;
    }
}
