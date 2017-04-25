using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class GradeInfo
{
    /// <summary>
    /// Lists the grade names supported by the game.
    /// <see cref="FlagsAttribute"/> is not present in this enum,
    /// so technically you can compare the value like <code>PlayerGrade >= (int)GradeLevel.A</code>
    /// </summary>
    public enum GradeLevel
    {
        // TODO: Make sure the values are correct... I just randomly put some values...

        None,                               // None is basically equal to Fail.
        APlus = 98, A = 92, AMinus = 90,    // A-Grade Family... required to pass the Asian Mode
        BPlus = 88, B = 82, BMinus = 80,    // B-Grade Family... required to pass the Hard Mode
        CPlus = 78, C = 72, CMinus = 70,    // C-Grade Family... required to pass the Easy Mode
        DPlus = 68, D = 62, DMinus = 60,    // D-Grade Family... required to pass the Parents-Don't-Give-A-Crap Mode
        Fail = 0                            // YOU FAILED LOL
    }

    /// <summary>
    /// Calculate the grade level and returns <see cref="GradeLevel"/> value.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="zero"></param>
    /// <returns></returns>
    public static GradeLevel CalculateGradeLevel(int score, bool zero = false)
    {
        if (zero)               // If the score is starting from zero, add 100 to the score so that it can compare them correctly.
            score += 100;



        // Protective coding is very important
        if (score > 100 || score < 0)                           // If the score is out of range,
        {
            if (zero && score > 0)                              // Checks whether the score is starting from zero and throw an exception,
                throw new ArgumentOutOfRangeException("score", score, "The value of score is odd. It should be between 100 and 0.");

            return CalculateGradeLevel(score, zero=true);     // If parameter zero is false, check again with the score starting from zero.
        }

        // A-Grade Family
        else if (score >= (int)GradeLevel.APlus)
            return GradeLevel.APlus;
        else if (score >= (int)GradeLevel.A)
            return GradeLevel.A;
        else if (score >= (int)GradeLevel.AMinus)
            return GradeLevel.AMinus;

        // B-Grade Family
        else if (score >= (int)GradeLevel.BPlus)
            return GradeLevel.BPlus;
        else if (score >= (int)GradeLevel.B)
            return GradeLevel.B;
        else if (score >= (int)GradeLevel.BMinus)
            return GradeLevel.BMinus;

        // C-Grade Family
        else if (score >= (int)GradeLevel.CPlus)
            return GradeLevel.CPlus;
        else if (score >= (int)GradeLevel.C)
            return GradeLevel.C;
        else if (score >= (int)GradeLevel.CMinus)
            return GradeLevel.CMinus;

        // D-Grade Family
        else if (score >= (int)GradeLevel.DPlus)
            return GradeLevel.DPlus;
        else if (score >= (int)GradeLevel.D)
            return GradeLevel.D;
        else if (score >= (int)GradeLevel.DMinus)
            return GradeLevel.DMinus;

        // Failure
        else
            return GradeLevel.Fail; ;
    }
}
