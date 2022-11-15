using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants
    
    private const string LayerGround = "Ground";
    private const string IsAlive = "isAlive";
    private const string IsGrounded = "isGrounded";
    private const string IsInvulnerable = "isInvulnerable";
    private const string HurtTrigger = "Hurt";

    #endregion
    
    #region Attributes

    private int _maxHealth;
    private float _jumpForce;
    private float _runningSpeed;
    private float _distanceTraveled;
    private int _healthPlayer;
    private bool _invulnerability;
    private bool _canMove;
    private bool _isInDanger;

    private bool _canDash;
    private bool _isDashing;
    private float _dashingPower;
    private float _dashingTime;

    private float _minimumSwipeLength;
    private Touch _touch;
    private Vector2 _touchInitalPosition, _touchEndPosition;
    
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private LayerMask _groundLayerMask;
    private Vector3 _startPosition;
    private Vector2 _reboundVelocity;
    private AudioSource _audioSource;
    private TrailRenderer _tr;
    
    #endregion
    
    #region Inspector
    
    public static PlayerController Instance;
    
    [SerializeField] 
    private GameObject playerGameObject;
    [SerializeField]
    private KillPlayerChannel killPlayerChannel;
    [SerializeField]
    private GameStateChannel gameStateChannel;
    [SerializeField]
    private ItemCollectedChannel itemCollectedChannel;
    [SerializeField] 
    private DamagePlayerTriggerChannel damagePlayerTriggerChannel;
    [SerializeField] 
    private HealthPlayerChannel healthPlayerChannel;
    
    #endregion

    #region EventFunctions
    private void Awake()
    {
        //Inicializamos Singleton, atributos y suscripciones a Actions de los channels
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        _groundLayerMask = LayerMask.GetMask(LayerGround);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _tr = GetComponent<TrailRenderer>();
        
        _jumpForce = 25.0f;
        _runningSpeed = 6.0f;
        _maxHealth = 6;
        _dashingPower = 15f;
        _dashingTime = 0.2f;
        _reboundVelocity = new Vector2(4f, 5f);

        _minimumSwipeLength = 40f;

        _startPosition = transform.position;
        gameStateChannel.OnChangeGameState += OnChangeGameState;
    }

    private void Update()
    {
        //Bloqueamos acciones del jugador mientras el dash se ejecuta
        if (_isDashing)
        {
            return;
        }
        
        //Controlamos que el estado es InGame
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            _animator.SetBool(IsGrounded, IsOnTheFloor());
            //reiniciamos el dash la capacidad de dash al tocar el suelo
            if (IsOnTheFloor())
            {
                SetCanDash(true);
            }
            
            
            // Controlamos los Input para saltar (y si está en el suelo) en base a plataforma (táctil o no táctil)        
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER        
            if (Input.GetButtonDown("Jump") && IsOnTheFloor())
#else
            if (IsTouched() && IsOnTheFloor())
#endif
            {
                Jump();
            } 
            
            // Controlamos los Input para dash (y si está habilitado) en base a plataforma (táctil o no táctil)
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER        
            if (Input.GetButtonDown("Fire2") && _canDash)
#else
            if (IsSwipedRight() && _canDash)
#endif
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void FixedUpdate()
    {
        //Controlamos si el estado es InGame y si el jugador puede seguir adelante
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame) && _canMove)
        {
            if (_rigidbody2D.velocity.x < _runningSpeed)
            {
                _rigidbody2D.velocity = new Vector2(_runningSpeed, _rigidbody2D.velocity.y);
            }
        }
    }

    private void OnDestroy()
    {
        //Nos desuscribimos de los Action
        gameStateChannel.OnChangeGameState -= OnChangeGameState;
        killPlayerChannel.OnDead -= OnDie;
        itemCollectedChannel.OnItemCollected -= OnItemCollected;
        damagePlayerTriggerChannel.OnDamagePlayer -= OnDamagePlayer;
    }
    #endregion

    #region MovementFunctions
    private void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _audioSource.Play();
    }
    
    private void Rebound()
    {
        _rigidbody2D.velocity = new Vector2(-_reboundVelocity.x, _reboundVelocity.y);
    }

    private void SetCanMove(bool value)
    {
        _canMove = value;
    }
    
    private void SetCanDash(bool value)
    {
        _canDash = value;
    }
    #endregion

    #region CheckFunctions
    private bool IsOnTheFloor()
    {
        bool isOnTheFloor = false;
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.0f, _groundLayerMask.value))
        {
            isOnTheFloor = true;
        }

        return isOnTheFloor;
    }
    
    private bool CheckInDanger()
    {
        bool danger =  _healthPlayer < 3;

        return danger;
    }
    #endregion

    #region DistanceFunctions
    private void InitDistanceTraveled()
    {
        _distanceTraveled = 0;
    }

    public float GetDistanceTravelled()
    {
        _distanceTraveled =
            Vector2.Distance(new Vector2(_startPosition.x, 0), new Vector2(transform.position.x, 0));
        return _distanceTraveled;
    }
    #endregion

    #region HealthFunctions

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetHealthPlayer()
    {
        return _healthPlayer;
    }
    private void CollectHealth(int value)
    {
        if (_healthPlayer < _maxHealth)
        {
            if (_healthPlayer + value < _maxHealth)
            {
                _healthPlayer += value;
            }
            else
            {
                _healthPlayer = _maxHealth;
            }

            if (!CheckInDanger())
            {
                healthPlayerChannel.InvokeOnHighHealth();
            }

            healthPlayerChannel.InvokeOnHealthIncrease(value);
        }
    }

    private void SubtractHealth()
    {
        if (_healthPlayer > 0)
        {
            _healthPlayer--;
            healthPlayerChannel.InvokeOnHealthDecrease();
        }
        
        if(CheckInDanger())
        {
            healthPlayerChannel.InvokeOnLowHealth();            
        }else
        {
            healthPlayerChannel.InvokeOnHighHealth();
        }
    }
    
    private void SetInvulnerability(bool value)
    {
        _invulnerability = value;
    }
    #endregion

    #region ChannelFunctions
    private void OnDie()
    {
        _animator.SetBool(IsAlive, false);
        Invoke(nameof(DelayGameOver),0.5f);
        killPlayerChannel.OnDead -= OnDie;
        itemCollectedChannel.OnItemCollected -= OnItemCollected;
        damagePlayerTriggerChannel.OnDamagePlayer -= OnDamagePlayer;
    }

    private void OnChangeGameState(EGameState newGameState)
   {
       switch (newGameState)
       {
           case EGameState.InTheGame:
               EnablePlayer();
               InitPlayer();
               break;
           case EGameState.Menu:
               DisablePlayer();
               break;
           case EGameState.GameOver:
               DisablePlayer();
               GameManager.Instance.SetFinalGameScore(_distanceTraveled);
               break;
       }
   }

    private void OnItemCollected(EItemType itemType, int value)
    {
        switch (itemType)
        {
            case EItemType.Money:
                break;
            case EItemType.Health:
                CollectHealth(value);
                break;
        }
    }

    private void OnDamagePlayer()
    {
        if (!_invulnerability)
        {
            _animator.SetTrigger(HurtTrigger);
            _animator.SetBool(IsInvulnerable, true);
            SubtractHealth();
            Rebound();
            if (_healthPlayer.Equals(0))
            {
                OnDie();
            }
            else
            {
                SetInvulnerability(true);
                DisableEnemyColision();
                StartCoroutine(InmobilizePlayer());
            }
        }
        Invoke(nameof(DelayInvulnerability), 2f);
    }
    #endregion

    #region Delays
    private void DelayInvulnerability()
    {
        SetInvulnerability(false);
        _animator.SetBool(IsInvulnerable, false);
        EnableEnemyColision();
    }
    
    private void DelayGameOver()
    {
        GetComponent<Rigidbody2D>().Sleep();
        GameManager.Instance.GameOver();
    }
    #endregion

    #region Coroutines

    private IEnumerator TirePlayer()
    {
        while (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            yield return new WaitForSeconds(10f);
            if (_healthPlayer > 1)
            {
                SubtractHealth();
            }
            else
            {
                SubtractHealth();
                Rebound();
                OnDie();
            }
        }
    }

    private IEnumerator InmobilizePlayer()
    {
        SetCanMove(false);
        yield return new WaitForSeconds(0.5f);
        SetCanMove(true);
    }

    private IEnumerator BlinkPlayer()
    {
        while (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            yield return new WaitForSeconds(0.25f);
            if (_invulnerability)
            {
                _spriteRenderer.renderingLayerMask = 0;
                yield return new WaitForSeconds(0.125f);
                _spriteRenderer.renderingLayerMask = 1;
            }
        }
    }

    private IEnumerator Dash()
    {
        SetCanDash(false);
        _isDashing = true;
        float originalGravity = _rigidbody2D.gravityScale;
        float originalVelocityX = transform.localScale.x;
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.velocity = new Vector2(transform.localScale.x * _dashingPower, 0f);
        _tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _rigidbody2D.gravityScale = originalGravity;
        _rigidbody2D.velocity = new Vector2(originalVelocityX, 0f);
        _isDashing = false;
    }
    
    #endregion

    #region GameObjectFunctions
    private void DisablePlayer()
    {
        playerGameObject.SetActive(false);
    }
    
    private void EnablePlayer()
    {
        playerGameObject.SetActive(true);
    }
    #endregion

    #region EnemyCollisionFunctions

    private void DisableEnemyColision()
    {
        Physics2D.IgnoreLayerCollision(3, 8, true);
    }
    
    private void EnableEnemyColision()
    {
        Physics2D.IgnoreLayerCollision(3, 8, false);
    }
    
    #endregion

    #region TactileInputs

    private bool IsSwipedRight()
    {
        float XLength;
        bool isSwipedRight = false;
        if (Input.touchCount > 0)
        {
            //En caso afirmativo, referenciamos el primer toque (Es decir el index 0)
            _touch = Input.GetTouch(0);
            //Preguntamos si el toque está en su fase de inicio
            if (_touch.phase == TouchPhase.Began) {
                //En caso afirmativo, capturamos su posición inicial
                _touchInitalPosition = _touch.position;
            }
            //Preguntamos si el toque está en su fase final
            if (_touch.phase == TouchPhase.Ended)
            {
                //En caso afirmativo, capturamos su posición final
                _touchEndPosition = _touch.position;
                //Calculamos la longitud del desplazamiento en X
                XLength = _touchEndPosition.x - _touchInitalPosition.x;
                //Checkeamos que la longitud es superior a la establecida para su detección
                if (XLength > _minimumSwipeLength)
                {
                    //Detectamos la dirección del SWIPE a partir de X
                    if (_touchInitalPosition.x < _touchEndPosition.x)
                    {
                        isSwipedRight = true;
                    }
                }
            }
        }
        return isSwipedRight;
    }
    
    private bool IsTouched()
    {
        float XLength;
        bool isTouched = false;
        if (Input.touchCount > 0)
        {
            //En caso afirmativo, referenciamos el primer toque (Es decir el index 0)
            _touch = Input.GetTouch(0);
            //Preguntamos si el toque está en su fase de inicio
            if (_touch.phase == TouchPhase.Began) {
                //En caso afirmativo, capturamos su posición inicial
                _touchInitalPosition = _touch.position;
            }
            //Preguntamos si el toque está en su fase final
            if (_touch.phase == TouchPhase.Ended)
            {
                //En caso afirmativo, capturamos su posición final
                _touchEndPosition = _touch.position;
                //Calculamos la longitud de X
                XLength = _touchEndPosition.x - _touchInitalPosition.x;
                //Detectamos si la longitud del SWIPE es inferior a la establecida en X
                if (XLength < _minimumSwipeLength) {
                    isTouched =  true;
                }
            }
        }
        return isTouched;
    }

    #endregion

    private void InitPlayer()
    {
        killPlayerChannel.OnDead += OnDie;
        itemCollectedChannel.OnItemCollected += OnItemCollected;
        damagePlayerTriggerChannel.OnDamagePlayer += OnDamagePlayer;
        _healthPlayer = _maxHealth;
        healthPlayerChannel.InvokeOnHealthIncrease(_maxHealth);
        _animator.SetBool(IsAlive, true);
        transform.position = _startPosition;
        _spriteRenderer.renderingLayerMask = 1;
        InitDistanceTraveled();
        SetCanMove(true);
        SetCanDash(true);
        StartCoroutine(TirePlayer());
        StartCoroutine(BlinkPlayer());
    }
}