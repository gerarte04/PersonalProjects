using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    public string Name { get; private set; }

    public Vector3 offset;

    public List<GameObject> parts;   

    public Face(string name, Vector3 offset)
    {
        parts = new List<GameObject>();

        Name = name;
        this.offset = offset;
    }

    public void AddPart(GameObject part)
    {
        parts.Add(part);
    }
    public void PrintInfo()
    {
        string str = "";
        foreach(GameObject p in parts)
        {
            str += p.transform.localPosition.x.ToString();
            str += ",";
            str += p.transform.localPosition.y.ToString();
            str += ",";
            str += p.transform.localPosition.z.ToString();
            str += "    ";
        }

        Debug.Log(str);
    }
}
