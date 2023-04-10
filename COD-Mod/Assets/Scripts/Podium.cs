using UnityEngine;

public class Podium : MonoBehaviour, IInteractable
{
    public enum PodiumIDs
    {
        None,
        Movement,
        Resource,
        Weapon,
    }
    
    [SerializeField] private PodiumIDs _PodiumID;
    public PodiumIDs GetPodiumID() => _PodiumID;
    
    public void OnInteract()
    {
        Debug.Log("Interacted with podium");
    }
}
