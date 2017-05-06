using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class AbstractStage : MonoBehaviour
{
    public bool Running { get; private set; }

    public IStageController ParentController;

    protected virtual void Awake()
    {
        Running = false;
        gameObject.SetActive(false);
    }

    public virtual void Begin()
    {
        gameObject.SetActive(true);
        Running = true;
    }

    public virtual void End()
    {
        Running = false;
        gameObject.SetActive(false);
        ParentController.Continue();
    }
}