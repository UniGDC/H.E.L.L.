using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D _thisBody;

    // Use this for initialization
    void Start()
    {
        _thisBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _thisBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}