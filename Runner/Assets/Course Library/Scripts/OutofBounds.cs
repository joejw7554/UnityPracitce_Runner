using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    [SerializeField] float leftBoundary = -15f;

    public int KeyIndexNumber
    { private get;  set; }


    private void Update()
    {
        if (transform.position.x < leftBoundary)
        {
            SpawnManager.Instance.RetrieveObj(gameObject, KeyIndexNumber);
        }
    }
}
