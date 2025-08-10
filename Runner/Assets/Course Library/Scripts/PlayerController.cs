using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum ParticleType
{
    Smoke, Dirt
}

public enum SoundEffctType
{
    Crash, Jump
}

[System.Serializable]
public class SoundEffectEntry
{
    public SoundEffctType type;
    public AudioClip clip;
}




public class PlayerController : MonoBehaviour
{
    InputAction jumpAction;
    Rigidbody playerRb;
    Animator playerAnimator;
    AudioSource playerAudioSource;

    Dictionary<ParticleType, ParticleSystem> particles;

    [SerializeField]
    List<SoundEffectEntry> audioClipList;

    Dictionary<SoundEffctType, AudioClip> soundEffects;



    [SerializeField]
    float jumpPower = 10;

    [SerializeField]
    float gravityModifer = 1;

    public bool isOnGround { get; set; } = true;

    private void Awake()
    {
        jumpAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();



        particles = new Dictionary<ParticleType, ParticleSystem>();
        particles.Add(ParticleType.Smoke, transform.Find("FX_Explosion_Smoke").GetComponent<ParticleSystem>());
        particles.Add(ParticleType.Dirt, transform.Find("FX_DirtSplatter").GetComponent<ParticleSystem>());


        soundEffects=new Dictionary<SoundEffctType, AudioClip>();
        foreach (var entry in audioClipList)
        {
            soundEffects.Add(entry.type, entry.clip);
        }

    }
    private void OnEnable()
    {
        jumpAction.Enable();
    }
    private void OnDisable()
    {
        jumpAction.Disable();
    }

    void Start()
    {
        Physics.gravity *= gravityModifer;
        playerRb.constraints = RigidbodyConstraints.FreezePositionZ;

        if (GameManager.Instance)
        {
            Debug.Log("Not Null");
            GameManager.Instance.OnGameOver += PlayerGameOver;
            PlayParticle(ParticleType.Dirt);

        }
    }

    void Update()
    {
        if (jumpAction.WasPressedThisFrame() && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isOnGround = false;
            if (playerAnimator)
            {
                int jumpId = Animator.StringToHash("Jump_trig");
                playerAnimator.SetTrigger(jumpId);
            }

            if (playerAudioSource)
            {
                PlaySoundeffectOneShot(SoundEffctType.Jump);
            }

            if (particles.ContainsKey(ParticleType.Dirt))
            {
                StopParticle(ParticleType.Dirt);
            }
        }
    }

    public void PlayerGameOver()
    {
        HandleDeathAnimation();
        jumpAction.Disable();
        StopParticle(ParticleType.Dirt);
        PlaySoundeffectOneShot(SoundEffctType.Crash);
    }

    public void PlayParticle(ParticleType particleType)
    {
        if (particles.ContainsKey(particleType))
        {
            particles[particleType].Play();
        }
    }


    void PlaySoundeffectOneShot(SoundEffctType effectType)
    {
        if (soundEffects.ContainsKey(effectType))
        {
            playerAudioSource.PlayOneShot(soundEffects[effectType]);
        }
    }

    void StopParticle(ParticleType particleName)
    {
        if (particles.ContainsKey(particleName))
        {
            particles[particleName].Stop();
        }
    }

    private void HandleDeathAnimation()
    {
        if (playerAnimator)
        {
            playerAnimator.SetInteger("DeathType_int", 2);
            playerAnimator.SetBool("Death_b", true);
        }
    }
}
