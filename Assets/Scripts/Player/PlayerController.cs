using UnityEngine;
using UnityEngine.WSA;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public float Speed;
    private Rigidbody2D _thisBody;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        _thisBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _thisBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, 0);

        // Bound player to between two walls
        float widthBound = Camera.main.orthographicSize * Screen.width / Screen.height - gameObject.GetComponent<Renderer>().bounds.size.x / 2;
        gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, -widthBound, widthBound), gameObject.transform.position.y,
            gameObject.transform.position.z);
    }
}