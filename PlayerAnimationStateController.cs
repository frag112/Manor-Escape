using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

#if ENABLE_INPUT_SYSTEM 
[RequireComponent(typeof(PlayerInput))]
#endif
public class PlayerAnimationStateController : MonoBehaviour
{
    private CharacterController _controller;
    Animator _animator;

    int _isWalkingHash, _isAimingHash, _isShootingHash;

    PlayerControls _input;

    float _velocity, _turning, _playerSpeed = 1.0f;

    bool _runPressed, _aimingPressed, _turnTrigger, _shootingPressed;
   

    private bool _dead = false;
    [SerializeField] private CinemachineVirtualCamera _deathCam;
    [SerializeField] private GameObject _deathText;

    private void Awake()
    {
        _input = new PlayerControls();
       // _input.CharacterControls.Movement.performed += ctx => Debug.Log(ctx.ReadValueAsObject());

        _input.CharacterControls.Movement.performed += ctx => _velocity = ctx.ReadValue<float>();
        _input.CharacterControls.Movement.canceled += ctx => _velocity = 0f;
        _input.CharacterControls.Turning.performed += ctx => _turning = ctx.ReadValue<float>();
        _input.CharacterControls.Turning.canceled += ctx => _turning = 0f;

        _input.CharacterControls.Run.performed += ctx => _runPressed = ctx.ReadValueAsButton();
        _input.CharacterControls.Run.started += ctx => _turnTrigger = ctx.ReadValueAsButton();
        _input.CharacterControls.Aiming.performed += ctx => _aimingPressed = ctx.ReadValueAsButton();
        _input.CharacterControls.Shoot.performed += ctx => _shootingPressed = ctx.ReadValueAsButton();

    }
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.Log("no animator");
        }
        _isWalkingHash = Animator.StringToHash("Velocity");
        _isAimingHash = Animator.StringToHash("IsAiming");
        _isShootingHash = Animator.StringToHash("IsShooting"); 
    }


    void Update()
    {
        if (_dead)
        {
            return;
        }
        HandleMovement();
        HandleAiming();


    }
    private void FixedUpdate()
    {
        if (_dead)
        {
            return;
        }
        Vector3 move = transform.forward;

        _controller.SimpleMove(_playerSpeed * move * Time.deltaTime * _velocity);//_playerSpeed *
        Vector3 rotation = new Vector3(0, 0, 0);
        rotation = new Vector3(0, _turning * Time.deltaTime * 90f, 0);
        this.transform.Rotate(rotation);
    }
    void HandleMovement()
    {
        float _isWalking = _animator.GetFloat(_isWalkingHash);
        // move variable
        //Vector3 rotation = new Vector3(0,0,0);
        _playerSpeed = 30f;
        if (_velocity != 0 && !_aimingPressed)
        {
            _animator.SetFloat(_isWalkingHash, _velocity);
            if (_velocity > 0)
            {
                _playerSpeed = 60f;
                if (_runPressed)
                {
                    _playerSpeed = 200f;
                }
            }

            // moving

        }
        if ((_velocity == 0) && (_isWalkingHash !=0))
        {
            _animator.SetFloat(_isWalkingHash, 0f);
        }
        if (_velocity > 0 && _runPressed)
        {

            _animator.SetFloat(_isWalkingHash, 1.2f);
        }
        if (_turning !=0)
        {
            //rotation = new Vector3(0, _turning * Time.deltaTime * 90f, 0);
            
        }

        // напрямую обращается к контроллеру, это нормально? можно ли поменять как то?
        // если использовать триггер то анимация запускается два раза, не знаю как поправить
        //if ((_velocity <0) && (_runPressed))
        //{
        //    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Fast Turn"))
        //    {
        //        //_animator.Play("Base Layer.Fast Turn", 0, 0);
        //        _animator.SetTrigger("FastTurn");
        //        this.transform.Rotate(0,Mathf.Lerp(this.transform.rotation.y, this.transform.rotation.y + 180, Time.deltaTime*2f),0);
        //    }
        //}
        if (_velocity == 0)// && !_runPressed)
        {
            _animator.SetFloat(_isWalkingHash, 0f);
        }
    }
    public bool Death(Vector3 lookhere)
    {
        if (_dead)
        {
            return false;
        }
        StartCoroutine(DeathRoutine());
        
        
        _controller.enabled = false;
        transform.LookAt(lookhere);
        _animator.SetTrigger("Bitten");
        _dead = true;
        return _dead;
    }
    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds (1f);
        _deathCam.Priority= 100;
        _deathText.SetActive(true);
        //for (int i = 0; i< 100; i++ )
        //{
        //    _deathText.transform.localScale = _deathText.transform.localScale * (i/100);
        //    yield return new WaitForEndOfFrame();
        //}

    }
    void HandleAiming()
    {
        bool _isAiming = _animator.GetBool(_isAimingHash);
        bool _isShooting = _animator.GetBool(_isShootingHash);

        if (_aimingPressed && (!_isAiming))
        {
            _animator.SetBool(_isAimingHash, true);
        }
        if ((!_aimingPressed) && _isAiming)
        {
            _animator.SetBool(_isAimingHash, false);
        }
        if (_isAiming && _shootingPressed)
        {
            _animator.SetBool(_isShootingHash, true);
        }
        if (!_shootingPressed)
        {
            _animator.SetBool(_isShootingHash, false);
        }
    }
    private void OnEnable()
    {
        _input.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        _input.CharacterControls.Disable();
    }
}
