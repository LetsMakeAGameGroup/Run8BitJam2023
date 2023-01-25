using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset = new Vector3(0,0,-10);
    bool isActive;

    private void Start()
    {
        GameMode.Instance.onGameStart += StartCameraMovement;
    }

    void StartCameraMovement() 
    {
        isActive = true;
    }

    private void LateUpdate()
    {
        if (!isActive) { return; }

        if (followTarget)
        {
            transform.position = followTarget.position + offset;
        }
    }
}
