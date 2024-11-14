using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variables to set the boundaries for camera movement
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float topLimit;
    [SerializeField] private float bottomLimit;

    // Reference to the player object for camera tracking
    public Transform player;
    public float timeOffset;
    public Vector2 positionOffset;

    void Update()
    {
        // Camera follows the player with an offset
        Vector3 startPosition = transform.position;
        Vector3 endPosition = player.transform.position;

        endPosition.x += positionOffset.x;
        endPosition.y += positionOffset.y;
        endPosition.z = -10;

        // Smooth transition using Lerp function
        //transform.position = Vector3.Lerp(startPosition, endPosition, timeOffset * Time.deltaTime);
        transform.position = new Vector3(endPosition.x,endPosition.y, endPosition.z);

        // Clamping camera position within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
            transform.position.z
        );
    }

    // Method to draw boundary lines in the Unity editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw top boundary line
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));

        // Draw right boundary line
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottomLimit));

        // Draw bottom boundary line
        Gizmos.DrawLine(new Vector2(rightLimit, bottomLimit), new Vector2(leftLimit, bottomLimit));

        // Draw left boundary line
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(leftLimit, topLimit));
    }
}
