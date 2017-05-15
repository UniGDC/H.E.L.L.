using UnityEngine;
using System.Collections;

// Currently not used.
public class ParallelStageManager : AbstractStage
{
    public AbstractStage[] ParallelStages;
    private Coroutine[] _stageCoroutines;

    protected override void _reinit()
    {
        foreach (AbstractStage stage in ParallelStages)
        {
            stage.Kill();
        }
    }

    protected override IEnumerator _run()
    {
        _stageCoroutines = new Coroutine[ParallelStages.Length];

        // Start all substages
        for (int i = 0; i < ParallelStages.Length; i++)
        {
            _stageCoroutines[i] = StartCoroutine(ParallelStages[i].Run());
        }

        // Wait for each substage to finish
        foreach (Coroutine stageCoroutine in _stageCoroutines)
        {
            yield return stageCoroutine;
        }
    }
}