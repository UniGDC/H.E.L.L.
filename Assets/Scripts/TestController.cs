using System;
using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour
{
    public GameObject Player;

    /// <summary>
    /// The maximum speed at which the test will move horizontally.
    /// </summary>
    public float HorizontalSpeedCap = 0.5F;

    /// <summary>
    /// The force strength to apply to the test when chasing the player.
    /// </summary>
    public float HorizontalForce = 0.5F;

    public float VerticalSpeed = 1;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -VerticalSpeed);
    }

    void FixedUpdate()
    {
        float differenceX = Player.transform.position.x - gameObject.transform.position.x;

        if (differenceX > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(HorizontalForce, 0));
        }
        else if (differenceX < 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-HorizontalForce, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Clamp horizontal speed
        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        gameObject.GetComponent<Rigidbody2D>().velocity =
            new Vector2(Mathf.Clamp(velocity.x, -HorizontalSpeedCap, HorizontalSpeedCap), velocity.y);
    }
}