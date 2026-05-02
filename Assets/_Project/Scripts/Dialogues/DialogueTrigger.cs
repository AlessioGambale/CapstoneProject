using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextAsset _inkJason;
    [SerializeField] private GameObject _visualCue;

    [Header("Events")]
    [SerializeField] private UnityEvent _onDialogueEnd;

    [Header("Second Dialogue")]
    [SerializeField] private TextAsset _inkJasonSecond;
    [SerializeField] private string _secondKnot;

    private bool _firstDone;
    private bool _playerInRange;
    private PlayerStateHandler _playerStateHandler;

    private void Start()
    {
        _playerInRange = false;
        _visualCue.SetActive(false);
    }

    private void Update()
    {
        if (_playerInRange && !DialogueManager.Instance.IsDialoguePlaying())
        {
            _visualCue.SetActive(true);
            if (Input.GetButtonDown("Submit"))
            {
                if (!_firstDone)
                {
                    _firstDone = true;
                    _playerStateHandler?.EnterDialogue();
                    DialogueManager.Instance.EnterDialogueMode(_inkJason, () =>
                    {
                        _playerStateHandler?.ExitDialogue();
                        _onDialogueEnd?.Invoke();
                    });
                }
                else if (_inkJasonSecond != null && RunManager.Instance.LastFightWon)
                {
                    RunManager.Instance.ClearFightWon();
                    _playerStateHandler?.EnterDialogue();
                    DialogueManager.Instance.EnterDialogueMode(_inkJasonSecond, _secondKnot, () =>
                    {
                        _playerStateHandler?.ExitDialogue();
                        _onDialogueEnd?.Invoke();
                    });
                }
            }
        }
        else
        {
            _visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInRange = true;
        _playerStateHandler = other.GetComponent<PlayerStateHandler>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInRange = false;
    }
}
