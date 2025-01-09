using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerType playerType;
    private PlayerType currentPlayerType;
    public event Action<Vector2,PlayerType> OnGridItemClikced;

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
        }
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
    }

    public PlayerType GetLocalPlayerType()
    {
        return playerType;
    }
}


public enum PlayerType { None,X,O }