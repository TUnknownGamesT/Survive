using System;
using UnityEngine;
public class PlayerRotation : MonoBehaviour
{
    public float distanceOfPoint;
    public Transform playerBody;
    public GameObject crossHair;

    public Vector3 mousePosition = Vector3.zero;


    void Update()
    {
        RotatePlayer();
    }
    private void RotatePlayer()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        // Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
        float t = Camera.main.transform.position.y / (Camera.main.transform.position.y - point.y);
        Vector3 finalPoint = new Vector3(t * (point.x - Camera.main.transform.position.x)
        + Camera.main.transform.position.x, 1, t * (point.z - Camera.main.transform.position.z)
        + Camera.main.transform.position.z);

        crossHair.transform.position = finalPoint;

        //Rotating the object to that point
        playerBody.LookAt(finalPoint, Vector3.up);
    }
}
