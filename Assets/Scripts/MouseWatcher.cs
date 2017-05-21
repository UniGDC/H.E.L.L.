using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.Events;

public class MouseWatcher : MonoBehaviour
{
    // ReSharper disable once InconsistentNaming
    public UnityEvent LMBListener;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LMBListener.Invoke();
        }
    }
}