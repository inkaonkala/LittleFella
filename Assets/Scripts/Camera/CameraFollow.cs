using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    //Box size!
    public Vector2 followOffset;
    public float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 cameraPos = transform.position;
        Vector3 targetPos = target.position;

        //Check box edges sides
        if (targetPos.x < cameraPos.x - followOffset.x)
            cameraPos.x = targetPos.x + followOffset.x;
        if (targetPos.x > cameraPos.x + followOffset.x)
            cameraPos.x = targetPos.x - followOffset.x;

        // Check box edges up and down
        if (targetPos.y < cameraPos.y - followOffset.y)
            cameraPos.y = targetPos.y + followOffset.y;
        if (targetPos.y > cameraPos.y + followOffset.y)
            cameraPos.y = targetPos.y - followOffset.y;

        transform.position = Vector3.SmoothDamp(transform.position,
                                                new Vector3(cameraPos.x, cameraPos.y, transform.position.z),
                                                ref velocity,
                                                smoothTime);
    }
}
