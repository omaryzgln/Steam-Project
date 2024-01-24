using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimAndMoveController : MonoBehaviour
{
    PlayerInpu playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;
    //bool isDashing = false;
    //int isDashingHash;
    //private bool isDashAnimating = false;

    public bool attack;
    bool isAttackPressed;

    private Vector3 moveDir;
    public float rotateSpeed = 25f;
    public float jumpForce = 8.0f;

    private Rigidbody rb;
    private Camera cam;

    void Awake()
    {
        cam = transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInpu();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
       // isDashingHash = Animator.StringToHash("isDashing");
    }
    public void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }
    public void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    public void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    public void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleRotation()
    {
        Vector3 targetDir = moveDir;

        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed);
        transform.rotation = targetRotation;
    }
    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.0f;
            currentMovement.y += gravity * Time.deltaTime;
        }
    }

    void Update()
    {
        handleRotation();
        handleAnimation();

        if (isRunPressed)
        {
            Vector3 v2 = currentMovement.z * cam.transform.forward;
            Vector3 h2 = currentMovement.x * cam.transform.right;
            Vector3 horizontalMovement = v2 + h2;

            float verticalMovement = currentMovement.y;

            moveDir = new Vector3(horizontalMovement.x, verticalMovement, horizontalMovement.z).normalized;
            characterController.Move(moveDir * Time.deltaTime * Constants.MovementSpeedMultip * 3);
        }
        else
        {
            Vector3 v2 = currentMovement.z * cam.transform.forward;
            Vector3 h2 = currentMovement.x * cam.transform.right;
            Vector3 horizontalMovement = v2 + h2;

            float verticalMovement = currentMovement.y;

            moveDir = new Vector3(horizontalMovement.x, verticalMovement, horizontalMovement.z).normalized;
            characterController.Move(moveDir * Time.deltaTime * Constants.MovementSpeedMultip);
        }
        handleGravity();

        if (isJumpPressed && characterController.isGrounded)
        {
         
        }


        if (isAttackPressed && attack == false)
        {
            attack = true;
            Debug.Log("Attacked");

            //GameObject clone;
            //clone = Instantiate(head, new Vector3(transform.position.x +0.1f, transform.position.y +0.1f, transform.position.z +0.1f), transform.rotation) as GameObject;
        }
    }

    void OnEnable()
    {
        playerInput.CharacterControl.Enable();
    }
    void OnDisable()
    {
        playerInput.CharacterControl.Disable();
    }
}
    //void DashAnim()
    //{
    //    animator.SetBool(isDashingHash, false);
    //    isDashAnimating = false;
    //}
    //void handleDash()
    //{
    //    if(!isDashing && characterController.isGrounded && isDashPressed)
    //    {
    //        animator.SetBool(isDashingHash, true);
    //        isDashAnimating = true;
    //        isDashing = true;
    //        Invoke(nameof(DashAnim), 1.05f);
    //    }
    //    else if(!isDashPressed && isDashing && characterController.isGrounded)
    //    {
    //        isDashing = false;
    //    }
    //}