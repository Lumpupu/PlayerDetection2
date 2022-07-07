using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IDamageble
{
    [SerializeField] private string Name;
    public void Detected()
    {
        Debug.Log($"{Name}: detected!!!");
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public int Health()
    {
        throw new System.NotImplementedException();
    }
}