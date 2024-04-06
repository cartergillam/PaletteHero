using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float zoomFactor = 1.0f;
    [SerializeField] float zoomSpeed = 5.0f;
    [SerializeField] float zoomedOutFactor = 2.0f; // New variable for zooming out

    private float originalSize = 0f;
    private Camera thisCamera;
    [SerializeField] private Transform player;
    private bool isZoomedOut = false;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        originalSize = thisCamera.orthographicSize;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            isZoomedOut = !isZoomedOut;
        }
        SetZoom(isZoomedOut ? zoomedOutFactor : zoomFactor);
    }

    private void SetZoom(float zoomFactor)
    {
        float targetSize = originalSize * zoomFactor;
        if (targetSize != thisCamera.orthographicSize)
        {
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }
}
