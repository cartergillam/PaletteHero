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
    private float attackShakeFactor = 0.6f;
    private float attackShakeDuration = 0.02f; // Duration of the attack shake effect

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

    public void AttackShake()
    {
        StartCoroutine(PerformAttackShake());
    }

    private IEnumerator PerformAttackShake()
    {
        if (isZoomedOut)
        {
            attackShakeFactor = 1.1f;
        }
        else {attackShakeFactor = 0.6f;}

            float targetSize = originalSize * attackShakeFactor;
            float elapsedTime = 0f;

            // Interpolate the camera size towards the target size over the specified duration
            while (elapsedTime < attackShakeDuration)
            {
                thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, targetSize, elapsedTime / attackShakeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the camera size is set to the target size at the end of the duration
            thisCamera.orthographicSize = targetSize;
    }
}
