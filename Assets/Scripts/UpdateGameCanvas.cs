using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateGameCanvas : MonoBehaviour
{
   private const string HIGHSCORE = "highscore";
   [SerializeField] 
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
       this._itemCollectedChannel.OnItemCollected += OnItemCollected;
       this._gameStateChannel.OnChangeGameState += OnChangeGameState;
   }

   private void Update()
   {
       if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
       {
           this._scorePoints.text = _playerController.GetDistanceTravelled().ToString("f0");
       }
   }

   private void OnDestroy()
   {
       this._itemCollectedChannel.OnItemCollected -= OnItemCollected;
       this._gameStateChannel.OnChangeGameState -= OnChangeGameState;
   }

   private void OnItemCollected(EItemType itemType)
   {
       if (itemType.Equals(EItemType.Coin))
       {
           this._coinsNumber.text = (GameManager.Instance.GetCollectedCoins() + 1).ToString();
       }
   }

   private void SetRecordPointsText()
   {
       this._recordPoints.text = PlayerPrefs.GetFloat(HIGHSCORE, 0).ToString("f0");
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
               SetRecordPointsText();
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
