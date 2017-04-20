using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using Object = System.Object;

public class SingletonMonoBehaviour<TInstanceClass> : MonoBehaviour
    where TInstanceClass : SingletonMonoBehaviour<TInstanceClass>
{
    private static TInstanceClass _instance = null;

    public static TInstanceClass Instance
    {
        get { return _instance; }
        set
        {
            if (_instance == null)
            {
                _instance = value;
                if (value.Persistent)
                {
                    DontDestroyOnLoad(value.gameObject);
                }
            }
            else if (_instance != value)
            {
                Destroy(value.gameObject);
            }
        }
    }

    public bool Persistent = true;

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}