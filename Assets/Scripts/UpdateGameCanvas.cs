using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class UpdateGameCanvas : MonoBehaviour
{
   private const string HIGHSCORE = "highscore";
   private PlayerController _playerController;
   [SerializeField] 
   private TextMeshProUGUI _coinsNumber;
   [SerializeField] 
   private TextMeshProUGUI _scorePoints;
   [SerializeField] 
   private TextMeshProUGUI _recordPoints;
   [SerializeField]
   private ItemCollectedChannel _itemCollectedChannel;
   [SerializeField]
   private GameStateChannel _gameStateChannel;

   private void Start()
   {
       _playerController = PlayerController.Instance;
       _itemCollectedChannel.OnItemCollected += OnItemCollected;
       _gameStateChannel.OnChangeGameState += OnChangeGameState;
   }

   private void Update()
   {
       if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
       {
           _scorePoints.text = _playerController.GetDistanceTravelled().ToString("f0");
       }
   }

   private void OnDestroy()
   {
       _itemCollectedChannel.OnItemCollected -= OnItemCollected;
       _gameStateChannel.OnChangeGameState -= OnChangeGameState;
   }

   private void OnItemCollected(EItemType itemType, int value)
   {
       if (itemType.Equals(EItemType.Money))
       {
           _coinsNumber.text = (GameManager.Instance.GetCollectedCoins() + value).ToString();
       }
   }

   private void SetRecordPointsText()
   {
       _recordPoints.text = PlayerPrefs.GetFloat(HIGHSCORE, 0).ToString("f0");
   }
   
   private void SetCoinsNumberText()
   {
       _coinsNumber.text = 0.ToString();
   }

   private void InitGameCanvas()
   {
       SetCoinsNumberText();
       SetRecordPointsText();
   }
   
   private bool CheckHighScore()
   {
       return PlayerPrefs.GetFloat(HIGHSCORE, 0) < _playerController.GetDistanceTravelled();
   }

   private void UpdateHighScore()
   {
       if (CheckHighScore())
       {
           PlayerPrefs.SetFloat(HIGHSCORE, _playerController.GetDistanceTravelled());
       }
   }

   private void OnChangeGameState(EGameState newGameState)
   {
       switch (newGameState)
       {
           case EGameState.InTheGame:
               InitGameCanvas();
               break;
           case EGameState.Menu:
               break;
           case EGameState.GameOver:
               UpdateHighScore();
               break;
           default:
               break;
       }
   }
}
