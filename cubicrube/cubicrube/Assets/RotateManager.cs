using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RotateManager
{
    public bool IsRotating { get; private set; } = false;

    GameObject gObj;
    GameObject baseObj;

    string dimension = "";
    int direction = 1;

    float acc = 0, vel = 0;
    bool accFlipped = false;

    public RotateManager(GameObject bObj) 
    {
        baseObj = bObj;
    }

    public void InstantRotateQueue(Dictionary<GameObject, object[]> rotates)
    {
        var dims = new List<string> { "X", "Y", "Z" };
        
        foreach (GameObject rotObj in rotates.Keys)
        {
            string dim = rotates[rotObj][0].ToString();
            int dir = Convert.ToInt32(rotates[rotObj][1]);

            IncreaseAngle(rotObj, 90 * dir, dim);

            foreach (Transform obj in rotObj.GetComponentsInChildren<Transform>())
                if (obj.parent == rotObj.transform)
                    obj.parent = baseObj.transform;
            MonoBehaviour.Destroy(rotObj);

            baseObj.GetComponent<CubeTurning>().UpdateDimensions(dims.Where(x => x != dim));
        }
    }

    public void StartNewRotation(GameObject rotObj, string dim, int dir, float a)
    {
        if (!IsRotating)
        {
            gObj = rotObj;
            dimension = dim;
            direction = dir;
            acc = a;

            IsRotating = true;
            vel = 0;
            accFlipped = false;
        }
    }

    public void FrameRotate()
    {
        if (IsRotating)
        {
            Quaternion r = gObj.transform.localRotation;

            if (GetAngle(gObj, dimension) * direction > 45 + 2 && !accFlipped)
            {
                acc *= -1;
                accFlipped = true;
            }

            vel += acc * Time.deltaTime;

            IncreaseAngle(gObj, vel * direction, dimension);
            if (GetAngle(gObj, dimension) * direction >= 90 - 2)
            {
                SetAngle(gObj, 90 * direction, dimension);
                IsRotating = false;

                foreach (Transform obj in gObj.GetComponentsInChildren<Transform>())
                    if(obj.parent == gObj.transform)
                        obj.parent = baseObj.transform;
                MonoBehaviour.Destroy(gObj);

                var dims = new List<string> { "X", "Y", "Z" };
                baseObj.GetComponent<CubeTurning>().UpdateDimensions(dims.Where(x => x != dimension));
            }
        }
    }

    private void IncreaseAngle(GameObject go, float angle, string dim)
    {
        Quaternion r = go.transform.localRotation;
        switch(dimension)
        {
            case "X":
                Quaternion endx = Quaternion.Euler(r.eulerAngles.x + angle, r.eulerAngles.y, r.eulerAngles.z);
                go.transform.localRotation = Quaternion.RotateTowards(r, endx, 90);
                break;

            case "Y":
                Quaternion endy = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y + angle, r.eulerAngles.z);
                go.transform.localRotation = Quaternion.RotateTowards(r, endy, 90);
                break;

            case "Z":
                Quaternion endz = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z - angle);
                go.transform.localRotation = Quaternion.RotateTowards(r, endz, 90);
                break;
        }
    }

    private void SetAngle(GameObject go, float angle, string dim)
    {
        Quaternion r = go.transform.localRotation;
        switch (dimension)
        {
            case "X":
                Quaternion endx = Quaternion.Euler(angle, r.eulerAngles.y, r.eulerAngles.z);
                go.transform.localRotation = Quaternion.RotateTowards(r, endx, 90);
                break;

            case "Y":
                Quaternion endy = Quaternion.Euler(r.eulerAngles.x, angle, r.eulerAngles.z);
                go.transform.localRotation = Quaternion.RotateTowards(r, endy, 90);
                break;

            case "Z":
                Quaternion endz = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, -angle);
                go.transform.localRotation = Quaternion.RotateTowards(r, endz, 90);
                break;
        }
    }

    private float GetAngle(GameObject go, string dim)
    {
        switch (dimension)
        {
            case "X":
                float ax = go.transform.eulerAngles.x;
                if (ax > 180)
                    return -(360 - ax);
                else
                    return ax;

            case "Y":
                float ay = go.transform.eulerAngles.y;
                if (ay > 180)
                    return -(360 - ay);
                else
                    return ay;

            case "Z":
                float az = go.transform.eulerAngles.z;
                if (az > 180)
                    return 360 - az;
                else
                    return -az;

            default:
                return 0;
        }
    }
}
