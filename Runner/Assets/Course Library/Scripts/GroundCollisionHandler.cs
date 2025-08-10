using UnityEngine;

public class GroundCollisionHandler : MonoBehaviour, ICollisionHandler
{
    public void HandleCollision(PlayerController player)
    {
        player.isOnGround = true;

        if (player.isOnGround)
        {
            player.PlayParticle(ParticleType.Dirt);
        }
#if TEST
        Debug.Log("GroundCollisionHandler Handled");
#endif
    }
}
