using TMPro;
using UnityEngine;

public class ExplorationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _epText;

    private void Start()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.CoinChanged += UpdateCoins;
            UpdateCoins(CoinManager.Instance.Coins);
        }
    }

    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.CoinChanged -= UpdateCoins;
    }

    private void Update()
    {
        if (ExplorationManager.Instance != null)
            _epText.SetText($"EP: {ExplorationManager.Instance.CurrentEP}");
    }

    private void UpdateCoins(int coins) => _coinsText.SetText($"{coins}");
}
