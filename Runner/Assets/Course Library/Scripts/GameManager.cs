//#define TEST

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event System.Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();

#if TEST
        Debug.Log("GameManager Gameover Called");
#endif
    }
}