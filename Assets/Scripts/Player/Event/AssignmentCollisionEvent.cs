using UnityEngine;
using System.Collections;

public class AssignmentCollisionEvent : AbstractPlayerEvent
{
    public GameObject Assignment { get; private set; }

    public AssignmentCollisionEvent(GameObject assignment)
    {
        Assignment = assignment;
    }
}