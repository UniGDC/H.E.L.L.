using UnityEngine;
using System.Collections;

public class AbstractAssignmentController : MonoBehaviour
{
    public CombatStage Parent;
    private float _destroyPoint;

    public int Damage = 1;

    private void Awake()
    {
        _destroyPoint = -Camera.main.orthographicSize - gameObject.GetComponent<Renderer>().bounds.size.y / 2;
    }

    private void Update()
    {
        if (gameObject.transform.position.y < _destroyPoint)
        {
            Destroy(gameObject);
        }
    }
}