using UnityEngine;

public class GridItem : MonoBehaviour
{
    [SerializeField] private Vector2 gridPosition;

    private void OnMouseDown()
    {
        GameManager.Instance.GridItemClickedRpc(gridPosition,GameManager.Instance.GetLocalPlayerType());
    }
}
