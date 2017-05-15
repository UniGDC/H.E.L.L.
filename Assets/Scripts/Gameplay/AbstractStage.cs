using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class AbstractStage : MonoBehaviour
{
    public bool Running { get; protected set; }

    protected virtual void _init()
    {
        _reinit();
    }

    protected abstract void _reinit();

    private void Awake()
    {
        Running = false;
        _init();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// The <code>LevelController</code> will start a coroutine with this method to run this stage.
    /// </summary>
    public IEnumerator Run()
    {
        Running = true;
        return _run();
    }

    protected abstract IEnumerator _run();

    /// <summary>
    /// Stops this stage if it is running, and re-initializes this stage.
    /// </summary>
    public void Kill()
    {
        Running = false;
        _reinit();
        gameObject.SetActive(false);
    }
}