using UnityEngine;
using System.Collections;

public class HomeworkController : MonoBehaviour
{
    public GameObject Player;

    /// <summary>
    /// Indicates whether the homework has launched.
    /// </summary>
    private bool _launched = false;

    /// <summary>
    /// THe speed at which homework will coast toward the launch position.
    /// </summary>
    public float CoastSpeed = 0.5F;

    /// <summary>
    /// The amount of time the homework stays still before launching towards player.
    /// </summary>
    public float LaunchDelay = 1;

    /// <summary>
    /// The speed at which the homework will move towards the player.
    /// </summary>
    public float LaunchSpeed = 1;

    /// <summary>
    /// The y coordinate from the bottom of the screen where the homework will start moving towards the player.
    /// </summary>
    public float LaunchPositionOffset = 0.5F;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, CoastSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y >= -Camera.main.orthographicSize + LaunchPositionOffset && !_launched)
        {
            // Object reached launch position, need to prepare to launch toward player.
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            _launched = true;

            Invoke("LaunchToPlayer", LaunchDelay);
        }
    }

    void LaunchToPlayer()
    {
        Vector2 positionDifference = Player.transform.position - gameObject.transform.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = positionDifference.normalized * LaunchSpeed;
    }
}