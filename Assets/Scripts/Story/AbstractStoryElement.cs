using System;
using UnityEngine;
using System.Collections;

public abstract class AbstractStoryElement : MonoBehaviour
{
    internal delegate void Callback();

    internal Callback OnFinished;

    private void Awake()
    {
        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        OnActivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnDeactivate();
    }

    public abstract void OnActivate();

    public abstract void OnClick();

    public abstract void OnDeactivate();
}