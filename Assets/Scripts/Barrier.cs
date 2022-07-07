using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour, IHealth, IDamageble
{
    [SerializeField] private int HealthAmount = 300;
    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        HealthAmount -= damage;
        if(HealthAmount <= 0) Die();
    }

    public int Health()
    {
        return HealthAmount;
    }
}
