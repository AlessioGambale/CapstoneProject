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
    [SerializeField] private GameObject _chestPrefab;
    [SerializeField] private Transform _chestSpawnPoint;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private bool _isBossFight;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private AudioClip _combatMusic;
    [SerializeField] private AudioSource _musicSource;


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
            {
                _combatCamera.transform.position = _cameraPosition.position;
                _combatCamera.transform.rotation = _cameraPosition.rotation;
            }
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"[CombatTrigger] Enter — triggered: {_triggered}, tag: {other.tag}");
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;
        _triggered = true;

        if (_playerSpawnPoint != null)
        {
            Debug.Log($"[CombatTrigger] Sposto player a {_playerSpawnPoint.position}");
            other.transform.position = _playerSpawnPoint.position;
        }

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

       
        other.GetComponent<UI_CombatantHUD>()?.Activate();
        foreach (var enemy in _enemies)
            enemy.GetComponent<UI_CombatantHUD>()?.Activate();

        if (_combatMusic != null && _musicSource != null)
        {
            _musicSource.clip = _combatMusic;
            _musicSource.Play();
        }

        CombatManager.Instance.OnCombatVictory += OnCombatVictory;
        CombatManager.Instance.OnCombatDefeat += OnCombatDefeat;
    }

    private void OnCombatVictory()
    {
        CombatManager.Instance.OnCombatVictory -= OnCombatVictory;
        CombatManager.Instance.OnCombatDefeat -= OnCombatDefeat;
        RunManager.Instance.SetFightWon();
        InventoryManager.Instance.ClearWeaponsAndAbilities();
        RunManager.Instance.ClearGearPulled();

        if (!_isBossFight && _chestPrefab != null && _chestSpawnPoint != null)
            Instantiate(_chestPrefab, _chestSpawnPoint.position, _chestSpawnPoint.rotation);

        if (_isBossFight && _winScreen != null)
            _winScreen.SetActive(true);

        CleanupCombat();
    }

    private void OnCombatDefeat()
    {
        CombatManager.Instance.OnCombatVictory -= OnCombatVictory;
        CombatManager.Instance.OnCombatDefeat -= OnCombatDefeat;

        if (_loseScreen != null)
            _loseScreen.SetActive(true);

        CleanupCombat();
    }

    private void CleanupCombat()
    {
        _collider.enabled = false;
        _combatCanvas.SetActive(false);
        _player.GetComponent<UI_CombatantHUD>()?.Deactivate();
        foreach (var enemy in _enemies)
            if (enemy != null)
            {
                enemy.GetComponent<UI_CombatantHUD>()?.Deactivate();
            }
               

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
