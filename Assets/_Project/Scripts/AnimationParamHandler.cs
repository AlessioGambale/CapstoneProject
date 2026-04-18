using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParamHandler : MonoBehaviour
{
    [SerializeField] private string _forwardName = "Forward";
    [SerializeField] private string _isOpenName = "IsOpen";
    [SerializeField] private string _isInsideName = "IsInside";
    [SerializeField] private string _jumpName = "Jump";
    [SerializeField] private string _deathName = "Death";
    [SerializeField] private string _isGrounded = "IsGrounded";
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
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
}