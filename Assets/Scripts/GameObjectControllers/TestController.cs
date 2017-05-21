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

        if (_tracking)
        {
            float projectedX = PlayerController.Instance.gameObject.transform.position.x /
                               (PlayerController.Instance.gameObject.transform.position.y - CombatStage.VanishingPoint.y) *
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

        Matrix4x4 transformation = CombatStage.GetPerspectiveTransformationMatrix(gameObject.transform.position);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float scaledWidth = halfWidth / (CombatStage.VanishingPoint.y - PlayerController.Instance.gameObject.transform.position.y) *
                            (CombatStage.VanishingPoint.y - gameObject.transform.position.y);

        if (gameObject.transform.position.x < -scaledWidth)
        {
            gameObject.transform.position = new Vector2(-scaledWidth, gameObject.transform.position.y);
            gameObject.GetComponent<Rigidbody2D>().velocity =
                transformation.MultiplyVector(new Vector2(0, -VerticalSpeed));
        }
        else if (gameObject.transform.position.x > scaledWidth)
        {
            gameObject.transform.position = new Vector2(scaledWidth, gameObject.transform.position.y);
            gameObject.GetComponent<Rigidbody2D>().velocity =
                transformation.MultiplyVector(new Vector2(0, -VerticalSpeed));
        }
        else
        {
            // Revert perspective transformation of velocity
            Vector2 velocity = transformation.inverse.MultiplyVector(gameObject.GetComponent<Rigidbody2D>().velocity);
            // Clamp horizontal speed
            gameObject.GetComponent<Rigidbody2D>().velocity =
                transformation.MultiplyVector(new Vector2(Mathf.Clamp(velocity.x, -HorizontalSpeed, HorizontalSpeed), -VerticalSpeed));
        }
    }
}