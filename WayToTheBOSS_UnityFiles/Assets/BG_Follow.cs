using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Follow : MonoBehaviour
{
    private Transform cam;
    public float lengthX;
    public float lengthY;

    void Start()
    {
        cam = Camera.main.gameObject.transform;
    }

    void Update()
    {
        if (cam.position.x >= transform.position.x + lengthX)
        {
            transform.position = new Vector2(cam.position.x + lengthX, transform.position.y);
        }
        else if (cam.position.x <= transform.position.x - lengthX)
        {
            transform.position = new Vector2(cam.position.x - lengthX, transform.position.y);
        }

        if (cam.position.y >= transform.position.y + lengthY)
        {
            transform.position = new Vector2(transform.position.x, cam.position.y + lengthY);
        }
        else if (cam.position.y <= transform.position.y - lengthY)
        {
            transform.position = new Vector2(transform.position.x, cam.position.y - lengthY);
        }

    }
}
