using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ConsumableHeal")]
public class SO_Consumable : SO_GenericItem
{
    [SerializeField] private ConsumableType _type;
    [SerializeField] private float _value;

    public override void Use(GameObject user)
    {
        PlayerCreature player = user.GetComponent<PlayerCreature>();
        if (player == null) return;

        switch (_type)
        {
            case ConsumableType.Heal:
                int healAmount = Mathf.RoundToInt(player.LifeController.MaxHealth * _value);
                player.LifeController.AddHp(healAmount);
                break;
        }
    }
}

public enum ConsumableType
{
    Heal
}
