using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset = new Vector3(0,0,-10);
    bool isActive;

    private void Awake()
    {
        offset.z = transform.position.z;
    }

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
            Vector3 xPos = followTarget.position + offset;
            xPos.y = transform.position.y;

            transform.position = xPos;
        }
    }
}
