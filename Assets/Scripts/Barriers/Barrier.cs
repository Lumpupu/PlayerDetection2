using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Barrier : MonoBehaviour, IDamageble
{
    [SerializeField] private int HealthAmount = 300;
    private int _health;
    public int Health
    { 
        get => _health; 
        private set
        {
            if(value > _health) _health -= value;
            else Die();
        }
    }

    public void Die()
    {
        //Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
    }

    private void Start()
    {
        HealthAmount = _health;
    }
}
