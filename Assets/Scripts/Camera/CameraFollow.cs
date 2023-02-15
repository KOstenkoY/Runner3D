using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float minDodgeX;
    [SerializeField] private float maxDodgeX;

    private Vector3 deltaPosition;

    private Vector3 currectPosition;

    public void SetCamera()
    {
        deltaPosition = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        currectPosition = target.position + deltaPosition;
        if (currectPosition.x <= minDodgeX)
        {
            currectPosition.x = minDodgeX;
        }
        else if (currectPosition.x >= maxDodgeX)
        {
            currectPosition.x = maxDodgeX;
        }
        transform.position = currectPosition;
    }

}
