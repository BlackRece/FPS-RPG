using BlackRece;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private float _maxHealth = 10.0f;
    [SerializeField] private float _gravity = 20.0f;
    
    private float _health;
    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private bool _pauseActions;
    private int _rank = 0;
    private int _resourceIncrease = 1;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Projectile>(out var projectile))
        {
            // increase resource counts
                
            // decrease enemy health
            _health -= projectile.Damage;
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleHealth();
        
        // replace with nav mesh agent
        HandleMovement();
    }
    
    public void Init(Vector3 pos, Vector3 rot)
    {
        _health = _rank < 1 
            ? _maxHealth
            : _maxHealth * _rank;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
        gameObject.SetActive(true);
    }
    
    private void HandleHealth()
    {
        if (_health <= 0)
        {
            var resourceAmount = _rank < 1 
                ? _resourceIncrease
                : _resourceIncrease * _rank + 1;
            UIManager.IncreaseAllResources(resourceAmount);
            gameObject.SetActive(false);
        }
    }

    private void HandleMovement()
    {
        //get player position
        var pos = FirstPersonController.GetPlayerPosition();
        if (_pauseActions)
        {
            _characterController.Move(transform.position);
            return;
        }

        // get direction to player
        _moveDirection = pos - transform.position;
        _moveDirection.y = 0;
        _moveDirection.Normalize();
        _moveDirection *= _speed;
        
        // apply gravity
        if (!_characterController.isGrounded)
            _moveDirection.y -= _gravity * Time.deltaTime;
        
        // move towards player
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
    
    public void HandlePause(bool isPaused)
    {
        _pauseActions = isPaused;
    }
}
