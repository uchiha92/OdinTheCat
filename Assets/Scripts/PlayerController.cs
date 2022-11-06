using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants
    
    private const string LayerGround = "Ground";
    private const string IsAlive = "isAlive";
    private const string IsGrounded = "isGrounded";
    private const string IsHurt = "isHurt";
    private const string ButtonFire1 = "Fire1";

    #endregion
    
    #region Attributes

    private int _maxHealth;
    private float _jumpForce;
    private float _runningSpeed;
    private float _distanceTraveled;
    private int _healthPlayer;
    private bool _invulnerability;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private LayerMask _groundLayerMask;
    private Vector3 _startPosition;
    private AudioSource _audioSource;
    
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
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        this._groundLayerMask = LayerMask.GetMask(LayerGround);
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._jumpForce = 25.0f;
        this._runningSpeed = 6.0f;
        this._maxHealth = 6;
        this._startPosition = this.transform.position;
        this.gameStateChannel.OnChangeGameState += OnChangeGameState;
        this._audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            this._animator.SetBool(IsGrounded, IsOnTheFloor());
            if (Input.GetButtonDown(ButtonFire1) && IsOnTheFloor())
            {
                Jump();
            } 
        }
    
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            if (this._rigidbody2D.velocity.x < this._runningSpeed)
            {
                this._rigidbody2D.velocity = new Vector2(this._runningSpeed, this._rigidbody2D.velocity.y);
            }
        }
    }

    private void OnDestroy()
    {
        this.gameStateChannel.OnChangeGameState -= OnChangeGameState;
        this.killPlayerChannel.OnDead -= OnDie;
        this.itemCollectedChannel.OnItemCollected -= OnItemCollected;
        this.damagePlayerTriggerChannel.OnDamagePlayer -= OnDamagePlayer;
    }
    #endregion

    #region MovementFunctions
    private void Jump()
    {
        this._rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        this._audioSource.Play();
    }
    #endregion

    #region CheckFunctions
    private bool IsOnTheFloor()
    {
        bool isOnTheFloor = false;
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.0f, _groundLayerMask.value))
        {
            isOnTheFloor = true;
        }

        return isOnTheFloor;
    }
    #endregion

    #region DistanceFunctions
    private void InitDistanceTraveled()
    {
        this._distanceTraveled = 0;
    }

    public float GetDistanceTravelled()
    {
        this._distanceTraveled =
            Vector2.Distance(new Vector2(this._startPosition.x, 0), new Vector2(this.transform.position.x, 0));
        return this._distanceTraveled;
    }
    #endregion

    #region HealthFunctions

    public int GetMaxHealth()
    {
        return this._maxHealth;
    }

    public int GetHealthPlayer()
    {
        return this._healthPlayer;
    }
    private void CollectHealth(int value)
    {
        if (this._healthPlayer < this._maxHealth)
        {
            if (this._healthPlayer + value < this._maxHealth)
            {
                this._healthPlayer += value;
            }
            else
            {
                this._healthPlayer = this._maxHealth;
            }

            this.healthPlayerChannel.InvokeOnHealthIncrease(value);
        }
    }

    private void SubtractHealth()
    {
        if (this._healthPlayer > 0)
        {
            this._healthPlayer--;
            SetInvulnerability(true);
            this._animator.SetBool(IsHurt, true);
            this.healthPlayerChannel.InvokeOnHealthDecrease();
        }
    }
    
    private void SetInvulnerability(bool value)
    {
        this._invulnerability = value;
    }
    #endregion

    #region ChannelFunctions
    private void OnDie()
    {
        this._animator.SetBool(IsAlive, false);
        Invoke(nameof(DelayGameOver),1f);
        this.killPlayerChannel.OnDead -= OnDie;
        this.itemCollectedChannel.OnItemCollected -= OnItemCollected;
        this.damagePlayerTriggerChannel.OnDamagePlayer -= OnDamagePlayer;
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
        if (!this._invulnerability)
        {
            SubtractHealth();
        }
        Invoke(nameof(DelayInvulnerability), 1.5f);
    }
    #endregion

    #region Delays
    private void DelayInvulnerability()
    {
        SetInvulnerability(false);
        this._animator.SetBool(IsHurt, false);
    }
    
    private void DelayGameOver()
    {
        GetComponent<Rigidbody2D>().Sleep();
        GameManager.Instance.GameOver();
    }
    #endregion

    #region Coroutines
    IEnumerator TirePlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            if (this._healthPlayer > 0)
            {
                this._healthPlayer--;
                this.healthPlayerChannel.InvokeOnHealthDecrease();
            }
        }
    }
    #endregion

    #region GameObjectFunctions
    private void DisablePlayer()
    {
        this.playerGameObject.SetActive(false);
    }
    
    private void EnablePlayer()
    {
        this.playerGameObject.SetActive(true);
    }
    #endregion
    
   
    private void InitPlayer()
    {
        this.killPlayerChannel.OnDead += OnDie;
        this.itemCollectedChannel.OnItemCollected += OnItemCollected;
        this.damagePlayerTriggerChannel.OnDamagePlayer += OnDamagePlayer;
        this._healthPlayer = this._maxHealth;
        this.healthPlayerChannel.InvokeOnHealthIncrease(this._maxHealth);
        this._animator.SetBool(IsAlive, true);
        this.transform.position = this._startPosition;
        InitDistanceTraveled();
        StartCoroutine(TirePlayer());
    }
}