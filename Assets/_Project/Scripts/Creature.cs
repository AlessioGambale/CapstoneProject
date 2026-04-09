using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] private string _creatureName;
    [SerializeField] private SO_Stats _stats;

    protected LifeController LifeController { get; private set; }

    public virtual void Hit(float damage)
    {
        LifeController.TakeDamage(damage ,_stats.DefencePercent);
    }

    public virtual void Die()
    {
        
    }
  

   
}
