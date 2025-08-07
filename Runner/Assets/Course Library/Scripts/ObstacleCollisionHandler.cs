#define TEST

using UnityEngine;

public class ObstacleCollisionHandler : MonoBehaviour, ICollisionHandler
{
    public void HandleCollision(PlayerController player)
    {
#if TEST
        Debug.Log($"GameManager Gameover Call Started from {this}");
#endif 
        GameManager.Instance.GameOver();

    }
}
