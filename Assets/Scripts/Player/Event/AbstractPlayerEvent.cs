using UnityEngine;
using System.Collections;

public abstract class AbstractPlayerEvent
{
    private bool _cancelled;

    public bool Cancelled
    {
        get { return _cancelled; }
        set
        {
            // Cannot un-cancel
            _cancelled = _cancelled || value;
        }
    }
}
