using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private RaycastHit _hit;
    private IDamageble _entity;

    public override void Shoot(Vector3 target)
    {
        if (Ammo >= 0)
        {
            if (Physics.Raycast(transform.position, target, out _hit, TargetLayer))
            {
                if (_hit.collider.TryGetComponent<IDamageble>(out _entity))
                {
                    WeaponPosition = WeaponPlace.position;
                    _bulletInstance = Instantiate(_rigidbodyBullet, WeaponPosition, Quaternion.identity);
                    _bulletInstance.AddForce((target - WeaponPosition).normalized * 5000);
                    _entity.GetDamage(Damage);
                    Ammo -= 1;
                }
            }
        }
    }

    public override void Reload() =>
        Ammo = AmmoCapacity;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    private void Reset()
    {
        Ammo = 10;
        AmmoCapacity = 10;
        Damage = 10;
        ReloadTime = 2;
        FireRateSeconds = 30/60; // 30 per minute -> second
    }
}
