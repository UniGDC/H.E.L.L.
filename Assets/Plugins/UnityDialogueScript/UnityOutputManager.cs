using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RangHo.DialogueScript;

public class UnityOutputManager : MonoBehaviour, IOutputManager
{
	public Dictionary<string, object> RegisteredObjects;

	[NonSerialized]
	public string SelectedObject;

	public delegate string GetUserInput();

	public UnityOutputManager()
	{

	}

	public void Say(object Speaker, string Content)
	{
		
	}

	public void Set(object Value)
	{
		if (SelectedObject.IndexOf('.') >= 0)
		{
			string[] Separated = SelectedObject.Split('.');
            PropertyInfo ContainerProperty = RegisteredObjects[Separated[0]].GetType().GetProperty(Separated[1], BindingFlags.Public | BindingFlags.Instance);
			if (ContainerProperty != null && ContainerProperty.CanWrite)
				ContainerProperty.SetValue(RegisteredObjects[Separated[1]], Value, null);
			return;
		}
		RegisteredObjects[SelectedObject] = Value;
	}

	public void Select(string Target, string Container = null)
	{
		if (Container == null)
			SelectedObject = Target;
		else
			SelectedObject = string.Concat(Container, ".", Target);
	}

	public void RegisterObject(object Target, string Key)
	{
		if (RegisteredObjects.ContainsKey(Key))
			return;
		RegisteredObjects.Add(Key, Target);
	}

	public object FindObject(string Key)
	{
		object output;
		if (RegisteredObjects.TryGetValue(Key, out output))
			return output;
		return null;
	}

	public void Choices(Dictionary<string, string> ChoicesDictionary, ref string ChosenLabel)
	{

	}

	public void Exception(Exception e)
	{

	}

	public void Finish()
	{

	}
}
