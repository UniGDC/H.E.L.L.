using System;
using UnityEngine;
using System.Collections;


public class StoryController : MonoBehaviour
{
    private int _currentIndex;
    public AbstractStoryElement[] StoryElements;

    private void Awake()
    {
        if (StoryElements == null || StoryElements.Length == 0)
        {
            StoryElements = gameObject.GetComponentsInChildren<AbstractStoryElement>();
        }
        _currentIndex = 0;
    }

    public void EndCurrentStoryElement()
    {
        StoryElements[_currentIndex].Deactivate();
        _currentIndex++;

        if (_currentIndex >= StoryElements.Length)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartNextStoryElement()
    {
        if (_currentIndex < StoryElements.Length)
        {
            StoryElements[_currentIndex].Activate();
        }
        else
        {
            // No point keeping this canvas anymore
            gameObject.SetActive(false);
        }
    }

    public void Continue()
    {
        EndCurrentStoryElement();
        StartNextStoryElement();
    }


    public void OnClick()
    {
        StoryElements[_currentIndex].OnClick();
    }
}