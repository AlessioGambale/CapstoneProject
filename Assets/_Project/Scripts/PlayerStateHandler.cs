using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCreature _playerCreature;
    [SerializeField] private InputHandler _input;
    [SerializeField] private GameObject _loseScreen;

    private AnimationParamHandler _paramHandler;
    public bool IsInCombat { get; private set; }
    public event Action OnCombatEnter;
    public event Action OnCombatExit;

    private void Awake()
    {
        if (_playerCreature == null) _playerCreature = GetComponent<PlayerCreature>();
        if (_input == null) _input = GetComponent<InputHandler>();
        _paramHandler = GetComponent<AnimationParamHandler>();
    }

    private void HandleVictory()
    {
        ExitCombat();
        GetComponentInChildren<PlayerToExplorationTransition>()?.Trigger();
    }

    private void HandleDefeat()
    {
        _playerCreature.Die();
    }

    public void EnterCombat()
    {
        Debug.Log($"EnterCombat — IsInCombat: {IsInCombat}, input enabled: {_input.enabled}, creature enabled: {_playerCreature.enabled}");
        if (IsInCombat) return;
        IsInCombat = true;
        _playerCreature.enabled = false;
        _input.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CombatManager.Instance.OnCombatVictory += HandleVictory;
        CombatManager.Instance.OnCombatDefeat += HandleDefeat;
        OnCombatEnter?.Invoke();
        _paramHandler?.EnterCombatLayer();
    }

    public void ExitCombat()
    {
        if (!IsInCombat) return;
        IsInCombat = false;
        _playerCreature.enabled = true;
        _input.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CombatManager.Instance.OnCombatVictory -= HandleVictory;
        CombatManager.Instance.OnCombatDefeat -= HandleDefeat;
        OnCombatExit?.Invoke();
        _paramHandler?.ExitCombatLayer();
    }

    public void EnterDialogue()
    {
        _playerCreature.enabled = false;
        _input.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitDialogue()
    {
        _playerCreature.enabled = true;
        _input.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
