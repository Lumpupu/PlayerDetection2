﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Flags] public enum FiringStatus { firing = 0, reload = 1 << 0, jammed = 1 << 1 }
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int Ammo;
    [SerializeField] protected int AmmoCapacity;
    [SerializeField] public float FireRateSeconds;
    [SerializeField] public float ReloadTime;
    [SerializeField] protected float Spread;
    [SerializeField] protected float JammingChance;
    [Space(5)]
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected Transform WeaponPlace;

    protected Rigidbody _rigidbodyBullet;
    protected Rigidbody _bulletInstance;
    protected Vector3 WeaponPosition;
    protected float _random;

    public abstract FiringStatus Shoot(Vector3 target);
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