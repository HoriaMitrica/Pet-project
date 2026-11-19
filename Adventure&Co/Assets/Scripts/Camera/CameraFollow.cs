using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    // LateUpdate so the camera moves after the player has moved this frame (avoids jitter).
    private void LateUpdate()
    {
        var position = player.transform.position;
        transform.position = new Vector3(position.x,position.y+1,-10);
    }
}
