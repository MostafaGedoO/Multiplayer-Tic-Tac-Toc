using System;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private GameObject xYouText;
    [SerializeField] private GameObject oYouText;
    [SerializeField] private GameObject xArrow;
    [SerializeField] private GameObject oArrow;

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnOnGameStarted;
        GameManager.Instance.OnTurnChanged += GameManager_OnTurnChanged;
    }

    private void GameManager_OnOnGameStarted()
    {
        if (GameManager.Instance.GetLocalPlayerType() == PlayerType.X)
        {
            xYouText.SetActive(true);
        }
        else
        {
            oYouText.SetActive(true);
        }
        
        GameManager_OnTurnChanged(PlayerType.X);
    }
    
    private void GameManager_OnTurnChanged(PlayerType _playerType)
    {
        if (_playerType == PlayerType.X)
        {
            xArrow.SetActive(true);
            oArrow.SetActive(false);
        }
        else
        {
            xArrow.SetActive(false);
            oArrow.SetActive(true);
        }
    }
    
}
