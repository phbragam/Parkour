using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    Quaternion targetRotation;
    CameraController cameraController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);


        if (moveAmount > 0)
        {
            // without normalizing the diagonal movement will have bigger values tha horizontal and vertical (over than 1 )
            var moveInput = (new Vector3(h, 0, v)).normalized;

            // make movement be in the direction of the camera (ignoring rotation X)
            var moveDir = cameraController.PlanarRotation * moveInput;

            // frame rate independent because of Time.deltaTime
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            // make character face move direction
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }
}
