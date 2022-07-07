using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int Ammo;
    [SerializeField] protected int AmmoCapacity;
    [SerializeField] protected int Damage;
    [SerializeField] public int FireRateSeconds;
    [SerializeField] protected int ReloadTime;
    [SerializeField] protected float Spread;
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected Transform WeaponPlace;
    [SerializeField] protected LayerMask TargetLayer;

    protected Rigidbody _rigidbodyBullet;
    protected Rigidbody _bulletInstance;
    protected Vector3 WeaponPosition;

    public abstract void Shoot(Vector3 target);
    public abstract void Reload();

    protected virtual void Start()
    {
        if(!Bullet.TryGetComponent<Rigidbody>(out _rigidbodyBullet)) 
            throw new ArgumentException("Weapon: miss rigidbody on bullet");

        if (Ammo > AmmoCapacity) throw new ArgumentException("Weapon: Ammo > AmmoCapacity");
    }

    protected virtual void OnValidate()
    {
        if (Ammo > AmmoCapacity) Debug.Log("Weapon: Ammo > AmmoCapacity");
    }
}
