using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float ceilingBuffer, groundBuffer, leftBuffer, rightBuffer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position + offset, followSpeed);
        Buffer();
    }

    private void Buffer()
    {
        if(transform.position.x >= rightBuffer)
        {
            transform.position = new Vector3(rightBuffer, transform.position.y, 0) + offset;
        }
        if (transform.position.x <= leftBuffer)
        {
            transform.position = new Vector3(leftBuffer, transform.position.y, 0) + offset;
        }
        if (transform.position.y >= ceilingBuffer)
        {
            transform.position = new Vector3(transform.position.x, ceilingBuffer, 0) + offset;
        }
        if (transform.position.y <= groundBuffer)
        {
            transform.position = new Vector3(transform.position.x, groundBuffer, 0) + offset;
        }
    }
}
