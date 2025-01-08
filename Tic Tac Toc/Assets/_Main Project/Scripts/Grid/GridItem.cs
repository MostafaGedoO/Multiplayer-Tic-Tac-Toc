using UnityEngine;

public class GridItem : MonoBehaviour
{
    [SerializeField] private Vector2 gridPosition;

    private void OnMouseDown()
    {
        GameManager.Instance.GridItemClicked(gridPosition);
    }
}
