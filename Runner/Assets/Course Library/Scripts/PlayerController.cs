using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    InputAction jumpAction;
    Rigidbody playerRb;
    Animator playerAnimator;


    ParticleSystem smokeParticle;

    ParticleSystem dirtParticle;

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

        GameManager.Instance.OnGameOver += PlayerGameOver;

        smokeParticle = transform.Find("FX_Explosion_Smoke").GetComponent<ParticleSystem>();
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
        }
    }

    public void PlayerGameOver()
    {
        HandleDeathAnimation();
        jumpAction.Disable();
        smokeParticle.Play();
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
