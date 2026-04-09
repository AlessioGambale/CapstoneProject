using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerCreature _movement;
    [SerializeField] private InputHandler _input;

    private bool _isInCombat;

    public bool IsInCombat => _isInCombat;

    void Awake()
    {
        if (_movement == null)
            _movement = GetComponent<PlayerCreature>();

        if (_input == null)
            _input = GetComponent<InputHandler>();
    }

    void Update()
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
            _movement.enabled = false;
            _input.enabled = false;
        }
        else
        {
            _movement.enabled = true;
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
