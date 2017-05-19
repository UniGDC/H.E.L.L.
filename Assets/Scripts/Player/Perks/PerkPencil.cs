public class PerkPencil : AbstractPerk
{
    public PerkPencil()
    {
        Cost = 5;
    }

    public void OnAssigmentCollision(AssignmentCollisionEvent e)
    {
        // If there are charges remaining and the assignment is harmful
        if (Charges > 0 && e.Assignment.GetComponent<AbstractAssignmentController>().Damage > 0)
        {
            Charges--;
            Destroy(e.Assignment);
            e.Cancelled = true;
        }
    }
}