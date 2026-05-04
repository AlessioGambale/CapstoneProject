using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

 class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextAsset _inkJason;
    [SerializeField] private GameObject _visualCue;
    [Header("Events")]
    [SerializeField] private UnityEvent _onDialogueEnd;
    [SerializeField] private UnityEvent _onSecondDialogueEnd;
    [SerializeField] private UnityEvent _onFinalDialogueEnd;
    [Header("Second Dialogue")]
    [SerializeField] private TextAsset _inkJasonSecond;
    [SerializeField] private string _secondKnot;
    [Header("Final Dialogue")]
    [SerializeField] private TextAsset _inkJasonFinal;
    [SerializeField] private bool _isOrin;

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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_isOrin && RunManager.Instance != null)
                {
                    if (!RunManager.Instance.OrinIntroPlayed)
                    {
                        RunManager.Instance.SetOrinIntroPlayed();
                        _playerStateHandler?.EnterDialogue();
                        DialogueManager.Instance.EnterDialogueMode(_inkJason, () =>
                        {
                            _playerStateHandler?.ExitDialogue();
                            _onDialogueEnd?.Invoke();
                        });
                    }
                    else if (_inkJasonFinal != null && RunManager.Instance.FightsWon >= 4 && RunManager.Instance.LastFightWon)
                    {
                        RunManager.Instance.ClearFightWon();
                        _playerStateHandler?.EnterDialogue();
                        DialogueManager.Instance.EnterDialogueMode(_inkJasonFinal, () =>
                        {
                            _playerStateHandler?.ExitDialogue();
                            RunManager.Instance.UnlockBoss();
                            _onFinalDialogueEnd?.Invoke();
                        });
                    }
                    else if (_inkJasonSecond != null && RunManager.Instance.LastFightWon)
                    {
                        RunManager.Instance.ClearFightWon();
                        _playerStateHandler?.EnterDialogue();
                        string knot = RunManager.Instance.CurrentPathKnot;
                        DialogueManager.Instance.EnterDialogueMode(_inkJasonSecond, knot, () =>
                        {
                            _playerStateHandler?.ExitDialogue();
                            _onSecondDialogueEnd?.Invoke();
                        });
                    }
                }
                else
                {
                    _playerStateHandler?.EnterDialogue();
                    DialogueManager.Instance.EnterDialogueMode(_inkJason, () =>
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
