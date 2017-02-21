using System;
using UnityEngine;
using System.Collections;

public class MouseWatcher : MonoBehaviour
{
    private Boolean _wasMouseDown;

    // Use this for initialization
    void Start()
    {
        _wasMouseDown = false;
    }

    // Update is called once per frame
    void Update()
    {
//        bool mouseDown = Input.GetButtonDown("Fire1");
//        if (!_wasMouseDown && mouseDown)
//        {
//            gameObject.GetComponent<LauncherScript>().OnClick();
//        }
//        _wasMouseDown = mouseDown;
    }
}