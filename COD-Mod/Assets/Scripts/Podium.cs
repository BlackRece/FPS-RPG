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
    
    [SerializeField] private GameObject _IconGameObject;
    [SerializeField, Range(0f, 100f)] private float _RotationSpeed = 50f;
    [SerializeField] private PodiumIDs _PodiumID;
    public PodiumIDs GetPodiumID() => _PodiumID;
    
    private void Awake()
    {
        if (_IconGameObject == null)
            throw new System.ArgumentNullException("ERROR: Icon GameObject is null!");
    }

    private void Update()
    {
        _IconGameObject.transform.Rotate(Vector3.up * Time.deltaTime * _RotationSpeed);
    }

    public void OnInteract()
    {
        Debug.Log("Interacted with podium");
    }
}
