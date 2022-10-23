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
    private KillTrigger _killTrigger;
    
    //[SerializeField]
    //private KillTriggerChannel m_KillTrigerChannel;

    private void Awake()
    {
        this._groundLayerMask = LayerMask.GetMask("Ground");
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._jumpForce = 25.0f;
        this._runningSpeed = 6.0f;
        this._animator.SetBool("isAlive", true);
        this._startPosition = this.transform.position;
        this._killTrigger = GameObject.FindGameObjectWithTag("KillTrigger").GetComponent<KillTrigger>();
        //this.m_KillTrigerChannel.OnDied += Die;
        GameManager.Instance.OnReset += Reset;
    }
    
    void InitPlayer()
    {
        _animator.SetBool("isAlive", true);
        _killTrigger.OnDead += Die;
        this.transform.position = _startPosition;
    }
    
    /*void InitPlayer()
    {
        _animator.SetBool("isAlive", true);
        m_KillTrigerChannel.OnDied += Die;
        this.transform.position = _startPosition;
    }*/

    // Update is called once per frame
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
        GameManager.Instance.OnReset -= Reset;
        _killTrigger.OnDead -= Die;
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
        _killTrigger.OnDead -= Die;
        Debug.Log("conejito muerto");
        GameManager.Instance.GameOver();
    }
    
   /* private void Die(KillTrigger killTrigger)
    {
        _animator.SetBool("isAlive", false);
        m_KillTrigerChannel.OnDied -= Die;
        GameManager.Instance.GameOver();
    }*/

   private void Reset()
   {
       InitPlayer();
   }
}