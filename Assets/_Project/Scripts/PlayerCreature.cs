using UnityEngine;

public class PlayerCreature : Creature
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("References")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private GroundCheck _groundCheck;

    private bool _isJumping = false;

    private AnimationParamHandler _paramHandler; 
    private Rigidbody _rb;

    private void OnEnable()
    {
        _groundCheck.OnIsGroundedChange += HandleJump;
    }
    protected override void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _paramHandler = GetComponent<AnimationParamHandler>();  
        if (_input == null) _input = GetComponent<InputHandler>();
        base.Awake();
        

    }
    private void OnTriggerEnter()
    { //
        CombatManager.Instance.RegisterPlayer(this);
    }

    void FixedUpdate()
    {
        Move();

    }
    private void Update()
    {
        _paramHandler.SetForward(_input.MoveInput.magnitude);
        Jump();

    }

    private void Move()
    {
        if (!_input.IsMovementPressed) return;

        Vector3 moveDir = Camera.main.transform.TransformDirection(_input.MoveInput);
        moveDir.y = 0f;
        moveDir.Normalize();

        Vector3 targetVelocity = moveDir * _moveSpeed;
        _rb.velocity = new Vector3(targetVelocity.x, _rb.velocity.y, targetVelocity.z);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        if (_input.IsJumpPressed && _groundCheck.IsGrounded && !_isJumping)
        {
            _isJumping = true;  
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z); 
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _paramHandler.Jump();
        }
    }
    
    public void HandleJump(bool IsGrounded)
    {
        if (IsGrounded)
        {
            _isJumping = false;
           _paramHandler.ResetJump();
        }
        _paramHandler.SetIsGrounded(IsGrounded);
    }

    public override void Hit(float damage)
    {
        if (LifeController == null)
        {
            Debug.Log("lifc Null sul player");
        }
        float finalDamage = LifeController.IsHpCritical ? damage * 1.5f : damage;
        base.Hit(finalDamage);
    }

    public override void Die()
    {
        _paramHandler.Death();   
    }

    private void OnDisable()
    {
        _groundCheck.OnIsGroundedChange -= HandleJump;
    }

}
