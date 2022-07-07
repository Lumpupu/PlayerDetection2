using UnityEngine;

interface IHealth
{
    int Health();
    void Die();
}

interface IDamageble
{
    void GetDamage(int damage);
}

interface IMovaeble
{
    void Move();
}
