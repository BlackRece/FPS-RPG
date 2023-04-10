using System;
using BlackRece;
using BlackRece.ProjectilePooler;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("PlayerController")]
    [SerializeField] private Camera _Camera;
    [SerializeField, Range(1, 10)] private float _WalkingSpeed = 3.0f;
    [SerializeField, Range(2, 20)] private float _RunningSpeed = 4.0f;
    [SerializeField, Range(0, 20)] private float _JumpSpeed = 6.0f;
    [SerializeField, Range(0.5f, 10)] private float _LookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] private float _LookXLimit = 80.0f;
    [SerializeField, Range(1, 100)] private float _ActionRange = 10.0f;
    [SerializeField] private bool _InverseY = false;

    [Space(20)] [Header("Advance")]
    [SerializeField] private bool _IsJumping;
    [SerializeField] private bool _IsFiring;
    [SerializeField] private float _Gravity = 20.0f;
    [SerializeField] private float timeToRunning = 2.0f;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool _CanRunning = true;
    
    private ProjectilePooler _pooler;
    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Vector3 _rotation;
    private bool _isRunning;
    private float _moveSpeed;
    private Quaternion _projectileRotation;
    private static Vector3 _position;
    [SerializeField] private Podium.PodiumIDs _podium;

    public static Vector3 GetPlayerPosition() => _position;
    
    public static bool IsPlayerInArena() => _isInArena;
    private bool _IsInArena { get => _isInArena; set => _isInArena = value; }
    private static bool _isInArena = true;
    [SerializeField] private bool _ArenaFlag;

    private void Awake()
    {
        if (_Camera == null)
            _Camera = GetComponentInChildren<Camera>();
        
        _characterController = GetComponent<CharacterController>();
        _pooler = GetComponent<ProjectilePooler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _rotation = _Camera.transform.localRotation.eulerAngles;
        _pooler.Init();
        _podium = Podium.PodiumIDs.None;
    }

    private void Update()
    {
        HandleMouseAim();
        HandleMovement();
        HandleActions();
        HandlePodiumInteraction();
        
        _position = transform.position;
        _ArenaFlag = _isInArena;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Podium>(out var podium))
        {
            _podium = podium.GetPodiumID();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _podium = Podium.PodiumIDs.None;
    }

    public void HandleTeleport(Vector3 pos, bool isArena)
    {
        _characterController.enabled = false;
        transform.position = pos;
        _IsInArena = isArena;
        _characterController.enabled = true;
    }

    private void HandleMouseAim()
    {
        var lookVertical = Input.GetAxis("Mouse Y") * _LookSpeed;
        _rotation.x += _InverseY ? -lookVertical : lookVertical;
        _rotation.x = Mathf.Clamp(_rotation.x, -_LookXLimit, _LookXLimit);
        _rotation.y = Input.GetAxis("Mouse X") * _LookSpeed;
        
        _Camera.transform.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
        transform.rotation *= Quaternion.Euler(0, _rotation.y, 0);
    }

    private void HandleMovement()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
        
        _IsJumping = !_characterController.isGrounded;
        if (!_characterController.isGrounded)
            _moveDirection.y -= _Gravity * Time.deltaTime;

        var moveDirectionY = _moveDirection.y;
        _moveSpeed = _isRunning ? _RunningSpeed : _WalkingSpeed;
        _moveDirection = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")) * _moveSpeed;
        
        if (_characterController.isGrounded && Input.GetButton("Jump"))
            _moveDirection.y += _JumpSpeed;
        else
            _moveDirection.y = moveDirectionY;
        
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
    
    private void HandleActions()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _IsFiring = true;
            // Transform tTransform = transform;
            // tTransform.rotation = _camera.transform.rotation;
            var camrot = _Camera.transform.localRotation.eulerAngles;
            var plyrot = transform.rotation.eulerAngles;
            var rot = new Vector3(camrot.x, plyrot.y, 0.0f);
                
            _pooler
                .GetGameObject()
                .GetComponent<Projectile>()
                .Init(transform.position, rot);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            _isRunning = _CanRunning ? true : false;
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            _isRunning = false;
            _WalkingSpeed = _RunningSpeed / timeToRunning;
        }
    }

    private void HandlePodiumInteraction()
    {
        if (_podium == Podium.PodiumIDs.None)
        {
            UIManager.ToggleUpgradePanel(false);
            return;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            switch (_podium)
            {
                case Podium.PodiumIDs.Movement:
                case Podium.PodiumIDs.Resource:
                case Podium.PodiumIDs.Weapon:
                    UIManager.ToggleUpgradePanel(true);
                    break;
            }
        }
    }
}
