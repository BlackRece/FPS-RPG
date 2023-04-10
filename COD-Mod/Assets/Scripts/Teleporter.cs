using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform _teleportTarget;
    [SerializeField] private bool _isInArena;
    private Collider _teleportCollider;
    
    // teleport from _teleportCollider to _teleportTarget
    private void Awake()
    {
        _teleportCollider = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<FirstPersonController>(out var player))
            player.HandleTeleport(_teleportTarget.position, _isInArena);
    }
}
