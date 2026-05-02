using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _droppedItemPrefab;
    [SerializeField] private List<SO_GenericItem> _possibleDrops;
    [SerializeField] private int _coinDrop = 10;

    private void Start()
    {
        CombatManager.Instance.OnCombatVictory += OnCombatVictory;
    }

    private void OnCombatVictory()
    {
        CombatManager.Instance.OnCombatVictory -= OnCombatVictory;

        if (_possibleDrops.Count > 0)
        {
            SO_GenericItem drop = _possibleDrops[Random.Range(0, _possibleDrops.Count)];
            GameObject obj = Instantiate(_droppedItemPrefab, transform.position + Vector3.up, Quaternion.identity);
            obj.GetComponent<DroppedItem>()?.SetUp(drop);
        }

        //CoinManager.Instance.AddCoin(_coinDrop);
    }

    private void OnDestroy()
    {
        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.OnCombatVictory -= OnCombatVictory;
        }
       
    }
}
