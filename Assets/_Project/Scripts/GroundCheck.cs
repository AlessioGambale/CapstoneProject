using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _checkDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(_checkPoint.position, Vector3.down, _checkDistance, _groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (_checkPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_checkPoint.position, _checkPoint.position + Vector3.down * _checkDistance);
    }
}
