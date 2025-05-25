using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpawners : MonoBehaviour
{
    public Transform targetObject;
    public float rotationSpeed = 30f;
    public float radius = 5f;
    private float angle = 0f;

    void Start()
    {
        if (targetObject == null)
        {
            enabled = false;
            return;
        }
    }

    // Pohybuje spawnerem v kruhu kolem cílového objektu
    void Update()
    {
        if (targetObject == null)
        {
            return;
        }

        angle += rotationSpeed * Time.deltaTime;

        if (angle >= 360f)
        {
            angle -= 360f;
        }

        float x = targetObject.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = targetObject.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float y = transform.position.y;

        transform.position = new Vector3(x, y, z);
    }
}