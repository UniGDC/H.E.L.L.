using System;
using UnityEngine;
using System.Collections;

public class FinalController : MonoBehaviour
{
    /// <summary>
    /// The speed at which the final will move down toward its final position
    /// </summary>
    public float CoastSpeed = 0.2F;

    /// <summary>
    /// The Y position, from the top of the screen, where the final will stop moving and start launching things
    /// </summary>
    public float EndPositionOffset = 0.5F;

    /// <summary>
    /// The number of times the final spits out questions
    /// </summary>
    public int LaunchCount = 3;

    /// <summary>
    /// THe interval between the final launching stuff
    /// </summary>
    public float LaunchInterval = 5F;

    private bool _inPosition = false;

    // Use this for initialization
    void Start()
    {
        // Slowly move down toward the final position.
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -Math.Abs(CoastSpeed), 0);
    }

    void FixedUpdate()
    {
        // Check if the final is in position
        if (!_inPosition && gameObject.transform.position.y <=
            Camera.main.orthographicSize - Math.Abs(EndPositionOffset))
        {
            // Stop moving and start launching final questions
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            Invoke("LaunchItems", LaunchInterval);

            _inPosition = true;
        }
    }

    void LaunchItems()
    {
        if (LaunchCount > 0)
        {
            // TODO Figure out what finals do
//            print("Launching final tests and homeworks");

            LaunchCount--;
            Invoke("LaunchItems", LaunchInterval);
        }
        else
        {
            EndFinal();
        }
    }

    void EndFinal()
    {
    }
}