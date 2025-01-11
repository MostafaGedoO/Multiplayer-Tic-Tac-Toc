using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject xPrefab;
    [SerializeField] private NetworkObject oPrefab;
    [SerializeField] private NetworkObject winLinePrefab;

    private const float GRID_SIZE = 3.1f;

    private void Start()
    {
        GameManager.Instance.OnGridItemClikced += GameManager_OnGridItemClikced;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
    }

    private void GameManager_OnGameWin(Vector2 _pos,float _zEuler)
    {
        NetworkObject _networkObject = Instantiate(winLinePrefab, GetWorldGridPosition(_pos), Quaternion.Euler(0,0,_zEuler));
        _networkObject.Spawn();
    }

    private void GameManager_OnGridItemClikced(Vector2 _gridPos , PlayerType _playerType)
    {
        SpwanObjectOnGridRpc(_gridPos , _playerType);
    }


    [Rpc(SendTo.Server)]
    private void SpwanObjectOnGridRpc(Vector2 _gridPos , PlayerType _playerType)
    {
        if (_playerType == PlayerType.X)
        {
            NetworkObject _networkObject = Instantiate(xPrefab, GetWorldGridPosition(_gridPos), Quaternion.identity);
            _networkObject.Spawn();
        }
        else
        {
            NetworkObject _networkObject = Instantiate(oPrefab, GetWorldGridPosition(_gridPos), Quaternion.identity);
            _networkObject.Spawn();
        }
    }

    private Vector2 GetWorldGridPosition(Vector2 _gridPos)
    {
        return new Vector2(-GRID_SIZE + _gridPos.y * GRID_SIZE, -GRID_SIZE + _gridPos.x * GRID_SIZE);
    }
}
