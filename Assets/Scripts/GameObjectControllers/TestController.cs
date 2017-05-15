using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class TestController : AbstractAssignmentController
{
    /// <summary>
    /// The maximum speed at which the test will move horizontally.
    /// </summary>
    public float HorizontalSpeed;

    /// <summary>
    /// The force strength to apply to the test when chasing the player.
    /// </summary>
    public float TrackingStrength;

    public float DisableTrack;
    private bool _tracking;

    public float VerticalSpeed;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Math.Abs(VerticalSpeed));
        _tracking = true;
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position.y < DisableTrack)
        {
            _tracking = false;
        }

        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        if (_tracking)
        {
            // Apply a force proportional to the distance between the projected intercept x coordinate and the player's current x coordinate
            float projectedX = (PlayerController.Instance.gameObject.transform.position.y - gameObject.transform.position.y) *
                               body.velocity.x / body.velocity.y;
            float differenceX = projectedX - PlayerController.Instance.gameObject.transform.position.x;

            body.AddForce(CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position)
                .MultiplyVector(new Vector2(-TrackingStrength * differenceX / halfWidth, 0)));
        }

        Matrix4x4 transformation = CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position);

        float scaledWidth = halfWidth / (CombatStage.VanishingPoint.y - PlayerController.Instance.gameObject.transform.position.y) *
                            (CombatStage.VanishingPoint.y - gameObject.transform.position.y);

        if (gameObject.transform.position.x < -scaledWidth)
        {
            gameObject.transform.position = new Vector2(-scaledWidth, gameObject.transform.position.y);
            body.velocity = transformation.MultiplyVector(new Vector2(0, -VerticalSpeed));
        }
        else if (gameObject.transform.position.x > scaledWidth)
        {
            gameObject.transform.position = new Vector2(scaledWidth, gameObject.transform.position.y);
            body.velocity = transformation.MultiplyVector(new Vector2(0, -VerticalSpeed));
        }
        else
        {
            // Revert perspective transformation of velocity
            Vector2 velocity = transformation.inverse.MultiplyVector(gameObject.GetComponent<Rigidbody2D>().velocity);
            // Clamp horizontal speed
            body.velocity = transformation.MultiplyVector(new Vector2(Mathf.Clamp(velocity.x, -HorizontalSpeed, HorizontalSpeed), -VerticalSpeed));
        }
    }
}