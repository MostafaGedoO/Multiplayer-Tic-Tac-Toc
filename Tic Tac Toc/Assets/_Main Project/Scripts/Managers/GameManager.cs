using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerType playerType;
    private PlayerType currentPlayerType;
    private PlayerType winningPlayerType;

    public event Action<Vector2,PlayerType> OnGridItemClicked;
    public event Action OnGameStarted;
    public event Action<Vector2,float,PlayerType> OnGameWin;
    public event Action<PlayerType> OnTurnChanged;
    public event Action OnRematch;
    
    private PlayerType[,] grid;
    
    private void Awake()
    {
        Instance = this;
        grid = new PlayerType[3, 3];
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

        if(grid[(int)_gridPos.x, (int)_gridPos.y] != PlayerType.None) return;
        
        grid[(int)_gridPos.x, (int)_gridPos.y] = _playerType;
        
        OnGridItemClicked?.Invoke(_gridPos, _playerType);

        if(currentPlayerType == PlayerType.X) 
        {
            currentPlayerType = PlayerType.O;
        }
        else if(currentPlayerType == PlayerType.O)
        {
            currentPlayerType = PlayerType.X;
        }

        TestWinner();
        
        FireOnTurnChangedRpc(currentPlayerType);
    }

    private void TestWinner()
    {
        //Horizontal
        for (int i = 0; i < 3; i++)
        {
            if (TestLine(grid[i, 0], grid[i, 1], grid[i, 2]))
            {
                currentPlayerType = PlayerType.None;
                FireOnGameWindRpc(new Vector2(i, 1), 0f, grid[i, 0]);
                return;
            }
        }
        
        //Vertical
        for (int i = 0; i < 3; i++)
        {
            if (TestLine(grid[0, i], grid[1, i], grid[2, i]))
            {
                currentPlayerType = PlayerType.None;
                FireOnGameWindRpc(new Vector2(1, i), 90f, grid[0, i]);
                return;
            }
        }
        
        //Diagonal
        if (TestLine(grid[0, 0], grid[1, 1], grid[2, 2]))
        {
            currentPlayerType = PlayerType.None;
            FireOnGameWindRpc(new Vector2(1, 1), 45f, grid[0, 0]);
            return;
        }  
        
        if (TestLine(grid[2, 0], grid[1, 1], grid[0, 2]))
        {
            currentPlayerType = PlayerType.None;
            FireOnGameWindRpc(new Vector2(1, 1), -45f, grid[1, 1]);
            return;
        }
        
        //Tie
        foreach (var _gridItem in grid)
        {
            //Check if there is an empty grid item
            if(_gridItem == PlayerType.None) return;
        }
        
        currentPlayerType = PlayerType.None;
        FireOnGameWindRpc(new Vector2(-1, -1), -1f, PlayerType.None);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FireOnGameWindRpc(Vector2 _gridPos, float _zEuler, PlayerType _playerType)
    {
        winningPlayerType = _playerType;
        OnGameWin?.Invoke(_gridPos, _zEuler, _playerType);
    }
    
    private bool TestLine(PlayerType _firstPlayerType, PlayerType _secondPlayerType, PlayerType _thirdPlayerType)
    {
        return  _firstPlayerType != PlayerType.None && _firstPlayerType == _secondPlayerType && _firstPlayerType == _thirdPlayerType;
    }

    [Rpc(SendTo.Server)]
    public void RematchRpc()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                grid[i, j] = PlayerType.None;
            }
        }

        if (winningPlayerType != PlayerType.None)
        {
            currentPlayerType = winningPlayerType;
            FireOnTurnChangedRpc(currentPlayerType);
        }
        else
        {
            int _randomStart = UnityEngine.Random.Range(0, 2);
            if (_randomStart == 0)
            {
                currentPlayerType = PlayerType.X;
            }
            else
            {
                currentPlayerType = PlayerType.O;
            }
            FireOnTurnChangedRpc(currentPlayerType);
        }
        
        FireOnRematchRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FireOnRematchRpc()
    {
        OnRematch?.Invoke();
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