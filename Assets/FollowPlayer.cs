using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.1f;
    public BoxCollider2D cameraBounds;

    private Vector2 minPos;
    private Vector2 maxPos;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        if (cameraBounds != null)
        {
            Bounds bounds = cameraBounds.bounds;
            minPos = bounds.min;
            maxPos = bounds.max;
        }

        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x + halfWidth, maxPos.x - halfWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y + halfHeight, maxPos.y - halfHeight);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        }
    }
}
