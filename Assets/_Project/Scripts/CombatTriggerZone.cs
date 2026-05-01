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

    private Transform _player;
    private bool _triggered;

    private void Start()
    {
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
        _player = other.transform;

        _combatCamera.transform.SetParent(null);

        if (_combatCamera != null)
        {
            _combatCamera.Priority = 0;
            if (_cameraPosition != null)
                _combatCamera.transform.position = _cameraPosition.position;
            _combatCamera.transform.rotation = _cameraPosition.rotation;
        }

        _combatCamera.gameObject.SetActive(true);

        if (_combatCamera != null)
            _combatCamera.Priority = 20;
      
        if (_cubeParent != null)
            _cubeParent.SetActive(true);

        _combatCanvas.SetActive(true);
        

        var playerCreature = other.GetComponent<PlayerCreature>();
        CombatManager.Instance.RegisterPlayer(playerCreature);

        var toCombat = other.GetComponentInChildren<PlayerToCombatTransition>();
        toCombat.TriggerTransition = true;

        foreach (var enemy in _enemies)
            CombatManager.Instance.RegisterEnemy(enemy);

        CombatManager.Instance.OnCombatVictory += OnCombatEnd;
        CombatManager.Instance.OnCombatDefeat += OnCombatEnd;
    }

    private void OnCombatEnd()
    {
        CombatManager.Instance.OnCombatVictory -= OnCombatEnd;
        CombatManager.Instance.OnCombatDefeat -= OnCombatEnd;

        _combatCamera.transform.SetParent(_player);

        if (_combatCamera != null)
            _combatCamera.Priority = 0;

        if (_cubeParent != null)
            _cubeParent.SetActive(false);
    }
}
