using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        transform.position = new Vector3(desiredPosition.x, desiredPosition.y, offset.z);
    }
}
