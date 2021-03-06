using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System = UnityEngine.Random;
public class Pistol : Weapon
{
    public override FiringStatus TryShoot(Vector3 target)
    {
        if (Ammo >= 0)
        {
            _random = Random.Range(-Spread, +Spread);
            target.Set(target.x + _random, target.y + _random, target.z);

            WeaponPosition = WeaponPlace.position;
            _bulletInstance = Instantiate(_rigidbodyBullet, WeaponPosition, Quaternion.identity);
            _bulletInstance.AddForce((target - WeaponPosition).normalized * 5000);
            Ammo -= 1;

            _random = Random.Range(0, 100);
            if(_random < JammingChance)
                return FiringStatus.jammed;
            
            return FiringStatus.firing;
        }
        else return FiringStatus.reload;
    }

    public override void Reload() =>
        Ammo = AmmoCapacity;
    
    protected override void Start()
    {
        base.Start();
        UnityEngine.Random.InitState(123);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    private void Reset()
    {
        Ammo = 10;
        AmmoCapacity = 10;
        ReloadTime = 2;
        FireRateSeconds = 30/60; // 30 per minute -> second
    }
}
