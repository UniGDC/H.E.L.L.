using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class AbstractStage : MonoBehaviour
{
    public bool Running { get; private set; }

    protected virtual void Awake()
    {
        Running = false;
        gameObject.SetActive(false);
    }

    public abstract IEnumerator Run();

    public abstract void Kill();
}