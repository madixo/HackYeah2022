using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    public Transform cam;
    public float xMultipler;
    public float yMultipler;

    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(cam.transform.position.x * xMultipler, cam.transform.position.y * yMultipler);
    }
}
