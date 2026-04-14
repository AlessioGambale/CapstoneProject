using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemDropper : MonoBehaviour
{
    [SerializeField] private RandomDropManager _randomDropManager;
    private bool _isInTrigger = true;
    private bool _canDrop = true;

    private void Update()
    {
        if (!_isInTrigger) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (!_canDrop) return;
        RandomDropManager.Instance.GetRandomDrop();
        _canDrop = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isInTrigger = false;
    }
}
