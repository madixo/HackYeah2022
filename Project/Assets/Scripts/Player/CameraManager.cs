using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public float multipler;

    public float minX = -1000f;
    public float maxX = 1000f;
    public float minY = -1000f;
    public float maxY = 1000f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 p = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10), Time.deltaTime * multipler);
        if (p.x < minX) p.x = minX;
        if (p.x > maxX) p.x = maxX;
        if (p.y < minY) p.y = minY;
        if (p.y > maxY) p.y = maxY;

        transform.position = p;
    }
}
