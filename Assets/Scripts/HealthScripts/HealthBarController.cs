using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Color FullHeartColor;
    public Color DepletedHeartColor;

    public Image[] HeartImages;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHealth(int hearts)
    {
        int i=0;
        for (; i < hearts && i < HeartImages.Length; i++)
        {
            HeartImages[i].color = FullHeartColor;
        }
        for (; i < HeartImages.Length; i++)
        {
            HeartImages[i].color = DepletedHeartColor;
        }
    }
}