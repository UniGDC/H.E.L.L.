using System;
using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour
{
    /// <summary>
    /// The maximum speed at which the test will move horizontally.
    /// </summary>
    public float HorizontalSpeed = 0.5F;

    /// <summary>
    /// The force strength to apply to the test when chasing the player.
    /// </summary>
    public float TrackingStrength = 0.5F;

    public float VerticalSpeed = 1;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Math.Abs(VerticalSpeed));
    }

    void FixedUpdate()
    {
        float projectedX = PlayerController.Player.transform.position.x /
                           (PlayerController.Player.transform.position.y - CombatStage.VanishingPoint.y) *
                           (gameObject.transform.position.y - CombatStage.VanishingPoint.y);
        float differenceX = projectedX - gameObject.transform.position.x;

        if (differenceX > 0)
        {
            gameObject.GetComponent<Rigidbody2D>()
                .AddForce(CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position)
                    .MultiplyVector(new Vector2(TrackingStrength, 0)));
        }
        else if (differenceX < 0)
        {
            gameObject.GetComponent<Rigidbody2D>()
                .AddForce(CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position)
                    .MultiplyVector(new Vector2(-TrackingStrength, 0)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 transformation = CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position);

        // Revert perspective transformation of velocity
        Vector2 velocity = transformation.inverse.MultiplyVector(gameObject.GetComponent<Rigidbody2D>().velocity);
        // Clamp horizontal speed
        gameObject.GetComponent<Rigidbody2D>().velocity =
            transformation.MultiplyVector(new Vector2(Mathf.Clamp(velocity.x, -HorizontalSpeed, HorizontalSpeed), -VerticalSpeed));
    }
}