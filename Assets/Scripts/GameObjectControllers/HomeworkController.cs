using System;
using UnityEngine;
using System.Collections;

public class HomeworkController : MonoBehaviour
{
    public Vector3 Target;

    public float SpeedModifier;

    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position)
            .MultiplyVector(new Vector2(0, -SpeedModifier));
    }
}