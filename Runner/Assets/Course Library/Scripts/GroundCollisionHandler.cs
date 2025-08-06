using UnityEngine;

public class GroundCollisionHandler : MonoBehaviour, ICollisionHandler
{
    public void HandleCollision(PlayerController player)
    {
        player.isOnGround = true;

#if TEST
        Debug.Log("GroundCollisionHandler Handled");
#endif
    }
}
