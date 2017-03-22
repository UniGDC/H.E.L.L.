using UnityEngine;
using System.Collections;

public class FinalHWController : MonoBehaviour
{
    public Vector3 Waypoint;
    public GameObject Target;

    /// <summary>
    /// The speed at which this object coasts toward the waypoint.
    /// </summary>
    public float CoastSpeedTuning = 1F;

    /// <summary>
    /// The amount of time this object pauses at waypoint.
    /// </summary>
    public float WaypointDelay = 1F;

    /// <summary>
    /// The speed at which this object will move toward target after reaching the waypoint.
    /// </summary>
    public float LaunchSpeedTuning = 0.8F;

    private bool _reachedWaypoint;

    // Use this for initialization
    void Start()
    {
        _reachedWaypoint = false;

        Vector2 direction = Waypoint - gameObject.transform.position;
        direction.Scale(new Vector2(CoastSpeedTuning, CoastSpeedTuning));
        gameObject.GetComponent<Rigidbody2D>().velocity = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_reachedWaypoint && gameObject.transform.position.y <= Waypoint.y)
        {
            _reachedWaypoint = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Invoke("MoveToTarget", WaypointDelay);
        }
    }

    void MoveToTarget()
    {
        Vector2 direction = Target.transform.position - gameObject.transform.position;
        direction.Scale(new Vector2(LaunchSpeedTuning, LaunchSpeedTuning));
        gameObject.GetComponent<Rigidbody2D>().velocity = direction;

        Destroy(Target);
    }
}