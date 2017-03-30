using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static GameObject Player;

    public float Speed;
    private Rigidbody2D _thisBody;

    private void Awake()
    {
        if (Player == null)
        {
            Player = gameObject;
        }
        else if (Player != gameObject)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    private void Start()
    {
        _thisBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _thisBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, 0);
    }

    private void OnDestroy()
    {
        if (Player == gameObject)
        {
            Player = null;
        }
    }
}