using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _droppedItemPrefab;
    [SerializeField] private List<SO_GenericItem> _possibleDrops;
    [SerializeField] private int _coinDrop;
    private EnemyCreature _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyCreature>();
    }

    private void Start()
    {
        _enemy.LifeController.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        if (_possibleDrops.Count > 0)
        {
            SO_GenericItem drop = _possibleDrops[Random.Range(0 , _possibleDrops.Count)];
            GameObject obj = Instantiate(_droppedItemPrefab , transform.position + Vector3.up , Quaternion.identity);
            obj.GetComponent<DroppedItem>().SetUp(drop);
        }
        CoinManager.Instance.AddCoin(_coinDrop);
    }

    private void OnDestroy()
    {
        if (_enemy != null) 
            _enemy.LifeController.OnDeath -= OnDeath;
    }
}
