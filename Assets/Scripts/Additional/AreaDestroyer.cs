using UnityEngine;
[RequireComponent(typeof(Collider))]
public class AreaDestroyer : MonoBehaviour
{
    private void OnTriggerExit(Collider other) =>
        Destroy(other.gameObject);
}
