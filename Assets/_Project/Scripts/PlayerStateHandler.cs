using System;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCreature _playerCreature;
    [SerializeField] private InputHandler _input;

    public bool IsInCombat { get; private set; }
    public event Action OnCombatEnter;
    public event Action OnCombatExit;

    private void Awake()
    {
        if (_playerCreature == null) _playerCreature = GetComponent<PlayerCreature>();
        if (_input == null) _input = GetComponent<InputHandler>();
    }

    private void Start()
    {
        
    }

    private void HandleVictory()
    {
        GetComponent<PlayerToExplorationTransition>().TriggerTransition = true;
    }

    private void HandleDefeat()
    {
        _playerCreature.Die();
    }

    public void EnterCombat()
    {
        if (IsInCombat) return;
        Debug.Log($"[PSH] EnterCombat — _playerCreature: {_playerCreature}");
        IsInCombat = true;
        _playerCreature.enabled = false;
        _input.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CombatManager.Instance.OnCombatVictory += HandleVictory;
        CombatManager.Instance.OnCombatDefeat += HandleDefeat;

        CombatManager.Instance.RegisterPlayer(_playerCreature);
        OnCombatEnter?.Invoke();
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
    }
}
