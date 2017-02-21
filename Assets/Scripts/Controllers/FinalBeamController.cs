using UnityEngine;
using System.Collections;

public class FinalBeamController : MonoBehaviour
{
    /// <summary>
    /// The amount of time the background beam indicator will show up for.
    /// This is also how much time the player has to get into or out of the beam.
    /// </summary>
    public float ChargeTime = 1F;

    /// <summary>
    /// The amount of time the beam takes to expand to full width.
    /// </summary>
    public float FireTime = 0.2F;

    /// <summary>
    /// The amount of time the beam takes to contrack back into nothingness.
    /// </summary>
    public float FadeTime = 0.5F;

    public float MaxBGOpacity;

    public GameObject Player;

    private GameObject _beam;

    /// <summary>
    /// Not sure if there is a better system, but this will do for now.
    /// 0 = charging
    /// 1 = firing
    /// 2 = fading
    /// </summary>
    private int _phaseIndex;

    // Use this for initialization
    void Start()
    {
        _beam = GetComponentInChildren<Transform>().Find("Beam").gameObject;
        _phaseIndex = 0;

        // Invisible at first
        Color spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        spriteColor.a = 0;
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_phaseIndex)
        {
            case 0:
            {
                Color spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
                spriteColor.a += MaxBGOpacity * Time.deltaTime / ChargeTime;
                gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
                if (spriteColor.a >= MaxBGOpacity)
                {
                    _phaseIndex++;
                }
                return;
            }
            case 1:
            {
                Vector3 scale = _beam.transform.localScale;
                scale.x += Time.deltaTime / FireTime;
                if (scale.x >= 1)
                {
                    scale.x = 1;
                }
                _beam.transform.localScale = scale;

                if (scale.x >= 1)
                {
                    // Background color goes invisible
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    _phaseIndex++;
                    CheckPlayerInBounds();
                }
                return;
            }
            case 2:
            {
                Vector3 scale = _beam.transform.localScale;
                scale.x -= Time.deltaTime / FadeTime;
                _beam.transform.localScale = scale;

                if (scale.x <= 0)
                {
                    _phaseIndex++;
                }
                return;
            }
            default:
                Destroy(gameObject);
                return;
        }
    }

    void CheckPlayerInBounds()
    {
        if (!gameObject.GetComponent<SpriteRenderer>().bounds.Contains(Player.transform.position))
        {
            Invoke("DeductPlayerGrade", FadeTime);
        }
    }

    void DeductPlayerGrade()
    {
//        Player.GetComponent<PlayerGrade>().ChangeGrade(-1);
//        print(Player.GetComponent<PlayerGrade>().Grade);
    }
}