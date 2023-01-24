using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform followTarget;

    private void LateUpdate()
    {
        if (followTarget)
        {
            Vector3 newPosition = transform.position;

            if (followTarget.position.x > transform.position.x)
            {
                newPosition.x = followTarget.position.x;

                transform.position = newPosition;
            }
        }
    }
}
