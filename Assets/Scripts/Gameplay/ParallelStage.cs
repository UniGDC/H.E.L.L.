using UnityEngine;
using System.Collections;

public class ParallelStageManager : AbstractStage, IStageController
{
    public AbstractStage[] ParallelStages;

    public override void Begin()
    {
        base.Begin();

        foreach (AbstractStage stage in ParallelStages)
        {
            stage.ParentController = this;
            stage.Begin();
        }
    }

    public void Continue()
    {
        foreach (AbstractStage stage in ParallelStages)
        {
            if (stage.Running)
            {
                return;
            }
        }

        End();
    }
}