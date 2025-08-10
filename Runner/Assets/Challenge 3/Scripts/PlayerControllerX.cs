using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;


    Coroutine disableInputCoroutine;


    [SerializeField]
    float softPush=30f;

    [SerializeField]
    float maxHeight;

    [SerializeField]
    float minHeight;

    bool canInput = true;
    [SerializeField]
    float disableTime = 1f;


    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb=GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && canInput)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

    }

    IEnumerator DisableInputForShort()
    {
        yield return new WaitForSeconds(disableTime);
        Debug.Log("Am I Repeating?");
        canInput = true;
    }



    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("UpPlane"))
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.AddForce(Vector3.down * softPush, ForceMode.Impulse);
            disableInputCoroutine = StartCoroutine(DisableInputForShort());
            Debug.Log("Collided with UpPlane");
        }

        else if (other.gameObject.CompareTag("downPlane"))
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * softPush, ForceMode.Impulse);
            Debug.Log("Collided with down");

        }

    }

}
