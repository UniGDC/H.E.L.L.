using UnityEngine;
using System.Collections;

public class PerkStudySkills : AbstractPerk
{
    public void OnAssignmentCollision(AssignmentCollisionEvent e)
    {
        if (e.Cancelled) return;

        if (Charges > 0 && e.Assignment.GetComponent<AbstractAssignmentController>().Damage > 0)
        {
            Charges--;
            Destroy(e.Assignment);
            e.Cancelled = true;
        }
    }
}