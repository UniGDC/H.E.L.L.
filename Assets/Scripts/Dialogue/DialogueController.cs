using System;
using UnityEngine;
using System.Collections;


public class DialogueController : MonoBehaviour
{
    private int _currentIndex;
    public AbstractStoryElement[] StoryElements;

    private void Awake()
    {
        if (StoryElements == null || StoryElements.Length == 0)
        {
            StoryElements = gameObject.GetComponentsInChildren<AbstractStoryElement>();
        }
    }

    private void Start()
    {
        foreach (AbstractStoryElement storyElement in StoryElements)
        {
            storyElement.OnFinished = NextStoryElement;
        }

        _currentIndex = 0;
        StoryElements[_currentIndex].Activate();
    }

    void NextStoryElement()
    {
        StoryElements[_currentIndex].Deactivate();
        _currentIndex++;
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

    public void OnClick()
    {
        StoryElements[_currentIndex].OnClick();
    }
}