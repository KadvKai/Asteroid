using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private void Awake()
    {
        var camera = GetComponent<Camera>();
        var positionY = camera.orthographicSize;
        var positionX = positionY * camera.aspect;
        transform.position = new Vector3(positionX, positionY, transform.position.z);
    }
}
