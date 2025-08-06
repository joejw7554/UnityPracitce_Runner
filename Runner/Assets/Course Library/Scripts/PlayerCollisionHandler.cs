using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var handler = collision.gameObject.GetComponent<ICollisionHandler>();
        var player = GetComponent<PlayerController>();

        if (handler != null && player != null)
        {
            handler.HandleCollision(player);
        }
    }
}