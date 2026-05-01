using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _selectionOverlay;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Button _selectButton;

    [Header("Status Buildup")]
    [SerializeField] private Image _buildupFill;
    [SerializeField] private GameObject _buildupBarRoot;

    [Header("Colors")]
    [SerializeField] private Color _green = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color _yellow = new Color(0.9f, 0.8f, 0.1f);
    [SerializeField] private Color _red = new Color(0.9f, 0.2f, 0.1f);

    private static readonly Dictionary<StatusType, Color> _statusColors = new Dictionary<StatusType, Color>
    {
        { StatusType.Bleeding,  new Color(0.8f, 0.1f, 0.1f) },
        { StatusType.Weakness,  new Color(0.9f, 0.5f, 0.1f) },
        { StatusType.Stun,      new Color(0.9f, 0.9f, 0.1f) },
        { StatusType.Fracture,  new Color(0.4f, 0.7f, 1.0f) },
        { StatusType.Panic,     new Color(0.7f, 0.1f, 0.9f) },
    };

    private EnemyCreature _enemy;
    private LifeController _lifeController;
    private StatusController _statusController;
    private Coroutine _hpRoutine;
    private Coroutine _buildupRoutine;

    public void SetupAsEnemy(EnemyCreature enemy)
    {
        if (enemy.LifeController != null)
            enemy.LifeController.OnHealthChange -= UpdateBar;

        if (_statusController != null)
        {
            _statusController.OnBuildupChanged -= UpdateBuildup;
            _statusController.OnStatusApplied -= HandleStatusApplied;
            _statusController.OnStatusExpired -= HandleStatusExpired;
        }

        _enemy = enemy;
        _lifeController = enemy.LifeController;
        _nameText.SetText(enemy.CreatureName);

        if (_selectButton != null)
        {
            _selectButton.gameObject.SetActive(true);
            _selectButton.onClick.RemoveAllListeners();
            _selectButton.onClick.AddListener(OnSelected);
        }

        _statusController = enemy.GetComponent<StatusController>();
        if (_statusController != null)
        {
            _statusController.OnBuildupChanged += UpdateBuildup;
            _statusController.OnStatusApplied += HandleStatusApplied;
            _statusController.OnStatusExpired += HandleStatusExpired;
        }

        if (_buildupBarRoot != null) _buildupBarRoot.SetActive(true);
        if (_buildupFill != null)
        {
            _buildupFill.fillAmount = 0f;
            _buildupFill.color = Color.white;
        }

        enemy.LifeController.OnHealthChange += UpdateBar;
        UpdateBar(enemy.LifeController.CurrentHealth, enemy.LifeController.MaxHealth);
    }

    public void SetupAsPlayer(LifeController lifeController)
    {
        if (_lifeController != null)
            _lifeController.OnHealthChange -= UpdateBar;

        _lifeController = lifeController;

        if (_selectButton != null)
            _selectButton.gameObject.SetActive(false);

        if (_buildupBarRoot != null)
            _buildupBarRoot.SetActive(false);

        _lifeController.OnHealthChange += UpdateBar;
        UpdateBar(_lifeController.CurrentHealth, _lifeController.MaxHealth);
    }

    private void UpdateBar(int current, int max)
    {
        Debug.Log($"[UI_HpBar] UpdateBar — {current}/{max} — fillAmount target: {(float)current / max}");
        float percent = (float)current / max;
        if (_hpRoutine != null)
            StopCoroutine(_hpRoutine);
        _hpRoutine = StartCoroutine(AnimateBar(percent));
        UpdateColor(percent);
    }

    private IEnumerator AnimateBar(float target)
    {
        float start = _fillImage.fillAmount;
        float time = 0f;
        float duration = 0.25f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _fillImage.fillAmount = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }
        _fillImage.fillAmount = target;
    }

    private void UpdateColor(float percent)
    {
        if (percent > 0.5f)
            _fillImage.color = _green;
        else if (percent > 0.2f)
            _fillImage.color = _yellow;
        else
            _fillImage.color = _red;
    }

    private void UpdateBuildup(float normalized)
    {
        if (_buildupFill == null) return;
        if (_buildupRoutine != null)
            StopCoroutine(_buildupRoutine);
        _buildupRoutine = StartCoroutine(AnimateBuildup(normalized));
    }

    private IEnumerator AnimateBuildup(float target)
    {
        float start = _buildupFill.fillAmount;
        float time = 0f;
        float duration = 0.3f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _buildupFill.fillAmount = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }
        _buildupFill.fillAmount = target;
    }

    private void HandleStatusApplied(StatusType type)
    {
        if (_buildupFill == null) return;
        if (_statusColors.TryGetValue(type, out Color c))
            _buildupFill.color = c;
        _buildupFill.fillAmount = 1f;
    }

    private void HandleStatusExpired()
    {
        if (_buildupFill == null) return;
        _buildupFill.fillAmount = 0f;
        _buildupFill.color = Color.white;
    }

    public void RefreshTargetButton()
    {
        if (_selectButton == null || _enemy == null) return;
        bool canSelect = CombatManager.Instance.isTargeting && !_enemy.IsDead;
        Debug.Log($"RefreshTargetButton — isTargeting: {CombatManager.Instance.isTargeting}, interactable: {canSelect}");
        _selectButton.interactable = canSelect;
    }

    private void OnSelected()
    {
        Debug.Log($"OnSelected chiamato — enemy: {_enemy?.name}, isDead: {_enemy?.IsDead}, isTargeting: {CombatManager.Instance.isTargeting}");
        if (_enemy == null || _enemy.IsDead) return;
        CombatManager.Instance.SelectTarget(_enemy);
    }

    public void SetHighlighted(bool value)
    {
        if (_selectionOverlay == null) return;
        StopCoroutine(nameof(FadeOverlay));
        StartCoroutine(FadeOverlay(value ? 0.25f : 0f));
    }

    private IEnumerator FadeOverlay(float targetAlpha)
    {
        float start = _selectionOverlay.color.a;
        float time = 0f;
        float duration = 0.15f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Lerp(start, targetAlpha, time / duration);
            _selectionOverlay.color = new Color(1f, 1f, 1f, a);
            yield return null;
        }
        _selectionOverlay.color = new Color(1f, 1f, 1f, targetAlpha);
    }

    private void OnDestroy()
    {
        if (_lifeController != null)
        {
            _lifeController.OnHealthChange -= UpdateBar;
        }

        if (_lifeController != null)
        {
            _lifeController.OnHealthChange -= UpdateBar;
        }
          
        if (_statusController != null)
        {
            _statusController.OnBuildupChanged -= UpdateBuildup;
            _statusController.OnStatusApplied -= HandleStatusApplied;
            _statusController.OnStatusExpired -= HandleStatusExpired;
        }
    }
}
