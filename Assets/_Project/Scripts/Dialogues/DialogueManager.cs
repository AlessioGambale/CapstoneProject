using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueManager : GenericSingleton<DialogueManager>
{
    private enum DialogueState { HIDDEN, ENTERING, PLAYING, SELECTING_CHOICES, APPLYING_CHOICES, EXITING }

    [Header("References")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _speakerText;
    [SerializeField] private DialogueChoice _choice;

    [Header("Dialogue State")]
    [SerializeField] private DialogueState _dialogueState;

    private Story _currentStory;
    private Action _onDialogueCompleteAction;
    private UnityEvent _onDialogueCompleteEvent;

    private static DialogueVariables _variables = new DialogueVariables();
    private List<DialogueChoice> _choiceList;

    private void Start()
    {
        _dialoguePanel.SetActive(false);
        _dialogueState = DialogueState.HIDDEN;
        _choiceList = new List<DialogueChoice>();
        _choiceList.Add(_choice);
    }

    public static bool GetBool(string variableName) => _variables.GetBool(variableName);
    public static void SetBool(string variableName, bool value) => _variables.SetBool(variableName, value);
    public static int GetInt(string variableName) => _variables.GetInt(variableName);
    public static void SetInt(string variableName, int value) => _variables.SetInt(variableName, value);

    public void EnterDialogueMode(TextAsset inkJason, string targetKnot) => EnterDialogueMode(inkJason, targetKnot, null, null);
    public void EnterDialogueMode(TextAsset inkJason, UnityEvent onDialogueCompleteEvent = null) => EnterDialogueMode(inkJason, null, onDialogueCompleteEvent, null);
    public void EnterDialogueMode(TextAsset inkJason, Action onDialogueCompleteAction = null) => EnterDialogueMode(inkJason, null, null, onDialogueCompleteAction);
    public void EnterDialogueMode(TextAsset inkJason, string targetKnot, UnityEvent onDialogueCompleteEvent = null) => EnterDialogueMode(inkJason, targetKnot, onDialogueCompleteEvent, null);
    public void EnterDialogueMode(TextAsset inkJason, string targetKnot, Action onDialogueCompleteAction = null) => EnterDialogueMode(inkJason, targetKnot, null, onDialogueCompleteAction);

    public void EnterDialogueMode(TextAsset inkJason, string targetKnot, UnityEvent onDialogueCompleteEvent, Action onDialogueCompleteAction)
    {
        _onDialogueCompleteAction = onDialogueCompleteAction;
        _onDialogueCompleteEvent = onDialogueCompleteEvent;

        _currentStory = new Story(inkJason.text);

        if (!string.IsNullOrEmpty(targetKnot))
            _currentStory.ChoosePathString(targetKnot);

        _dialoguePanel.SetActive(true);
        _dialogueState = DialogueState.ENTERING;

        _variables.AddNewGlobalVariablesFromStory(_currentStory);
        _variables.StartListening(_currentStory);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            string text = _currentStory.Continue();
            ParseTags();
            DisplayOrHideChoices();

            if (HasChoices())
            {
                _dialogueState = DialogueState.SELECTING_CHOICES;
                if (!string.IsNullOrEmpty(text))
                    _dialogueText.SetText(text);
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    _dialogueText.SetText(text);
                    if (_dialogueState != DialogueState.ENTERING && _dialogueState != DialogueState.APPLYING_CHOICES)
                        _dialogueState = DialogueState.PLAYING;
                }
                else
                {
                    ContinueStory();
                }
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        _dialogueState = DialogueState.EXITING;
        yield return new WaitForSeconds(0.1f);
        _variables.StopListening(_currentStory);
        _dialogueState = DialogueState.HIDDEN;
        _dialoguePanel.SetActive(false);
        _dialogueText.SetText(string.Empty);
        if (_speakerText != null)
            _speakerText.SetText(string.Empty);

        if (_onDialogueCompleteAction != null)
            _onDialogueCompleteAction();
        else if (_onDialogueCompleteEvent != null)
            _onDialogueCompleteEvent?.Invoke();
    }

    private bool HasChoices() => _currentStory.currentChoices != null && _currentStory.currentChoices.Count > 0;

    private void DisplayOrHideChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        int i = 0;

        for (; i < currentChoices.Count && i < _choiceList.Count; i++)
        {
            _choiceList[i].gameObject.SetActive(true);
            _choiceList[i].ChoiceText.SetText(currentChoices[i].text);
        }
        for (; i < currentChoices.Count; i++)
        {
            DialogueChoice choiceGameObject = Instantiate(_choice, _choice.transform.parent);
            choiceGameObject.ChoiceIndex = i;
            _choiceList.Add(choiceGameObject);
            choiceGameObject.gameObject.SetActive(true);
            choiceGameObject.ChoiceText.SetText(currentChoices[i].text);
        }
        for (; i < _choiceList.Count; i++)
            _choiceList[i].gameObject.SetActive(false);

        if (currentChoices.Count > 0)
            EventSystem.current.SetSelectedGameObject(_choiceList[0].gameObject);
    }

    private void ParseTags()
    {
        List<string> tags = _currentStory.currentTags;
        foreach (string tag in tags)
        {
            if (tag.Contains(":"))
            {
                string[] splitTag = tag.Split(':');
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();

                switch (tagKey)
                {
                    case "speaker":
                        if (_speakerText != null)
                            _speakerText.SetText(tagValue);
                        break;
                    default:
                        Debug.Log($"Tag: {tagKey} - Value: {tagValue}");
                        break;
                }
            }
            else
            {
                Debug.Log($"Tag: {tag}");
            }
        }
    }

    public void MakeChoice(int index)
    {
        _currentStory.ChooseChoiceIndex(index);
        _dialogueState = DialogueState.APPLYING_CHOICES;
        ContinueStory();
    }

    public bool IsDialoguePlaying()
    {
        switch (_dialogueState)
        {
            case DialogueState.ENTERING:
            case DialogueState.PLAYING:
            case DialogueState.SELECTING_CHOICES:
            case DialogueState.APPLYING_CHOICES:
            case DialogueState.EXITING:
                return true;
            default:
            case DialogueState.HIDDEN:
                return false;
        }
    }

    private void Update()
    {
        switch (_dialogueState)
        {
            case DialogueState.ENTERING:
            case DialogueState.APPLYING_CHOICES:
                _dialogueState = DialogueState.PLAYING;
                return;
            case DialogueState.PLAYING:
                if (Input.GetButtonDown("Submit"))
                    ContinueStory();
                break;
            case DialogueState.SELECTING_CHOICES:
                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(_choiceList[0].gameObject);
                break;
            case DialogueState.EXITING:
            case DialogueState.HIDDEN:
                return;
        }
    }
}
