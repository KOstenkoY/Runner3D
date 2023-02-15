using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController), typeof(PlayerController), typeof(Animator))]
public class PlayerStateMachine : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerController _playerController;

    // state variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    private Animator _animator;

    private Coroutine _coroutine;
     
    [Header("For run")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _decriaseSpeed;

    [Header("For jump")]
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _terminalVelocity = -10f;
    [SerializeField] private float _minFall = -1.5f;
    [SerializeField] private float _speedRoadChanging = 10;     // when player change line

    private float _verticalSpeed;

    private float _targetLine;              // change name, Use on Update

    [Header("For slide")]
    [SerializeField] private float _slideTime = 0.7f;
    [SerializeField] private float _colliderHeight = 1f;            // move height of character controller in sliding time
    [SerializeField] private Vector3 _colliderCenterPosition;       // move position of character controller in sliding time

    [Header("For death")]
    [SerializeField] private float timeDeathAnimation = 1f;

    private bool _isDied = false;
    private bool _isJumpPressed = false;
    private bool _requireNewJump = false;
    private bool _isSlidingPressed = false;

    // getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerController PlayerController { get { return PlayerController;  } }
    public Animator Animator { get { return _animator; } }
    public Coroutine Coroutine { get { return _coroutine; } set { _coroutine = value; } }
    public float RunSpeed { get { return _runSpeed; } set { _runSpeed = value; } }
    public float Gravity => _gravity;
    public float JumpForce => _jumpForce;
    public float TerminalVelocity => _terminalVelocity;
    public float MinFall => _minFall;
    public float SlideTime => _slideTime;
    public float ColliderHeight => _colliderHeight;
    public Vector3 ColliderCenterPosition => _colliderCenterPosition;
    public float VerticalSpeed { get { return _verticalSpeed; } set { _verticalSpeed = value; } }
    public bool IsDied { get { return _isDied; } set { _isDied = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } set { _isJumpPressed = value; } }
    public bool RequireNewJump { get { return _requireNewJump; } set { _requireNewJump = value; } }
    public bool IsSlidingPressed { get { return _isSlidingPressed; } set { _isSlidingPressed = value; } }

    private void OnEnable()
    {
        SwipeDetection.OnSwipeUp += OnJump;
        SwipeDetection.OnSwipeDown += OnSlide;
    }

    private void OnDisable()
    {
        SwipeDetection.OnSwipeUp -= OnJump;
        SwipeDetection.OnSwipeDown -= OnSlide;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerController>();

        _animator = GetComponent<Animator>();

        _verticalSpeed = _minFall;

        // setup states
        _states = new PlayerStateFactory(this);
        _currentState = _states.Run();
        _currentState.EnterState();
    }

    private void OnJump()
    {
        _isJumpPressed = true;
        _requireNewJump = false;
    }

    private void OnSlide()
    {
        _isSlidingPressed = true;
    }

    public void OnDied()
    {
        _isDied = true;
    }

    private void Update()
    {
        _currentState.UpdateState();

        _targetLine = Mathf.Lerp(_targetLine, _playerController.CurrentLine, _speedRoadChanging * Time.deltaTime);

        Vector3 movement = new Vector3(_targetLine - transform.position.x, _verticalSpeed * Time.deltaTime, _runSpeed * Time.deltaTime);
        _characterController.Move(movement);
    }
}
