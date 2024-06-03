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

    public void Die()
    {
        this.enabled = false;
    }
    private void RotatePlayer()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = distanceOfPoint;
        mousePosition =  Camera.main.ScreenToWorldPoint(mousePosition);

        mousePosition.y = playerBody.position.y;
        crossHair.transform.position = mousePosition;
        
        playerBody.LookAt(mousePosition);
    }
}
