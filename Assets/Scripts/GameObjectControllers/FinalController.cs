using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

// Currently not used as the design for finals is still work in progress
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
    public int AttackCount = 3;

    /// <summary>
    /// THe interval between the final launching stuff
    /// </summary>
    public float LaunchInterval = 5F;

    public GameObject Player;

    public GameObject FinalHWPrefab;

    public GameObject FinalTargetPrefab;

    /// <summary>
    /// This number of special homework objects would be fired each attack.
    /// </summary>
    public int HomeworkVolleySize = 3;

    /// <summary>
    /// How far down from the final object will the launched homework turn to move in the direction of their intended target.
    /// </summary>
    public float HomeworkRedirectOffset = 1F;

    /// <summary>
    /// How far from the bottom of the screen will the targets appear.
    /// </summary>
    public float TargetYPosOffset = 1F;

    public GameObject FinalBeamPrefab;

    private bool _inPosition = false;

    internal delegate void Callback();

    internal Callback OnEnd;

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

            Invoke("FinalAttack", LaunchInterval);

            _inPosition = true;
        }
    }

    void FinalAttack()
    {
        if (AttackCount > 0)
        {
            // TODO Figure out what finals do

            AttackCount--;
            Invoke("FinalAttack", LaunchInterval);
        }
        else
        {
            EndFinal();
        }
    }

    void HomeworkVolley()
    {
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        for (int i = 0; i < HomeworkVolleySize; i++)
        {
            GameObject finalHomework = Instantiate(FinalHWPrefab);

            Vector3 waypoint = new Vector3(Random.Range(-halfWidth, halfWidth), gameObject.transform.position.y - HomeworkRedirectOffset, 0);
            GameObject target = Instantiate(FinalTargetPrefab);
            target.transform.position = new Vector3(Random.Range(-halfWidth, halfWidth), -Camera.main.orthographicSize + TargetYPosOffset, 0);

            finalHomework.transform.position = gameObject.transform.position;
            finalHomework.GetComponent<FinalHWController>().Waypoint = waypoint;
            finalHomework.GetComponent<FinalHWController>().Target = target;
        }
    }

    void BeamAttack()
    {
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        GameObject finalBeam = Instantiate(FinalBeamPrefab);

        finalBeam.GetComponent<FinalBeamController>().Player = Player;
        finalBeam.transform.position = new Vector3(
            Random.Range(-halfWidth + finalBeam.transform.lossyScale.x / 2, halfWidth - finalBeam.transform.lossyScale.x / 2), 0, 0);
    }

    void EndFinal()
    {
        gameObject.SetActive(false);
        OnEnd.Invoke();
    }
}