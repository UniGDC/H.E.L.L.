using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Controller;

    public float Speed;
    private Rigidbody2D _thisBody;

    private void Awake()
    {
        if (Controller == null)
        {
            Controller = this;
        }
        else if (Controller != this)
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
        _thisBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, 0);
    }

    private void OnDestroy()
    {
        if (Controller == this)
        {
            Controller = null;
        }
    }
}