using UnityEngine;
using System.Diagnostics;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FirearmsBullet : MonoBehaviour
{
    [SerializeField] public int Damage;
    private IDamageble _entity;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageble>(out _entity))
            _entity.GetDamage(Damage);
    }
}
