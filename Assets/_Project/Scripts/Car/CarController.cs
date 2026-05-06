using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _groundAlign = 15f;

    [Header("Ground Alignment")]
    [SerializeField] private Transform _frontPoint;
    [SerializeField] private Transform _backPoint;

    [Header("References")]
    [SerializeField] private PlayerCreature _playerCreature;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _seatTransform;

    private bool _isDriving;
    private bool _playerIsNear;
    private Transform _playerOriginalParent;
    private Rigidbody _rb;
    private Rigidbody _playerRb;

    private Vector3 _seatLocalPos;
    private Quaternion _seatLocalRot;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerRb = _playerCreature != null ? _playerCreature.GetComponent<Rigidbody>() : null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerIsNear)
        {
            if (!_isDriving) EnterCar();
            else ExitCar();
        }
    }

    private void FixedUpdate()
    {
        AlignToGround();

        if (!_isDriving) return;

        float _v = Input.GetAxis("Vertical");
        float _h = Input.GetAxis("Horizontal");

        Vector3 _move = transform.forward * _v * _speed;
        _rb.velocity = new Vector3(_move.x, _rb.velocity.y, _move.z);

        transform.Rotate(0f, _h * _turnSpeed * Time.fixedDeltaTime, 0f, Space.Self);

        if (_playerRb != null) _playerRb.velocity = Vector3.zero;
        _playerCreature.transform.localPosition = _seatLocalPos;
        _playerCreature.transform.localRotation = _seatLocalRot;
    }

    private void AlignToGround()
    {
        if (_frontPoint == null || _backPoint == null) return;

        bool _frontHit = Physics.Raycast(_frontPoint.position, Vector3.down, out RaycastHit _hitFront, 1.5f);
        bool _backHit = Physics.Raycast(_backPoint.position, Vector3.down, out RaycastHit _hitBack, 1.5f);

        Debug.DrawRay(_frontPoint.position, Vector3.down * 1.5f, _frontHit ? Color.green : Color.red);
        Debug.DrawRay(_backPoint.position, Vector3.down * 1.5f, _backHit ? Color.green : Color.red);

        if (!_frontHit || !_backHit) return;

        Vector3 _forward = (_hitFront.point - _hitBack.point).normalized;
        Vector3 _normal = (_hitFront.normal + _hitBack.normal).normalized;

        float currentY = transform.eulerAngles.y;

        Quaternion _target = Quaternion.LookRotation(_forward, _normal);

        Vector3 euler = _target.eulerAngles;
        euler.y = currentY;

        Quaternion finalRot = Quaternion.Euler(euler);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, _groundAlign * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player")) _playerIsNear = true;
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player")) _playerIsNear = false;
    }

    private void EnterCar()
    {
        _isDriving = true;
        _playerCreature.enabled = false;

        if (_playerRb != null) _playerRb.velocity = Vector3.zero;

        _playerOriginalParent = _playerCreature.transform.parent;
        _playerCreature.transform.SetParent(transform);

        _seatLocalPos = _seatTransform ? _seatTransform.localPosition : new Vector3(0, 0.3f, 0);
        _seatLocalRot = _seatTransform ? _seatTransform.localRotation : Quaternion.identity;

        _playerCreature.transform.localPosition = _seatLocalPos;
        _playerCreature.transform.localRotation = _seatLocalRot;

        _playerAnimator?.SetBool("IsSitting", true);
    }

    private void ExitCar()
    {
        _isDriving = false;

        _playerCreature.transform.SetParent(_playerOriginalParent);
        _playerCreature.transform.position = transform.position + transform.right * 0.1f;

        _playerAnimator?.SetBool("IsSitting", false);
        _playerCreature.enabled = true;

        _rb.velocity = Vector3.zero;

    }

    public void SetColor(Color _c) =>
        GetComponentInChildren<Renderer>().material.color = _c;
}