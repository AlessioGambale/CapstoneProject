using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    [Header("PickUp Settings")]
    [SerializeField] private float _altitude = 0.1f;
    [SerializeField] private float _rotationSpeed = 150f;
    [SerializeField] private float _altitudeSpeed = 1.0f;
    [SerializeField] private float _lifeSpan;

    [Header("Events")]
    [SerializeField] private UnityEvent _onPicK;

    private Vector3 _position;
    protected virtual void OnPick(GameObject player)
    {
        _onPicK.Invoke();
    }
    private void Start()
    {
        _position = transform.position;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        float offSetY = Mathf.Sin(Time.time * _altitudeSpeed) * _altitude;
        transform.position = _position + Vector3.up * offSetY;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnPick(other.gameObject);
        Destroy(gameObject, _lifeSpan);
    }
}
