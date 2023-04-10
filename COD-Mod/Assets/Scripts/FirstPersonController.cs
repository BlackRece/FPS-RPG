using BlackRece;
using BlackRece.ProjectilePooler;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("PlayerController")]
    [SerializeField] private Camera _camera;
    [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
    [SerializeField, Range(2, 20)] float runingSpeed = 4.0f;
    [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;
    [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
    [SerializeField] bool inverseY = false;

    [Space(20)] [Header("Advance")]
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isFiring;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float timeToRunning = 2.0f;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool CanRunning = true;
    
    private ProjectilePooler _pooler;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 rotation;
    private bool isRunning;
    private float moveSpeed;
    private Quaternion _projectileRotation;
    private static Vector3 _position;

    public static Vector3 GetPlayerPosition() => _position;
    
    public static bool IsPlayerInArena() => _isInArena;
    private bool _IsInArena { get => _isInArena; set => _isInArena = value; }
    private static bool _isInArena = true;
    [SerializeField] private bool _ArenaFlag;

    public void HandleTeleport(Vector3 pos, bool isArena)
    {
        characterController.enabled = false;
        transform.position = pos;
        _IsInArena = isArena;
        characterController.enabled = true;
    }
    
    private void Awake()
    {
        if (_camera == null)
            _camera = GetComponentInChildren<Camera>();
        
        characterController = GetComponent<CharacterController>();
        _pooler = GetComponent<ProjectilePooler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rotation = _camera.transform.localRotation.eulerAngles;
        _pooler.Init();
    }

    private void Update()
    {
        HandleMouseAim();
        HandleMovement();
        HandleActions();
        
        _position = transform.position;
        _ArenaFlag = _isInArena;
    }
    
    private void HandleMouseAim()
    {
        var lookVertical = Input.GetAxis("Mouse Y") * lookSpeed;
        rotation.x += inverseY ? -lookVertical : lookVertical;
        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
        rotation.y = Input.GetAxis("Mouse X") * lookSpeed;
        
        _camera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.rotation *= Quaternion.Euler(0, rotation.y, 0);
    }

    private void HandleMovement()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
        
        isJumping = !characterController.isGrounded;
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        var moveDirectionY = moveDirection.y;
        moveSpeed = isRunning ? runingSpeed : walkingSpeed;
        moveDirection = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")) * moveSpeed;
        
        if (characterController.isGrounded && Input.GetButton("Jump"))
            moveDirection.y += jumpSpeed;
        else
            moveDirection.y = moveDirectionY;
        
        characterController.Move(moveDirection * Time.deltaTime);
    }
    
    private void HandleActions()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isFiring = true;
            // Transform tTransform = transform;
            // tTransform.rotation = _camera.transform.rotation;
            var camrot = _camera.transform.localRotation.eulerAngles;
            var plyrot = transform.rotation.eulerAngles;
            var rot = new Vector3(camrot.x, plyrot.y, 0.0f);
                
            _pooler
                .GetGameObject()
                .GetComponent<Projectile>()
                .Init(transform.position, rot);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            var hit = new RaycastHit();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 100.0f))
            {
                Debug.Log(hit.collider.name);
            }
        }
        
        if (Input.GetButtonDown("Fire3"))
        {
            isRunning = CanRunning ? true : false;
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            isRunning = false;
            walkingSpeed = runingSpeed / timeToRunning;
        }
    }
}
