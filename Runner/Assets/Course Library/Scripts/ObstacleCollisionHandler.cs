using UnityEngine;

public class ObstacleCollisionHandler : MonoBehaviour, ICollisionHandler
{
    public void HandleCollision(PlayerController player)
    {
        Debug.Log("Game Over");
        Debug.Log("Obstacle Handled");
        player.GameOver();

    }
}
