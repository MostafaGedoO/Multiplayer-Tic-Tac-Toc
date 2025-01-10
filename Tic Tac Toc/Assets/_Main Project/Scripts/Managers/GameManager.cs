using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerType playerType;
    private PlayerType currentPlayerType;
    public event Action<Vector2,PlayerType> OnGridItemClikced;

    public event Action OnGameStarted;
    public event Action<PlayerType> OnTurnChanged;
    
    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if(IsHost)
        {
            playerType = PlayerType.X;
        }
        else
        {
            playerType = PlayerType.O;
        }

        if(IsServer)
        {
            currentPlayerType = PlayerType.X;
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong _ClientId)
    {
        if (NetworkManager.ConnectedClients.Count == 2)
        {
            FireOnGameStartedRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FireOnGameStartedRpc()
    {
        OnGameStarted?.Invoke();
    }

    [Rpc(SendTo.Server)]
    public void GridItemClickedRpc(Vector2 _gridPos,PlayerType _playerType)
    {
        if (_playerType != currentPlayerType) return;

        OnGridItemClikced?.Invoke(_gridPos, _playerType);

        if(currentPlayerType == PlayerType.X) 
        {
            currentPlayerType = PlayerType.O;
        }
        else if(currentPlayerType == PlayerType.O)
        {
            currentPlayerType = PlayerType.X;
        }
        
        FireOnTurnChangedRpc(currentPlayerType);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FireOnTurnChangedRpc(PlayerType _playerType)
    {
        OnTurnChanged?.Invoke(_playerType);
    }
    
    public PlayerType GetLocalPlayerType()
    {
        return playerType;
    }
}


public enum PlayerType { None,X,O }