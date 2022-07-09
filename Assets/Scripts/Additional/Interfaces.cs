using UnityEngine;
interface IDamageble
{
    int Health{get;}
    void GetDamage(int damage);
}

interface IMoveable
{
    void Move();
}
