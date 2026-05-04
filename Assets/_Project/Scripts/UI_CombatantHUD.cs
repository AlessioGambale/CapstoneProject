using UnityEngine;


public class UI_CombatantHUD : MonoBehaviour
{
    public enum CombatantType { Player, Enemy }

    [Header("Config")]
    [SerializeField] private CombatantType _type;

    [Header("References")]
    [SerializeField] private UI_HpBar _hpBar;

    [Header("Billboard")]
    [SerializeField] private Transform _hudRoot;
    [SerializeField] private bool _billboard = true;

    private EnemyCreature _enemy;

    private void Awake()
    {
        if (_type == CombatantType.Enemy)
            _enemy = GetComponent<EnemyCreature>();
    }

    public void Activate()
    {
        if (_hudRoot != null)
            _hudRoot.gameObject.SetActive(true);

        if (_type == CombatantType.Player)
        {
            var player = CombatManager.Instance.Player;
            if (player == null) return;
            _hpBar.SetupAsPlayer(player.LifeController);
        }
        else
        {
            _hpBar.SetupAsEnemy(_enemy);
        }
    }

    public void Deactivate()
    {
        if (_hudRoot != null)
            _hudRoot.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_billboard && _hudRoot != null && Camera.main != null)
            _hudRoot.forward = Camera.main.transform.forward;

        if (_type == CombatantType.Enemy)
            _hpBar.RefreshTargetButton();
    }
}