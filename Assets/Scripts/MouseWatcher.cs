using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MouseWatcher : MonoBehaviour
{
    private bool _wasLMBDown = false;
    // ReSharper disable once InconsistentNaming
    public UnityEvent LMBListener;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_wasLMBDown)
        {
            LMBListener.Invoke();
        }
    }
}