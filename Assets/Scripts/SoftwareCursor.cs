using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftwareCursor : MonoBehaviour
{
    public Canvas canvas;
    public float mouseSensitivity = 15.0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void KeepFullyOnScreen() {
        RectTransform target = transform.GetComponent<RectTransform>();
        RectTransform canvasRect = canvas.transform as RectTransform;
        RectTransform parentRect = target.parent.GetComponent<RectTransform>();
        Camera        cam        = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        Vector2       screenPos  = RectTransformUtility.WorldToScreenPoint(cam, target.position);
 
        float minX = target.pivot.x * target.rect.size.x;
        float maxX = canvasRect.rect.size.x - (1 - target.pivot.x) * target.rect.size.x;
        float minY = target.pivot.y * target.rect.size.y;
        float maxY = canvasRect.rect.size.y - (1 - target.pivot.y) * target.rect.size.y;
       
        screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
        screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);
 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos, cam, out Vector2 anchoredPos);
        target.localPosition = anchoredPos;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        
        transform.position += new Vector3(mouseX, mouseY, 0) * mouseSensitivity;
        KeepFullyOnScreen();
    }

    public float GetAxis(string axis) {
        return Input.GetAxis(axis);
    }

    public void SetHidden(bool hidden) {
        gameObject.SetActive(!hidden);
    }

    public void SetCursorPosition(Vector3 position) {
        transform.position = position;
    }

    public Vector3 GetCursorPosition() {
        return transform.position;
    }

    public bool GetKey(KeyCode mouseButton) {
        return Input.GetKey(mouseButton);
    }

    public bool GetMouseButtonDown(int button) {
        return Input.GetMouseButtonDown(button);
    }
}
