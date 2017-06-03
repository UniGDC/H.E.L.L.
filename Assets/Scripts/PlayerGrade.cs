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
    public int Grade { get; private set; }


    public Color[] GradeColors;

    // Use this for initialization
    void Start()
    {
        Grade = 0;

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
                Grade--;
                break;
            default:
                break;
        }

        UpdateGradeDisplay();
    }

    public void ChangeGrade(int change)
    {
        Grade += change;

        UpdateGradeDisplay();
    }

    private void UpdateGradeDisplay()
    {
        var CurrentGrade = GradeInfo.CalculateGradeLevel(Grade, true);
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