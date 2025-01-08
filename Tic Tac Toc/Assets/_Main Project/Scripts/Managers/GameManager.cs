using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void GridItemClicked(Vector2 _gridPos)
    {
        Debug.Log("Grid " + _gridPos + " Clicked");
    }
}
