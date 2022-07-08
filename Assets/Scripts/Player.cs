using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IDamageble
{
    [SerializeField] private string Name;
    [SerializeField] private int HealthAmount = 100;

    public void Detected()
    {
        Debug.Log($"Player: {Name} detected!!!");
    }

    public void Die()
    {
        Debug.Log($"Player: die");
        Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        if(damage < HealthAmount)
            HealthAmount = HealthAmount - damage;
        else Die();
        Debug.Log($"Player: get {damage} damage");
    }

    public int Health()
    {
        throw new System.NotImplementedException();
    }
}