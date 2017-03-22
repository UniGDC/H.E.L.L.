using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class AbstractGameplayStage : MonoBehaviour
{
    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void Begin()
    {
        gameObject.SetActive(true);
    }

    public virtual void End()
    {
        gameObject.SetActive(false);
        LevelController.Controller.Continue();
    }
}
