using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string LAYERGROUND = "Ground";
    private const string ISALIVE = "isAlive";
    private const string ISGROUNDED = "isGrounded";
    private const string BUTTONFIRE1 = "Fire1";
    
    private float _jumpForce;
    private float _runningSpeed;
    private float _distanceTraveled;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private LayerMask _groundLayerMask;
    private Vector3 _startPosition;
    
    [SerializeField]
    private KillPlayerChannel _killPlayerChannel;
    [SerializeField]
    private GameStateChannel _gameStateChannel;

    private void Awake()
    {
        this._groundLayerMask = LayerMask.GetMask(LAYERGROUND);
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._jumpForce = 25.0f;
        this._runningSpeed = 6.0f;
        this._startPosition = this.transform.position;
        this._gameStateChannel.OnChangeGameState += OnChangeGameState;
    }
    
    private void InitPlayer()
    {
        this._killPlayerChannel.OnDead += Die;
        this._animator.SetBool(ISALIVE, true);
        this.transform.position = _startPosition;
        InitDistanceTraveled();
    }
    
    private void Update()
    {
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            _animator.SetBool(ISGROUNDED, IsOnTheFloor());
            if (Input.GetButtonDown(BUTTONFIRE1) && IsOnTheFloor())
            {
                Jump();
            } 
        }
    
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            if (_rigidbody2D.velocity.x < _runningSpeed)
            {
                _rigidbody2D.velocity = new Vector2(_runningSpeed, _rigidbody2D.velocity.y);
            }
        }
    }

    private void OnDestroy()
    {
        _gameStateChannel.OnChangeGameState -= OnChangeGameState;
        _killPlayerChannel.OnDead -= Die;
    }

    private void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private bool IsOnTheFloor()
    {
        bool isOnTheFloor = false;
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.0f, _groundLayerMask.value))
        {
            isOnTheFloor = true;
        }

        return isOnTheFloor;
    }

    private void InitDistanceTraveled()
    {
        this._distanceTraveled = 0;
    }

    public float GetDistanceTravelled()
    {
        this._distanceTraveled =
            Vector2.Distance(new Vector2(this._startPosition.x, 0), new Vector2(this.transform.position.x, 0));
        return _distanceTraveled;
    }

    private void Die()
    {
        _animator.SetBool(ISALIVE, false);
        Invoke("SleepPlayer",1f);
        _killPlayerChannel.OnDead -= Die;
        GameManager.Instance.GameOver();
    }

    private void SleepPlayer()
    {
        GetComponent<Rigidbody2D>().Sleep();
    }

    private void OnChangeGameState(EGameState newGameState)
   {
       switch (newGameState)
       {
           case EGameState.InTheGame:
               InitPlayer();
               break;
           case EGameState.Menu:
               break;
           case EGameState.GameOver:
               GameManager.Instance.SetFinalGameScore(_distanceTraveled);
               break;
           default:
               break;
       }
       
   }
}