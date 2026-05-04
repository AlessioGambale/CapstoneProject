using UnityEngine;

public class AnimationParamHandler : MonoBehaviour
{
    [SerializeField] private string _forwardName = "Forward";
    [SerializeField] private string _isOpenName = "IsOpen";
    [SerializeField] private string _isInsideName = "IsInside";
    [SerializeField] private string _jumpName = "Jump";
    [SerializeField] private string _deathName = "Death";
    [SerializeField] private string _isGrounded = "IsGrounded";
    [SerializeField] private string _attackName = "Attack";
    [SerializeField] private string _specialAttackName = "SpecialAttack";
    [SerializeField] private string _abilityName = "Ability";
    [SerializeField] private string _healName = "Heal";
    [SerializeField] private string _takeHitName = "TakeHit";

    private int _combatLayerIndex;


    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        Debug.Log($"[AnimParamHandler] Animator trovato: {_animator}, su: {gameObject.name}");
        _combatLayerIndex = _animator.GetLayerIndex("Combat");
    }

    public void EnterCombatLayer()
    {
        _animator.SetLayerWeight(_combatLayerIndex, 1f);
    }

    public void ExitCombatLayer()
    {
        _animator.SetLayerWeight(_combatLayerIndex, 0f);
    }


    public void SetForward(float speed)
    {
        _animator.SetFloat(_forwardName, speed);
    }
    
    public void Open()
    {
        _animator.SetTrigger(_isOpenName);
    }
    
    public void OnIsInside()
    {
        _animator.SetTrigger(_isInsideName);
    }

    public void Jump()
    {
        _animator.SetTrigger(_jumpName);
    }

    public void Death()
    {
        _animator.SetTrigger(_deathName);
    }

    public void SetIsGrounded(bool isGrounded)
    {
        _animator.SetBool(_isGrounded, isGrounded);
    }

    public void ResetJump()
    {
        _animator.ResetTrigger(_jumpName);
    }

    public void Attack()
    {
        Debug.Log($"[AnimParamHandler] Attack trigger — animator: {_animator}, gameObject: {gameObject.name}");
        _animator.SetTrigger(_attackName);
    }

    public void SpecialAttack()
    {
        _animator.SetTrigger(_specialAttackName);
    }

    public void Ability()
    {
        _animator.SetTrigger(_abilityName);
    }

    public void Heal()
    {
        _animator.SetTrigger(_healName);
    }
    public void TakeHit()
    {
        _animator.SetTrigger(_takeHitName);
    }
}