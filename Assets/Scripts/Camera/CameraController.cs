using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public SoftwareCursor swCursor;

    // Camera state
    public enum CameraState {CameraNone, CameraRotate, CameraSteer, CameraRun}
    CameraState cameraState = CameraState.CameraNone;
    public enum CameraMoveState {OnlyWhileMoving, OnlyHorizontalWhileMoving, AlwaysAdjust, NeverAdjust}
    
    // Camera smoothing
    float panAngle;
    float panOffset;
    bool camXAdjust;
    bool camYAdjust;
    readonly float camRotationXCusion = 3.0f;
    readonly float yRotMin = 0.0f;
    readonly float yRotMax = 20.0f;
    float camRotationXSpeed = 0.0f;
    float camRotationYSpeed = 0.0f;

    // Camera variables
    readonly float cameraHeight = 0.80f;
    readonly float cameraMaxDistance = 25.0f;
    readonly float cameraMaxTilt = 90.0f;
    float currentPan;
    float currentTilt = 10.0f;
    float currentDistance = 5.0f;
    [HideInInspector]
    public bool autorunReset = false;

    // Input variables
    readonly KeyCode leftMouse = KeyCode.Mouse0;
    readonly KeyCode rightMouse = KeyCode.Mouse1;
    readonly KeyCode middleMouse = KeyCode.Mouse2;

    // Collisions
    public bool collisionDebug;
    public float collisionCushion = 0.35f;
    public float clipCusion = 1.5f;
    public int rayGridX = 9;
    public int rayGridY = 5;
    public LayerMask collisionMask;
    Vector3[] camClip;
    Vector3[] clipDirection;
    Vector3[] playerClip;
    Vector3[] rayColOrigin;
    Vector3[] rayColPoint;
    bool[] rayColHit;
    float adjustedCamDistance;
    Ray camRay;
    RaycastHit camRayHit;

    // References
    PlayerControls player;
    public Transform tilt;
    Camera mainCam;

    Vector3 mousePositionAtClick;
    bool mouseMove = false;

    // Camera Player Options
    [Header("Camera Options")]
    [Range(0.025f, 1.75f)]
    public float cameraAutoFollowSpeed = 1;
    public CameraMoveState cameraMoveState = CameraMoveState.OnlyWhileMoving;
    [Range(0,4)]
    public float cameraSpeed = 2.0f;

    bool HasMouseMoved() {
        return (swCursor.GetAxis("Mouse X") != 0) || (swCursor.GetAxis("Mouse Y") != 0);
    }

    private void Awake() {
        transform.SetParent(null);
    }

    void Start() {
        // Get the player controls object from the player in the scene (TODO, wont work with multiplayer. need to reference directly)
        player = FindAnyObjectByType<PlayerControls>();

        // Set the main camera
        mainCam = Camera.main;

        // Set the players camera reference to this object
        player.mainCam = this;

        // Set the default camera position and rotation (TODO allow public camera settings?)
        transform.SetPositionAndRotation(player.transform.position + Vector3.up * cameraHeight, player.transform.rotation);

        // Set the default tilt angle of the camera (TODO allow public tilt settings?)
        tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);

        // Set the camera position offset to behind the player
        mainCam.transform.position += tilt.forward * -currentDistance;
        CameraClipInfo();
    }

    Vector2 GetMousePosition() {
        return swCursor.GetCursorPosition();
    }

    void SetMousePosition(int x, int y) {
        swCursor.SetCursorPosition(new Vector3(x, y, 0));
    }

    void Update()
    {
        // No mouse button
        if (!swCursor.GetKey(leftMouse) && !swCursor.GetKey(rightMouse) && !swCursor.GetKey(middleMouse)) {
            cameraState = CameraState.CameraNone;
            swCursor.SetHidden(false);
            if (mouseMove) {
                SetMousePosition((int)mousePositionAtClick.x, (int)mousePositionAtClick.y);
                mouseMove = false;
            }
        }
        // Left button
        else if (swCursor.GetKey(leftMouse) && !swCursor.GetKey(rightMouse) && !swCursor.GetKey(middleMouse)) {
            cameraState = CameraState.CameraRotate;
            if (HasMouseMoved()) {
                swCursor.SetHidden(true);
                mousePositionAtClick = GetMousePosition();
                mouseMove = true;
            }
        }
        // Right mouse button
        else if (!swCursor.GetKey(leftMouse) && swCursor.GetKey(rightMouse) && !swCursor.GetKey(middleMouse)) {
            cameraState = CameraState.CameraSteer;
            if (HasMouseMoved()) {
                swCursor.SetHidden(true);
                mousePositionAtClick = GetMousePosition();
                mouseMove = true;
            }
        }
        // Left and right mouse button or middile mouse button
        else if ((swCursor.GetKey(leftMouse) && swCursor.GetKey(rightMouse)) || swCursor.GetKey(middleMouse)) {
            cameraState = CameraState.CameraRun;
            if (HasMouseMoved()) {
                swCursor.SetHidden(true);
                mousePositionAtClick = GetMousePosition();
                mouseMove = true;
            }
        }

        if (rayGridX * rayGridY != rayColOrigin.Length)
        {
            CameraClipInfo();
        }

        CameraCollisions();
        CameraInputs();
    }

    void LateUpdate() 
    {
        // Get the current pan angle.
        panAngle = Vector3.SignedAngle(transform.forward, player.transform.forward, Vector3.up);

        switch (cameraMoveState)
        {
            case CameraMoveState.OnlyWhileMoving:
            {
                // If player is moving
                if ((player.inputNormalized.magnitude > 0) || (player.rotation != 0))
                {
                    CameraXAdjust();
                    CameraYAdjust();
                }
                break;
            }
            case CameraMoveState.OnlyHorizontalWhileMoving:
            {
                if ((player.inputNormalized.magnitude > 0) || (player.rotation != 0))
                {
                    CameraXAdjust();
                }
                break;
            }
            case CameraMoveState.AlwaysAdjust:
            {
                CameraXAdjust();
                CameraYAdjust();
                break;
            }
            case CameraMoveState.NeverAdjust:
            {
                CameraNeverAdjust();
                break;
            }
            default:
            {
                break;
            }
        }

        CameraTransforms();
    }

    void CameraClipInfo()
    {
        camClip = new Vector3[4];

        mainCam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), mainCam.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, camClip);

        clipDirection = new Vector3[4];
        playerClip = new Vector3[4];

        int rays = rayGridX * rayGridY;
        rayColOrigin = new Vector3[rays];
        rayColPoint = new Vector3[rays];
        rayColHit = new bool[rays];
    }

    void CameraCollisions()
    {
        float camDistance = currentDistance + collisionCushion;

        for (int i = 0; i < camClip.Length; i++)
        {
            Vector3 clipPoint = mainCam.transform.up * camClip[i].y + mainCam.transform.right * camClip[i].x;
            clipPoint *= clipCusion;
            clipPoint += mainCam.transform.forward * camClip[i].z;
            clipPoint += transform.position - (tilt.forward * cameraMaxDistance);

            Vector3 playerPoint = mainCam.transform.up * camClip[i].y + mainCam.transform.right * camClip[i].x;
            playerPoint += transform.position;

            clipDirection[i] = (clipPoint - playerPoint).normalized;
            playerClip[i] = playerPoint;
        }

        int currentRay = 0;
        bool isColliding = false;

        float rayX = rayGridX - 1;
        float rayY = rayGridY - 1;

        for (int x = 0; x < rayGridX; x++)
        {
            Vector3 clipUpperPoint = Vector3.Lerp(clipDirection[1], clipDirection[2], x / rayX);
            Vector3 clipLowerPoint = Vector3.Lerp(clipDirection[0], clipDirection[3], x / rayX);

            Vector3 playerUpperPoint = Vector3.Lerp(playerClip[1], playerClip[2], x / rayX);
            Vector3 playerLowerPoint = Vector3.Lerp(playerClip[0], playerClip[3], x / rayX);

            for (int y = 0; y < rayGridY; y++)
            {
                camRay.origin = Vector3.Lerp(playerUpperPoint, playerLowerPoint, y / rayY);
                camRay.direction = Vector3.Lerp(clipUpperPoint, clipLowerPoint, y / rayY);
                rayColOrigin[currentRay] =  camRay.origin;

                if (Physics.Raycast(camRay, out camRayHit, camDistance, collisionMask))
                {
                    isColliding = true;
                    rayColHit[currentRay] = true;
                    rayColPoint[currentRay] = camRayHit.point;

                    if (collisionDebug)
                    {
                        Debug.DrawLine(camRay.origin, camRayHit.point, Color.cyan);
                        Debug.DrawLine(camRayHit.point, camRay.origin + camRay.direction * camDistance, Color.magenta);
                    }
                }
                else
                {
                    if (collisionDebug)
                    {
                        Debug.DrawLine(camRay.origin, camRay.origin + camRay.direction * camDistance, Color.cyan);
                    }
                }

                currentRay++;
            }
        }

        if (isColliding)
        {
            float minRayDistance = float.MaxValue;
            currentRay = 0;

            for (int i = 0; i < rayColHit.Length; i++)
            {
                if (rayColHit[i])
                {
                    float colDistance = Vector3.Distance(rayColOrigin[i], rayColPoint[i]);

                    if (colDistance < minRayDistance)
                    {
                        minRayDistance = colDistance;
                        currentRay = i;
                    }
                }
            }

            Vector3 clipCenter = transform.position - (tilt.forward * currentDistance);

            adjustedCamDistance = Vector3.Dot(-mainCam.transform.forward, clipCenter - rayColPoint[currentRay]);
            adjustedCamDistance = currentDistance - (adjustedCamDistance + collisionCushion);
            adjustedCamDistance = Mathf.Clamp(adjustedCamDistance, 0, cameraMaxDistance);
        }
        else
        {
            adjustedCamDistance = currentDistance;
        }
    }

    void CameraInputs()
    {
        if (cameraState != CameraState.CameraNone)
        {
            // If camera is not being adjusted and the state is set to adjust, then adjust
            if (!camYAdjust && (cameraMoveState == CameraMoveState.AlwaysAdjust || cameraMoveState == CameraMoveState.OnlyWhileMoving))
            {
                camYAdjust = true;
            }

            if (cameraState == CameraState.CameraRotate)
            {
                // If camera is not being adjusted and the state is set to an adjustable state.
                if (!camXAdjust && cameraMoveState != CameraMoveState.NeverAdjust)
                {
                    camXAdjust = true;
                }

                if (player.steer == true)
                {
                    player.steer = false;
                }

                // Set pan to mouse x input.
                currentPan += swCursor.GetAxis("Mouse X") * cameraSpeed;
            }
            else if (cameraState == CameraState.CameraSteer || cameraState == CameraState.CameraRun)
            {
                if (player.steer == false)
                {
                    Vector3 playerReset = player.transform.eulerAngles;
                    playerReset.y = transform.eulerAngles.y;
                    player.transform.eulerAngles = playerReset;
                    player.steer = true;
                }
            }
            currentTilt -= swCursor.GetAxis("Mouse Y") * cameraSpeed;
            currentTilt = Mathf.Clamp(currentTilt, -cameraMaxTilt, cameraMaxTilt);
        }
        else 
        {
            if (player.steer == true)
            {
                player.steer = false;
            }
        }

        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * 2;
        currentDistance = Mathf.Clamp(currentDistance, 0, cameraMaxDistance);
    }

    void CameraNeverAdjust()
    {
        switch (cameraState)
        {
            case CameraState.CameraSteer:
            case CameraState.CameraRun:
            {
                if (panOffset != 0)
                {
                    panOffset = 0;
                }
                currentPan = player.transform.eulerAngles.y;
                break;
            }
            case CameraState.CameraNone:
            {
                currentPan = player.transform.eulerAngles.y - panOffset;
                break;
            }
            case CameraState.CameraRotate:
            {
                panOffset = panAngle;
                break;
            }
        }
    }

    void CameraXAdjust()
    {
        if (cameraState != CameraState.CameraRotate)
        {
            if (camXAdjust)
            {
                camRotationXSpeed += Time.deltaTime * cameraAutoFollowSpeed;

                if (Mathf.Abs(panAngle) > camRotationXCusion)
                {
                    currentPan = Mathf.Lerp(currentPan, currentPan+panAngle, camRotationXSpeed);
                }
                else
                {
                    camXAdjust = false;
                }
            }
            else
            {
                if (camRotationXSpeed > 0)
                {
                    camRotationXSpeed = 0;
                }
                currentPan = player.transform.eulerAngles.y;
            }
        }
    }

    void CameraYAdjust()
    {
        if (cameraState == CameraState.CameraNone)
        {
            if (camYAdjust)
            {
                camRotationYSpeed += (Time.deltaTime / 2) * cameraAutoFollowSpeed;
                if (currentTilt >= yRotMax || currentTilt <= yRotMin)
                {
                    currentTilt = Mathf.Lerp(currentTilt, yRotMax / 2, camRotationYSpeed);
                }
                else if (currentTilt < yRotMax && currentTilt > yRotMin)
                {
                    camYAdjust = false;
                }
            }
            else
            {
                if (camRotationYSpeed > 0)
                {
                    camRotationYSpeed = 0;
                }
            }
        }
    }

    void CameraTransforms()
    {
        if (cameraState == CameraState.CameraRun)
        {
            player.autorun = true;
            if (!autorunReset)
            {
                autorunReset = true;
            }
        }
        else
        {
            if (autorunReset)
            {
                player.autorun = false;
                autorunReset = false;       
            }
        }

        transform.position = player.transform.position + Vector3.up * cameraHeight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(currentTilt, tilt.eulerAngles.y, tilt.eulerAngles.z);
        mainCam.transform.position = transform.position + tilt.forward * -adjustedCamDistance;
    }
}
