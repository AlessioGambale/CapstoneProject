using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CombatTriggerZone : MonoBehaviour
{
    [SerializeField] private List<EnemyCreature> _enemies;
    [SerializeField] private GameObject _combatCanvas;
    [SerializeField] private GameObject _cubeParent;
    [SerializeField] private CinemachineVirtualCamera _combatCamera;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Collider _collider;
    [SerializeField] private string _zoneId;

    private Transform _player;
    private bool _triggered;

    private void Start()
    {
        if (RunManager.Instance.IsZoneTriggered(_zoneId))
        {
            _triggered = true;
            _collider.enabled = false;
        }

        if (_combatCamera != null)
        {
            _combatCamera.Priority = 0;
            if (_cameraPosition != null)
                _combatCamera.transform.position = _cameraPosition.position;
            _combatCamera.transform.rotation = _cameraPosition.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;
        _triggered = true;
        RunManager.Instance.RegisterZoneTriggered(_zoneId);
        _player = other.transform;

        if (_combatCamera != null)
        {
            _combatCamera.transform.SetParent(null);
            if (_cameraPosition != null)
            {
                _combatCamera.transform.position = _cameraPosition.position;
                _combatCamera.transform.rotation = _cameraPosition.rotation;
            }
            _combatCamera.gameObject.SetActive(true);
            _combatCamera.Priority = 20;
        }

        if (_cubeParent != null)
            _cubeParent.SetActive(true);

        _combatCanvas.SetActive(true);

        CombatManager.Instance.RegisterPlayer(other.GetComponent<PlayerCreature>());

        foreach (var enemy in _enemies)
            CombatManager.Instance.RegisterEnemy(enemy);

        other.GetComponentInChildren<PlayerToCombatTransition>()?.Trigger();

        CombatManager.Instance.OnCombatVictory += OnCombatVictory;
        CombatManager.Instance.OnCombatDefeat += OnCombatDefeat;
    }

    private void OnCombatVictory()
    {
        CombatManager.Instance.OnCombatVictory -= OnCombatVictory;
        CombatManager.Instance.OnCombatDefeat -= OnCombatDefeat;
        RunManager.Instance.SetFightWon();
        CleanupCombat();
    }

    private void OnCombatDefeat()
    {
        CombatManager.Instance.OnCombatVictory -= OnCombatVictory;
        CombatManager.Instance.OnCombatDefeat -= OnCombatDefeat;
        CleanupCombat();
    }

    private void CleanupCombat()
    {
        _collider.enabled = false;

        if (_combatCamera != null)
        {
            _combatCamera.transform.SetParent(_player);
            _combatCamera.gameObject.SetActive(false);
            _combatCamera.Priority = 0;
        }

        if (_cubeParent != null)
            _cubeParent.SetActive(false);

        foreach (var enemy in _enemies)
            if (enemy != null)
                Destroy(enemy.gameObject, 5f);
    }
}
