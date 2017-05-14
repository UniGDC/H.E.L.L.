using UnityEngine;
using System.Collections;

public class ParallelStageManager : AbstractStage
{
    public AbstractStage[] ParallelStages;
    private Coroutine[] _stageCoroutines;

    public override IEnumerator Run()
    {
        _stageCoroutines = new Coroutine[ParallelStages.Length];

        for (int i = 0; i < ParallelStages.Length; i++)
        {
            _stageCoroutines[i] = StartCoroutine(ParallelStages[i].Run());
        }

        foreach (Coroutine stageCoroutine in _stageCoroutines)
        {
            yield return stageCoroutine;
        }
    }

    public override void Kill()
    {
        foreach (AbstractStage stage in ParallelStages)
        {
            stage.Kill();
        }
    }
}