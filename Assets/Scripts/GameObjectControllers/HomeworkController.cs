using System;
using UnityEngine;
using System.Collections;

public class HomeworkController : MonoBehaviour
{
    public Vector3 Target;

    public float SpeedModifier;

    private void FixedUpdate()
    {
        Vector2 currentDirection = new Vector2(gameObject.transform.position.x,
            gameObject.transform.position.y - CombatStage.VanishingPointYCoordinate);
        float newMagnitude = (CombatStage.VanishingPointYCoordinate - gameObject.transform.position.y) /
                             (CombatStage.VanishingPointYCoordinate - Target.y);
        newMagnitude *= SpeedModifier;

        currentDirection.Scale(new Vector2(newMagnitude / Mathf.Abs(currentDirection.y), newMagnitude / Mathf.Abs(currentDirection.y)));

        gameObject.GetComponent<Rigidbody2D>().velocity = currentDirection;
    }
}