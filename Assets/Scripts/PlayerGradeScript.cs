using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGradeScript : MonoBehaviour
{
    public Text GradeDisplay;
    /// <summary>
    /// Grade starts at 0 and goes down into the negatives as the player takes damage.
    /// </summary>
    private int _grade;

    public String[] Grades;
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("Collision");
        switch (coll.gameObject.tag)
        {
            case "Homework":
            case "Test":
                print("Collision");
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
        if (-_grade >= 0 && -_grade < Grades.Length && -_grade < GradeColors.Length)
        {
        }
        else
        {
            GradeDisplay.text = "ERROR";
            GradeDisplay.color = Color.red;
        }
    }
}