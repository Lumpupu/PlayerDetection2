using UnityEngine;
[RequireComponent(typeof(Collider))]
public class AreaDestroyer : MonoBehaviour
{
    private void OnCollisionExit(Collision other) =>
        Destroy(other.gameObject);
}
