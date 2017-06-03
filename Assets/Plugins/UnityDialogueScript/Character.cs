using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character
{
	public string Name;

	public Image CurrentImage;

	public Dictionary<string, Image> AvailablePoseList;

	public Character(string Name, Dictionary<string, Image> PoseList, string NameOfDefaultImage = "default")
	{
		this.Name = Name;
		this.AvailablePoseList = PoseList;
		this.CurrentImage = AvailablePoseList[NameOfDefaultImage];
	}

	public void ChangePose(string TargetPose)
	{
		this.CurrentImage = AvailablePoseList[TargetPose];
	}
}
