using UnityEngine;
using System.Collections;

public class Spawned : SingletonMonoBehaviour<Spawned>
{
    private void Awake()
    {
        Instance = this;
    }
}