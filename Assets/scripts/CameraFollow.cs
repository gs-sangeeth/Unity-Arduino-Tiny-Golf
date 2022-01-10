using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;



    void FixedUpdate()
    {
        try
        {
            target = GameObject.Find("arrow").transform;
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.SetPositionAndRotation(smoothedPosition, target.rotation);

            transform.LookAt(target);
        }
        catch (Exception)
        {

        }
    }
}
