using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionChanger : MonoBehaviour
{
    private bool lockedCamera = false;
    private Camera mainCam;
    [SerializeField] private float newCeilingBuffer, newFloorBuffer, newLeftBuffer, newRightbuffer;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            if (lockedCamera)
            {
                lockedCamera = false;
                mainCam.GetComponent<CameraFollow>().ResetAllBuffers();
            }
            else
            {
                lockedCamera = true;
                mainCam.GetComponent<CameraFollow>().SetNewBuffers(newCeilingBuffer, newFloorBuffer,
                    newLeftBuffer, newRightbuffer);
            }
        }
    }
}
