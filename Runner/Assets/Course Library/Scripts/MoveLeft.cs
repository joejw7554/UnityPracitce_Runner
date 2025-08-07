using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;

    private void Start()
    {
        PlayerController playerControllerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        GameManager.Instance.OnGameOver += StopMoving;
    }

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    void StopMoving()
    {
        enabled = false;
    }
}



