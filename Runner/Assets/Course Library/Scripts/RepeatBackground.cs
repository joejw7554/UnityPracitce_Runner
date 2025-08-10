using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    Vector3 startPos;
    float repeatX;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (GetComponent<SpriteRenderer>())
        {
            repeatX = GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
        }

    }
    void Start()
    {
        startPos = transform.position;
       
    }

    void Update()
    {
        if (transform.position.x < startPos.x - repeatX)
        {
            transform.position = startPos;
        }
    }
}
