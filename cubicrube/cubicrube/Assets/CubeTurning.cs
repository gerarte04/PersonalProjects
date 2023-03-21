using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeTurning : MonoBehaviour
{
    public List<Face> cubeFaces;

    const int cubeSize = 7;
    float edgeSize = 2 * 3 / (float)cubeSize;

    string rotDimension = "";
    int rotDirection = 1;

    RotateManager rotManager;

    void Start()
    {
        CubeInitialize.CreateNewCube(ref cubeFaces, cubeSize, this.transform);

        rotManager = new RotateManager(this.gameObject);
    }

    public void MixButtonPress()
    {
        var dirs = new List<int> { -1, 1 };

        var rotates = new Dictionary<GameObject, object[]>();
        for (int i = 0; i < 20; i++)
        {
            var face = cubeFaces[Random.Range(0, cubeFaces.Count)];

            var rotObj = new GameObject();
            rotObj.transform.parent = this.transform;
            rotObj.transform.localPosition = face.offset;

            foreach (GameObject go in face.parts)
                go.transform.parent = rotObj.transform;

            object[] pars = new object[2];
            pars[0] = face.Name[4];
            pars[1] = dirs[Random.Range(0, 2)];
            rotates[rotObj] = pars;
        }

        rotManager.InstantRotateQueue(rotates);

    }

    public void UpdateDimensions(IEnumerable<string> dims)
    {
        foreach (string dim in dims)
            foreach (Face face in cubeFaces.Where(f => f.Name[4].ToString() == dim))
                UpdateFace(face, dim);
    }

    public void UpdateFace(Face face, string dim)
    {
        float startCoord = edgeSize * (int)(cubeSize / 2 + 1);
        face.parts.Clear();

        switch(dim)
        {
            case "X":
                for(float y = startCoord; y >= -startCoord; y -= edgeSize)
                {
                    
                    Vector3 point = new Vector3(face.offset.x, y, startCoord + edgeSize);
                    RaycastHit[] hit = Physics.RaycastAll(point, -transform.forward, edgeSize * (cubeSize + 2));

                    for (int i = 0; i < hit.Length; i++)
                    {
                        face.AddPart(hit[i].transform.gameObject);
                        // face.parts[i].obj = hit[i].transform.gameObject;
                    }
                }
                break;

            case "Y":
                for (float z = startCoord; z >= -startCoord; z -= edgeSize)
                {

                    Vector3 point = new Vector3(startCoord + edgeSize, face.offset.y, z);
                    RaycastHit[] hit = Physics.RaycastAll(point, -transform.right, edgeSize * (cubeSize + 2));

                    for (int i = 0; i < hit.Length; i++)
                    {
                        face.AddPart(hit[i].transform.gameObject);
                        // face.parts[i].obj = hit[i].transform.gameObject;
                    }
                }
                break;

            case "Z":
                for (float y = startCoord; y >= -startCoord; y -= edgeSize)
                {

                    Vector3 point = new Vector3(startCoord + edgeSize, y, face.offset.z);
                    RaycastHit[] hit = Physics.RaycastAll(point, -transform.right, edgeSize * (cubeSize + 2));

                    for (int i = 0; i < hit.Length; i++)
                    {
                        face.AddPart(hit[i].transform.gameObject);
                        // face.parts[i].obj = hit[i].transform.gameObject;
                    }
                }
                break;

            default:
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
            rotDimension = "Z";
        if (Input.GetKeyDown("w"))
            rotDimension = "Y";
        if (Input.GetKeyDown("e"))
            rotDimension = "X";

        if (Input.GetKeyUp("q"))
            if (rotDimension == "Z")
                rotDimension = "";
        if (Input.GetKeyUp("w"))
            if (rotDimension == "Y")
                rotDimension = "";
        if (Input.GetKeyUp("e"))
            if (rotDimension == "X")
                rotDimension = "";

        if (Input.GetKeyDown("right shift") || Input.GetKeyDown("left shift"))
            rotDirection = -1;
        else if (Input.GetKeyUp("right shift") || Input.GetKeyUp("left shift"))
            rotDirection = 1;

        rotManager.FrameRotate();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 500) && !rotManager.IsRotating)
            {
                foreach(Face face in cubeFaces.Where(f => f.Name[4].ToString() == rotDimension))
                {
                    bool IsInFace = false;
                    foreach(GameObject cp in face.parts)
                    {
                        if (cp == hit.transform.gameObject)
                        {
                            IsInFace = true;
                        }
                    }

                    if (IsInFace)
                    {
                        Debug.Log(face.Name);
                        var rotObj = new GameObject();
                        rotObj.transform.parent = this.transform;
                        rotObj.transform.localPosition = face.offset;

                        foreach (GameObject cp in face.parts)
                            cp.transform.parent = rotObj.transform;

                        rotManager.StartNewRotation(rotObj, rotDimension, rotDirection, 15);
              
                        break;
                    }
                }
            }
        }
    }
}
