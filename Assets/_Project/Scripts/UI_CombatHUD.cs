using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CombatHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_HpBar _playerHPBar;
    [SerializeField] private UI_HpBar _enemyHPBarPrefab;
    [SerializeField] private Transform _enemyHPBarsParent;

    private List<UI_HpBar> _enemyBars = new List<UI_HpBar>();

    private void OnEnable()
    {
        CombatManager.Instance.OnCombatStarted += Setup;
    }

    private void OnDisable()
    {
        CombatManager.Instance.OnCombatStarted -= Setup;
    }

    private void Setup()
    {
        SetupPlayer();
        SetupEnemies();
    }

    private void SetupPlayer()
    {
        var player = CombatManager.Instance.Player;

        if (player == null)
        {
            Debug.LogError("Player NON trovato nel CombatManager!");
            return;
        }

        _playerHPBar.SetupAsPlayer(player.LifeController);
    }

    private void SetupEnemies()
    {
        foreach (var bar in _enemyBars)
            Destroy(bar.gameObject);

        _enemyBars.Clear();

        List<EnemyCreature> enemies = CombatManager.Instance.Enemies;
        if (enemies == null) return;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            UI_HpBar bar = Instantiate(_enemyHPBarPrefab, _enemyHPBarsParent);
            bar.SetupAsEnemy(enemy);

            _enemyBars.Add(bar);
        }
    }

    private void Update()
    {
        bool targeting = CombatManager.Instance.isTargeting;
        foreach (var bar in _enemyBars)
        {
            bar.SetHighlighted(targeting);
            bar.RefreshTargetButton();
        }
    }
}
