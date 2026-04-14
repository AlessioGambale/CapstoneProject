using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDropManager : MonoBehaviour
{
    [SerializeField] private List<SO_Weapon> _weapons;
    [SerializeField] private List<SO_Ability> _abilities;

    public static RandomDropManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void GetRandomDrop()
    {
        GetRandomAbilty();
        GetRandomWeapon();
    }

    public void GetRandomAbilty()
    {
        int random = Random.Range(0, _abilities.Count);
        InventoryManager.Instance.AddItem(_abilities[random]);
    }

    public void GetRandomWeapon()
    {
        int random = Random.Range(0, _weapons.Count);
        InventoryManager.Instance.AddItem(_weapons[random]);
    }
}
