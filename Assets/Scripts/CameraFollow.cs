using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerPos;
    public Camera mainCamera;

    private Vector3 camVelocity = Vector3.zero;
    private float targetPosX;
    private float targetPosY;
    private Vector3 targetPos;

    private float cameraHalfHeight;
    private float cameraHalfWidth;

    public float offsetX = 0f;
    public float offsetY = 0f;

    public Transform borderR, borderL, borderU, borderD;

    void Start()
    {
        cameraHalfHeight = mainCamera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;
    }

    void FixedUpdate()
    {
        if (playerPos == null) // Player is dead or not assigned
        {
            return;
        }

        float borderLeftX = borderL.TransformPoint(Vector3.zero).x;
        float borderRightX = borderR.TransformPoint(Vector3.zero).x;
        float borderUpY = borderU.TransformPoint(Vector3.zero).y;
        float borderDownY = borderD.TransformPoint(Vector3.zero).y;

        targetPosX = Mathf.Clamp(playerPos.position.x, (borderLeftX + cameraHalfWidth) + offsetX, (borderRightX - cameraHalfWidth) - offsetX);
        targetPosY = Mathf.Clamp(playerPos.position.y, (borderDownY + cameraHalfHeight) + offsetY, (borderUpY - cameraHalfHeight) - offsetY);

        targetPos = new Vector3(targetPosX, targetPosY, mainCamera.transform.position.z);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPos, ref camVelocity, 0.2f);
    }
}
