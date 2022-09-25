using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 velocity = Vector3.zero;

    // Movement Lag
    public float smoothTime = 0.15f;

    public float minX = 0;
    public float maxX = 0;
    public float minY = 0;
    public float maxY = 0;

    private void Start()
    {
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        minX = horzExtent - (float)(31.875 / 2.0);
        maxX = (float)(31.875 / 2.0) - horzExtent;
        minY = vertExtent - (float)(21 / 2.0);
        maxY = (float)(21.0 / 2.0) - vertExtent;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;

            targetPos.x = Mathf.Clamp(target.position.x, minX, maxX);
            targetPos.y = Mathf.Clamp(target.position.y, minY, maxY);

            targetPos.z = transform.position.z;

            // Camera follows target with a small delay
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }

}
