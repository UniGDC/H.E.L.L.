using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGrade : MonoBehaviour
{
    public Text GradeDisplay;
    /// <summary>
    /// Grade starts at 0 and goes down into the negatives as the player takes damage.
    /// </summary>
    private int _grade;


    public Color[] GradeColors;

    // Use this for initialization
    void Start()
    {
        _grade = 0;

        // Initialize GradeDisplay if it has not been initialized.
        if (GradeDisplay == null)
        {
            GradeDisplay = FindObjectOfType<Text>();
        }

        UpdateGradeDisplay();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Homework":
            case "Test":
                Destroy(coll.gameObject);
                _grade--;
                break;
            default:
                break;
        }

        UpdateGradeDisplay();
    }

    private void UpdateGradeDisplay()
    {
        var CurrentGrade = GradeInfo.CalculateGradeLevel(_grade, true);
        if (CurrentGrade != GradeInfo.GradeLevel.None)
        {
            GradeDisplay.text = CurrentGrade.ToString();
            GradeDisplay.color = Color.cyan;
        }
        else
        {
            GradeDisplay.text = "ERROR";
            GradeDisplay.color = Color.red;
        }
    }
}