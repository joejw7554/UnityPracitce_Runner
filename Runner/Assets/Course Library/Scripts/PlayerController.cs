using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public event System.Action OnGameOver;

    InputAction jumpAction;
    Rigidbody playerRb;
    Animator playerAnimator;

    [SerializeField]
    float jumpPower = 10;

    [SerializeField]
    float gravityModifer = 1;

    private bool isGameOver = false;
    public bool IsGameOver
    {
        get { return isGameOver; }
        private set
        {
            isGameOver = value;
            if (isGameOver && OnGameOver != null)
                OnGameOver.Invoke();
        }
    }

    public bool isOnGround { get; set; } = true;

    private void Awake()
    {
        jumpAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
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
            if(playerAnimator)
            {
                int jumpId = Animator.StringToHash("Jump_trig");
                Debug.Log(jumpId);
                playerAnimator.SetTrigger(jumpId);
            }
        }
    }

    public void GameOver()
    {
        // 게임 오버 처리 및 이벤트 발생
        IsGameOver = true;
    }
}
