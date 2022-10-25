using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private LayerMask _groundLayerMask;
    private float _jumpForce;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private float _runningSpeed;
    private Vector3 _startPosition;
    [SerializeField]
    private KillPlayerChannel killPlayerChannel;
    [SerializeField]
    private GameStateChannel gameStateChannel;

    private void Awake()
    {
        this._groundLayerMask = LayerMask.GetMask("Ground");
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._jumpForce = 25.0f;
        this._runningSpeed = 6.0f;
        this._animator.SetBool("isAlive", true);
        this._startPosition = this.transform.position;
        this.killPlayerChannel.OnDead += Die;
    }
    
    void InitPlayer()
    {
        _animator.SetBool("isAlive", true);
        this.transform.position = _startPosition;
        this.gameStateChannel.OnChangeGameState += OnChangeGameState;
    }
    
    private void Update()
    {
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            _animator.SetBool("isGrounded", IsOnTheFloor());
            if (Input.GetButtonDown("Fire1") && IsOnTheFloor())
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
        gameStateChannel.OnChangeGameState -= OnChangeGameState;
        killPlayerChannel.OnDead -= Die;
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

    private void Die()
    {
        _animator.SetBool("isAlive", false);
        killPlayerChannel.OnDead -= Die;
        GameManager.Instance.GameOver();
    }

    private void OnChangeGameState(EGameState newGameState)
   {
       switch (newGameState)
       {
           case EGameState.InTheGame:
               InitPlayer();
               break;
           case EGameState.Menu:
               Debug.Log("BunnyOnMenu");
               break;
           case EGameState.GameOver:
               Debug.Log("BunnyOnGameOver");
               break;
           default:
               Debug.Log("BunnyOnDefault");
               break;
       }
       
   }
}