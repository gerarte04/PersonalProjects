using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class CameraScript : MonoBehaviour
{
    const float dist = 20;

    const float rotSpeed = 400;
    
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(45, 45, 0);
        this.transform.position = -this.transform.forward * dist;
    }

    
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            float rotX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

            Quaternion r = this.transform.rotation;
            this.transform.rotation = Quaternion.Euler(r.eulerAngles.x - rotY, r.eulerAngles.y + rotX, 0);
            this.transform.position = -this.transform.forward * dist;
        }

        if (Input.GetMouseButtonDown(2))
        {
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(2))
            Cursor.visible = true;
    }
}
