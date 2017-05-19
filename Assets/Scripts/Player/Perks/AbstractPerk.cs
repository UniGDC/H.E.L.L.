using UnityEngine;
using System.Collections;

public class AbstractPerk : MonoBehaviour
{
    public int Charges;
    public int Cost { get; protected set; }
}