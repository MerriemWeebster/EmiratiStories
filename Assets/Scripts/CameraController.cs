using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSens = 100f;
    public float sphereRadius = 5f;
    public float maxDistance = 10f;
    public LayerMask layer;
    public float gazeDelay = 0f;

    float xRotation = 0f;

    float currentHitDistance;
    Transform gazingAt;

    private void Start()
    {
        // Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHitDistance = maxDistance;
        gazingAt = null;
    }

    private void Update()
    {
        // Get input for movement of mouse (camera control)
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.fixedDeltaTime;

        // Calculations to move camera
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotating the player horizontally
        transform.parent.Rotate(Vector3.up * mouseX);

        CheckGaze();
    }

    // Checks if the camera is gazing on a gazable object
    void CheckGaze()
    {

        RaycastHit hit;
        if(Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxDistance, layer))
        {
            if(gazingAt != hit.transform)
            {
                if(gazingAt)
                {
                    gazingAt.GetComponent<GazableObject>().OnStopGaze();
                }

                gazingAt = hit.transform;
            }

            currentHitDistance = hit.distance;
            hit.transform.GetComponent<GazableObject>().OnGaze(gazeDelay);
        }
        else
        {
            if(!gazingAt) { return; }
            gazingAt.GetComponent<GazableObject>().OnStopGaze();
        }

    }

    // Shows the sphere cast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Debug.DrawLine(transform.position, transform.position + transform.forward * currentHitDistance, Color.white);
        Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, sphereRadius);
    }
}
