using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FirearmsBullet : MonoBehaviour
{
    [SerializeField] private int Damage;
    private IDamageble _entity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageble>(out _entity))
        {
            _entity.GetDamage(Damage);
            Destroy(this.gameObject);
        }
    }
}
