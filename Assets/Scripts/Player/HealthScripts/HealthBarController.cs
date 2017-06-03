using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Color FullHeartColor;
    public Color DepletedHeartColor;

    public Image[] HeartImages;

    [NonSerialized]
    public int HeartLeft;

    public delegate void WhenDied();

    // Use this for initialization
    void Start()
    {
        HeartLeft = HeartImages.Length;
        foreach (Image HeartSprite in HeartImages)
            HeartSprite.color = FullHeartColor;     // Initialize the heart counter
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Use this function when manually setting hearts is necessary (e.g. loading a savefile)
    public void SetHealth(int hearts)
    {
        for (int i = 1; i < HeartImages.Length && i <= hearts; i++)
            HeartImages[HeartImages.Length - i].color = DepletedHeartColor;    // Fill heart color from the right edge.
    }

    public void LoseOneHeart()
    {
        HeartImages[HeartLeft - 1].color = DepletedHeartColor; 
        HeartLeft--;
    }
}