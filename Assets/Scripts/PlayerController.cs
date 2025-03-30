using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputActions.IGameplayActions
{
    [SerializeField] float moveSpeed = 5f;
    private InputActions inputActions;
    private Vector2 movementInput;
    private bool isActive = false;
    
    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);
    }
    
    private void Start()
    {
        // 禁用俄罗斯方块输入
        PlayerInput.DisableAllInputs();
    
        // 启用玩家输入
        inputActions.Gameplay.Enable();
    }

    void OnEnable()
    {
        inputActions.Gameplay.Enable();
        isActive = true;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Disable();
        isActive = false;
    }

    void Update()
    {
        if(!isActive) return;
        
        // 在XZ平面移动
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        movementInput.x = context.ReadValue<float>() * -1;
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        movementInput.x = context.ReadValue<float>();
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        movementInput.y = context.ReadValue<float>() * -1;
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        movementInput.y = context.ReadValue<float>();
    }
}