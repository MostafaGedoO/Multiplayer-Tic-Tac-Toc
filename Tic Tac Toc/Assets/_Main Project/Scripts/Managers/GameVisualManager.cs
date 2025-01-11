using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject xPrefab;
    [SerializeField] private NetworkObject oPrefab;
    [SerializeField] private NetworkObject winLinePrefab;

    private readonly List<GameObject> gridVisuals = new List<GameObject>(); 
    
    private const float GRID_SIZE = 3.1f;

    private void Start()
    {
        GameManager.Instance.OnGridItemClicked += GameManagerOnGridItemClicked;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        GameManager.Instance.OnRematch += GameManager_OnRematch;
    }

    private void GameManager_OnRematch()
    {
        if(!IsServer) return;
        
        foreach (var _visual in gridVisuals)
        {
            Destroy(_visual);
        }
        gridVisuals.Clear();
    }

    private void GameManager_OnGameWin(Vector2 _pos,float _zEuler,PlayerType _playerType)
    {
        if(!IsServer) return;
        
        if(_playerType == PlayerType.None) return;
        NetworkObject _networkObject = Instantiate(winLinePrefab, GetWorldGridPosition(_pos), Quaternion.Euler(0,0,_zEuler));
        _networkObject.Spawn();
        gridVisuals.Add(_networkObject.gameObject);
    }

    private void GameManagerOnGridItemClicked(Vector2 _gridPos , PlayerType _playerType)
    {
        SpawnObjectOnGridRpc(_gridPos , _playerType);
    }


    [Rpc(SendTo.Server)]
    private void SpawnObjectOnGridRpc(Vector2 _gridPos , PlayerType _playerType)
    {
        if (_playerType == PlayerType.X)
        {
            NetworkObject _networkObject = Instantiate(xPrefab, GetWorldGridPosition(_gridPos), Quaternion.identity);
            _networkObject.Spawn();
            gridVisuals.Add(_networkObject.gameObject);
        }
        else
        {
            NetworkObject _networkObject = Instantiate(oPrefab, GetWorldGridPosition(_gridPos), Quaternion.identity);
            _networkObject.Spawn();
            gridVisuals.Add(_networkObject.gameObject);
        }
    }

    private Vector2 GetWorldGridPosition(Vector2 _gridPos)
    {
        return new Vector2(-GRID_SIZE + _gridPos.y * GRID_SIZE, -GRID_SIZE + _gridPos.x * GRID_SIZE);
    }
}
