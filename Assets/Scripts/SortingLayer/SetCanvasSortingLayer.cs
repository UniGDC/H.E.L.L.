using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasSortingLayer : MonoBehaviour
{
	public string SortingLayerName;

	public int SortingOrder;

	// Use this for initialization
	void Start()
	{
		gameObject.GetComponent<Canvas>().sortingLayerName = this.SortingLayerName;
		gameObject.GetComponent<Canvas>().sortingOrder = this.SortingOrder;
	}
}
