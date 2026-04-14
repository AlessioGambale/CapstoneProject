using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCreature _playerCreature;
    [SerializeField] private InputHandler _input;

    private bool _isInCombat;
    public bool IsInCombat => _isInCombat;

    private void Awake()
    {
        if (_playerCreature == null)
            _playerCreature = GetComponent<PlayerCreature>();
        if (_input == null)
            _input = GetComponent<InputHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            EnterCombat();
        if (Input.GetKeyDown(KeyCode.V))
            ExitCombat();

        HandleState();
    }

    private void HandleState()
    {
        if (_isInCombat)
        {
            _playerCreature.enabled = false;
            _input.enabled = false;
        }
        else
        {
            _playerCreature.enabled = true;
            _input.enabled = true;
        }
    }

    public void EnterCombat()
    {
        _isInCombat = true;
    }

    public void ExitCombat()
    {
        _isInCombat = false;
    }
}
