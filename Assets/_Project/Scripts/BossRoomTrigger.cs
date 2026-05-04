using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _bossRoomPrefab;
    [SerializeField] private Transform _spawnPoint;
    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;
        if (!RunManager.Instance.BossUnlocked) return;
        _triggered = true;
        Instantiate(_bossRoomPrefab, _spawnPoint.position, _spawnPoint.rotation);
    }
}
