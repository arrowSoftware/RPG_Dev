using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum MountType {
    flying, ground
}

public class PlayerControls : MonoBehaviour
{   
    // Inputs
    public Controls controls;
    Vector2 inputs;
    [HideInInspector]
    public Vector2 inputNormalized;
    bool run = true;
    bool jump = true;
    [HideInInspector]
    public float rotation;
    [HideInInspector]
    public bool steer;
    [HideInInspector]
    public bool autorun;

    // Velocity
    public Vector3 velocity;
    readonly float gravity = -15;
    float velocityY = 0;
    readonly float terminalVelocity = -25;
    float fallMult = 0;

    // Running
    public float baseSpeed = 1.0f;
    public float runSpeed = 4.0f;
    public float rotateSpeed = 2f;
    float currentSpeed;

    // Ground
    Vector3 collisionPoint;
    Vector3 forwardDirection;
    float slopeAngle;
    float forwardAngle;
    float directionAngle;
    float strafeAngle;
    float forwardMultiplier;
    float strafeMultiplier;
    Ray groundRay;
    RaycastHit groundHit; 
    
    // Mounted
    public MountedState mountedState = MountedState.unmounted;
    public float mountedSpeed = 1.6f;
    public float flyingSpeed = 2.5f;
    public bool mount = false;
    public MountType mountType;

    // jumping
    [SerializeField]
    bool jumping;
    float jumpSpeed;
    readonly float jumpHeight = 3.0f;
    Vector3 jumpDirection;
    float jumpStartPosY;
    private bool canJump = true;

    // Swimming
    [HideInInspector]
    public float waterSurface;
    [HideInInspector]
    public bool inWater;
    readonly float swimSpeed = 2.0f;
    [HideInInspector]
    public float distanceFromWaterSurface;
    readonly float swimLevel = 1.0f;

    // References
    CharacterController controller;
    public Transform groundDirection;
    public Transform moveDirection;
    public Transform fallDirection;
    public Transform swimDirection;
    [HideInInspector]
    public CameraController mainCam;
    public GameObject inventoryUI;
    public GameObject actionBarUI;
    
    public enum MoveState {
        locomotion, swimming, flying
    }

    public enum MountedState {
        unmounted, mounted, mountedFlying
    }

    public MoveState moveState = MoveState.locomotion;
    public LayerMask groundMask;

    // Debug
    public bool showGroundNormal;
    public bool showFallNormal;
    public bool showMoveDirectionRay;
    public bool showForwardDirection;
    public bool showStrafeDirection;
    public bool showSwimNormal;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controls.inventory.GetControlBindingDown()) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }

        GetInputs();
        GetSwimDirection();
    
        if (inWater) {
            GetWaterLevel();            
        }
        
        if (mount) {
            if (mountType == MountType.ground) {
                if (mountedState != MountedState.mounted) {
                    mountedState = MountedState.mounted;
                }
            } else if (mountType == MountType.flying) {
                if (mountedState != MountedState.mountedFlying) {
                    mountedState = MountedState.mountedFlying;
                }
            }
        } else {
            if (mountedState != MountedState.unmounted) {
                mountedState = MountedState.unmounted;
            }
        }
        
        switch (moveState) {
            case MoveState.locomotion:
            {
                Locomotion();
                break;
            }

            case MoveState.swimming:
            {
                Swimming();
                break;
            }

            case MoveState.flying:
            {
                Flying();
                break;
            }
        }
    }

    void GetInputs()
    {
        // autorun
        if (controls.autorun.GetControlBindingDown())
        {
            autorun = !autorun;
        }

        // Forwards Backwards movement
        inputs.y = Axis(controls.forwards.GetControlBinding(), controls.backwards.GetControlBinding());

        if (inputs.y != 0 && !mainCam.autorunReset)
        {
            autorun = false;
        }
        
        if (autorun)
        {
            inputs.y += 1;
            inputs.y = Mathf.Clamp(inputs.y, -1, 1);
        }

        // Strafe movement
        inputs.x = Axis(controls.strafeRight.GetControlBinding(), controls.strafeLeft.GetControlBinding());

        if (steer)
        {
            inputs.x = Axis(controls.rotateRight.GetControlBinding(), controls.rotateLeft.GetControlBinding());
            inputs.x = Mathf.Clamp(inputs.x, -1, 1);
        }

        // Rotate movement
        if (steer)
        {
            rotation = Input.GetAxis("Mouse X") * mainCam.cameraSpeed;
        }
        else
        {
            rotation = Axis(controls.rotateRight.GetControlBinding(), controls.rotateLeft.GetControlBinding());
        }

        // run
        if (controls.walk_run.GetControlBindingDown())
        {
            run = !run;
        }

        // jump
        jump = controls.jump.GetControlBinding();

        inputNormalized = inputs.normalized;
    }

    void Locomotion()
    {
        GroundDirecton();

        // Rotate
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation*rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        // Running and walking
        if (controller.isGrounded && slopeAngle <= controller.slopeLimit)
        {
            currentSpeed = baseSpeed;

            if (run)
            {
                currentSpeed *= runSpeed;

                if (mountedState != MountedState.unmounted) {
                    currentSpeed *= mountedSpeed;
                }

                if (inputNormalized.y < 0) 
                {
                    currentSpeed /= 2.0f;
                }
            }
        }
        else if (!controller.isGrounded || slopeAngle > controller.slopeLimit)
        {
            inputNormalized = Vector2.Lerp(inputNormalized, Vector2.zero, 0.025f);

            currentSpeed = Mathf.Lerp(currentSpeed, 0, 0.025f);
        }

        // Press jump button to jump
        if (jump && controller.isGrounded && slopeAngle <= controller.slopeLimit && !jumping && canJump) {
            Jump();
        }

        // Apply gravity if not grounded.
        if (!controller.isGrounded)
        {
            switch (mountedState) {
                case MountedState.unmounted:
                case MountedState.mounted:
                {
                    if (velocityY > terminalVelocity){
                        velocityY += gravity * Time.deltaTime;
                    }
                    break;
                }
                case MountedState.mountedFlying:
                {
                    if (Physics.Raycast(groundRay, out groundHit, 0.15f, groundMask)) {
                        if (velocityY > terminalVelocity) {
                            velocityY += gravity * Time.deltaTime;
                        }
                    } else {
                        moveState = MoveState.flying;
                    }
                    break;
                }
            }
        }
        else if (controller.isGrounded && slopeAngle > controller.slopeLimit)
        {
            velocityY = Mathf.Lerp(velocityY, terminalVelocity, 0.25f);
        }

        // Checking water level
        if (inWater) {
            // Setting the ray
            groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
            groundRay.direction = Vector3.down;

// Turn on for slowing down when entering/walking in shallow water
//            if (Physics.Raycast(groundRay, out groundHit, 0.15f)) {
//                currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed,  distanceFromWaterSurface/swimLevel);
//            }

            if (distanceFromWaterSurface >= swimLevel) {
                if (jumping) {
                    jumping = false;
                }
                moveState = MoveState.swimming;
            }
        }

        // Apply Inputs
        if (!jumping)
        {
            // Applying movement direction inputs
            velocity = groundDirection.forward * inputNormalized.y * forwardMultiplier + groundDirection.right * inputNormalized.x * strafeMultiplier;
            // Applying current move speed
            velocity *= currentSpeed;
            // gravity
            velocity += fallDirection.up * (velocityY * fallMult);
        }
        else
        {
            //velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;
            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY + groundDirection.forward * inputNormalized.y + groundDirection.right * inputNormalized.x;
        }

        // Move controller
        controller.Move(velocity * Time.deltaTime);

        if (mountedState == MountedState.mountedFlying) {
            float currentJumpHeight = transform.position.y - jumpStartPosY;
            // TODO when you hop onto a flying mount you are always flying near the ground there currently is no flying mount running on ground.
            // this is becuase this math takes the start jump posisiion but since you didnt jump it treats you as flying right away
            if (currentJumpHeight > 0.5f) {
                moveState = MoveState.flying;
            }
        }

        if (controller.isGrounded)
        {
            if (jumping)
            {
                jumping = false;
            }
            
            if (!jump && !canJump) {
                canJump = true;
            }
            velocityY = 0;
        }
    }

    void GroundDirecton()
    {
        // Setting ground direction to controller position
        forwardDirection = transform.position;

        // Setting forard direction based on controller inputs.
        if (inputNormalized.magnitude > 0)
        {
            forwardDirection += transform.forward * inputNormalized.y + transform.right * inputNormalized.x; 
        }
        else
        {
            forwardDirection += transform.forward;
        }

        moveDirection.LookAt(forwardDirection);
        fallDirection.rotation = transform.rotation;
        groundDirection.rotation = transform.rotation;

        // Setting the ray
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        if (showForwardDirection)
        {
            Debug.DrawLine(groundRay.origin, groundRay.origin + Vector3.down * 0.3f, Color.red);
        }

        forwardMultiplier = 1;
        fallMult = 1;
        strafeMultiplier = 1;

        if (Physics.Raycast(groundRay, out groundHit, 0.3f, groundMask))
        {
            slopeAngle = Vector3.Angle(transform.up, groundHit.normal);
            directionAngle = Vector3.Angle(moveDirection.forward, groundHit.normal) -90.0f;

            if (directionAngle < 0 && slopeAngle <= controller.slopeLimit)
            {
                // Checking the forward angle to the slope
                forwardAngle = Vector3.Angle(transform.forward, groundHit.normal) - 90.0f;
                // Applying the movement multiplier based on forward angle
                forwardMultiplier = 1 / Mathf.Cos(forwardAngle * Mathf.Deg2Rad);
                // Rotating ground direction x.
                groundDirection.eulerAngles += new Vector3(-forwardAngle, 0, 0);

                // checing strafe angle agasint the slope.
                strafeAngle = Vector3.Angle(groundDirection.right, groundHit.normal) - 90.0f;
                // Applying the movement multiplier based on strafe angle
                strafeMultiplier = 1 / Mathf.Cos(strafeAngle * Mathf.Deg2Rad);
                groundDirection.eulerAngles += new Vector3(0, 0, strafeAngle);
           }
            else if (slopeAngle > controller.slopeLimit)
            {
                float groundDistance = Vector3.Distance(groundRay.origin, groundHit.point);

                if (groundDistance <= 0.1f)
                {
                    fallMult = 1 / Mathf.Cos((90 - slopeAngle) * Mathf.Deg2Rad);
                    Vector3 groundCross = Vector3.Cross(groundHit.normal, Vector3.up);
                    fallDirection.rotation = Quaternion.FromToRotation(transform.up, Vector3.Cross(groundCross, groundHit.normal));
                }   
            }
        }

        PlayerDebug();
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStartPosY = transform.position.y;
            jumping = true;
            canJump = false;
        }

        switch (moveState) {
            case MoveState.locomotion:
            {
                jumpDirection = (transform.forward * inputs.y + transform.right * inputs.x).normalized;
                jumpSpeed = currentSpeed;

                velocityY = Mathf.Sqrt(-gravity * jumpHeight);                
                break;
            }
            case MoveState.swimming:
            {
                jumpDirection = (transform.forward * inputs.y + transform.right * inputs.x).normalized;
                jumpSpeed = swimSpeed;

                velocityY = Mathf.Sqrt(-gravity * jumpHeight * 2.0f);     
                break;
            }
            case MoveState.flying:
            default:
            {
                break;
            }
        }
    }

    void GetSwimDirection() {
        if (steer) {
            swimDirection.eulerAngles = transform.eulerAngles + new Vector3(mainCam.tilt.eulerAngles.x, 0, 0);
        }
    }

    void GetWaterLevel() {
        distanceFromWaterSurface = waterSurface - transform.position.y;
    }

    void Swimming() {

        if (!inWater && !jumping) {
            velocityY = 0;
            velocity = new Vector3(velocity.x, velocityY, velocity.z);
            jumpDirection = velocity;
            jumpSpeed = swimSpeed / 2;
            jumping = true;
            moveState = MoveState.locomotion;
        }

        // Rotate
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation*rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        // Setting the ray
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        if (showForwardDirection) {
            Debug.DrawLine(groundRay.origin, groundRay.origin + Vector3.down * 0.15f, Color.red);
        }

        if (!jumping && jump && distanceFromWaterSurface <= swimLevel) {
            Jump();
        }

        if (!jumping) {
            velocity = swimDirection.forward * inputNormalized.y + swimDirection.right * inputNormalized.x;

            velocity.y += Axis(jump, controls.sit.GetControlBinding());

            // Prevent double speed when swimming up with space bar
            velocity = velocity.normalized;

            velocity *= swimSpeed;

            controller.Move(velocity * Time.deltaTime);

            if (Physics.Raycast(groundRay, out groundHit, 0.15f, groundMask)) {
                if (distanceFromWaterSurface < swimLevel) {
                    moveState = MoveState.locomotion;
                }
            } else {
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, float.MinValue, waterSurface - swimLevel), transform.position.z);
            }
        } else {
            // Jump
            if (velocityY > terminalVelocity) {
                velocityY += gravity * Time.deltaTime;
            }

            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            if (mountedState == MountedState.mountedFlying) {
                float currentJumpHeight = transform.position.y - jumpStartPosY;
                if (currentJumpHeight > 0.5f) {
                    moveState = MoveState.flying;
                }
            }

            if (Physics.Raycast(groundRay, out groundHit, 0.15f, groundMask)) {
                if (distanceFromWaterSurface < swimLevel) {
                    moveState = MoveState.locomotion;
                }
            }

            if (distanceFromWaterSurface > swimLevel) {
                jumping = false;    
            }
        }
    }

    void Flying() {
        if (mountedState == MountedState.unmounted) {
            moveState = MoveState.locomotion;
        }

        if (jumping) {
            velocityY = 0;
            jumping = false;
        }

        // Rotate
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation*rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        // Setting the ray
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        if (showForwardDirection) {
            Debug.DrawLine(groundRay.origin, groundRay.origin + Vector3.down * 0.15f, Color.red);
        }

        velocity = swimDirection.forward * inputNormalized.y + swimDirection.right * inputNormalized.x;

        velocity.y += Axis(jump, controls.sit.GetControlBinding());

        // Prevent double speed when swimming up with space bar
        velocity = velocity.normalized;

        velocity *= runSpeed * flyingSpeed;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded) {
            moveState = MoveState.locomotion;
        }

        if (inWater && distanceFromWaterSurface >= swimLevel) {
            moveState = MoveState.swimming;
        }
    }

    public float Axis(bool positive, bool negative)
    {
        float axis = 0;

        if (positive)
        {
            axis += 1;
        }

        if (negative)
        {
            axis -= 1;
        }
        return axis;
    }

    void PlayerDebug()
    {
        Vector3 lineStart = transform.position + Vector3.up * -1f;

        if (showMoveDirectionRay)
        {
            Debug.DrawLine(lineStart, lineStart + moveDirection.forward * 1.5f, Color.cyan);
        }

        if (showForwardDirection)
        {
            Debug.DrawLine(lineStart - groundDirection.forward * 0.5f, lineStart + groundDirection.forward * 0.5f, Color.blue);
        }

        if (showStrafeDirection)
        {
            Debug.DrawLine(lineStart - groundDirection.right * 0.5f, lineStart + groundDirection.right * 0.5f, Color.red);
        }

        if (showFallNormal)
        {
            Debug.DrawLine(lineStart, lineStart + fallDirection.up * 0.5f, Color.green);
        }

        if (showSwimNormal)
        {
            Debug.DrawLine(lineStart, lineStart + swimDirection.forward, Color.magenta);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if (hit.point.y <= transform.position.y + 0.25f)
        {
            collisionPoint = hit.point;
            collisionPoint = collisionPoint - transform.position;
        }
    }

    public void Teleport(Vector3 position, Quaternion rotation) {
        transform.position = position;
        Physics.SyncTransforms();
        transform.eulerAngles = rotation.eulerAngles;
        velocity = Vector3.zero;
        GetComponent<NavMeshAgent>().Warp(transform.position);
    }
}
