using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public static class CubeInitialize
{
    public static void CreateNewCube(ref List<Face> faces, int size, Transform baseObj)
    {
        float edgeSize = 2 * 3 / (float)size;
        float partScale = 3 / (float)size;

        faces = new List<Face>(); // up to down, right to left, front to back

        for (int i = 0; i < 3; i++)
        {
            string dimension = "sarai";
            Vector3 devCoeffs = new Vector3(0, 0, 0);

            switch (i)
            {
                case 0:
                    dimension = "Y";
                    devCoeffs = new Vector3(0, 1, 0);
                    break;
                case 1:
                    dimension = "X";
                    devCoeffs = new Vector3(1, 0, 0);
                    break;
                case 2:
                    dimension = "Z";
                    devCoeffs = new Vector3(0, 0, 1);
                    break;
            }

            for (int j = -size / 2; j <= (int)(size / 2); j += (size % 2 == 0 && j == -1) ? 2 : 1)
            {
                float deviation = 0;
                if (size % 2 == 1)
                    deviation = edgeSize * j;
                else
                    deviation = edgeSize * ((j < 0) ? j + 0.5f : j - 0.5f);

                Vector3 offset = new Vector3(deviation * devCoeffs.x, deviation * devCoeffs.y, deviation * devCoeffs.z);

                Face face = new Face("face" + dimension + j.ToString(), offset);
                faces.Add(face);
            }
        }

        using (StreamReader sr = new StreamReader("Assets/partsPositions.txt"))
        {
            string str;

            while ((str = sr.ReadLine()) != null)
            {
                string[] info = str.Split(' ');
                string name = info[0];
                Vector3 devCoeffs = new Vector3(Convert.ToInt32(info[1]), Convert.ToInt32(info[2]), Convert.ToInt32(info[3]));

                var colors = new List<FaceColor>();
                for (int i = 4; i <= info.Length - 1; i++)
                    colors.Add((FaceColor)Enum.Parse(typeof(FaceColor), info[i]));


                string nameInfo = name.Split('_')[0];
                string path = "sarai";
                List<Vector3> relPositions = new List<Vector3>();

                switch (nameInfo)
                {
                    case "empty":
                        path = name;

                        for (int i = -size / 2 + 1; i <= (int)(size / 2) - 1; i += (size % 2 == 0 && i == -1) ? 2 : 1)
                            for (int j = -size / 2 + 1; j <= (int)(size / 2) - 1; j += (size % 2 == 0 && j == -1) ? 2 : 1)
                                for (int k = -size / 2 + 1; k <= (int)(size / 2) - 1; k += (size % 2 == 0 && k == -1) ? 2 : 1)
                                    relPositions.Add(new Vector3(i, j, k));

                        break;

                    case "center":
                        path = "centers/" + name;

                        for (int i = -size / 2 + 1; i <= (int)(size / 2) - 1; i += (size % 2 == 0 && i == -1) ? 2 : 1)
                            for (int j = -size / 2 + 1; j <= (int)(size / 2) - 1; j += (size % 2 == 0 && j == -1) ? 2 : 1)
                            {
                                Vector3 relPos = new Vector3(0, 0, 0);

                                if (devCoeffs.x != 0)
                                    relPos = new Vector3(devCoeffs.x * (int)(size / 2), i, j);
                                else if (devCoeffs.y != 0)
                                    relPos = new Vector3(i, devCoeffs.y * (int)(size / 2), j);
                                else if (devCoeffs.z != 0)
                                    relPos = new Vector3(i, j, devCoeffs.z * (int)(size / 2));

                                relPositions.Add(relPos);
                            }

                        break;

                    case "edge":
                        path = "edges/" + name;

                        for (int i = -size / 2 + 1; i <= (int)(size / 2) - 1; i += (size % 2 == 0 && i == -1) ? 2 : 1)
                        {
                            Vector3 relPos = new Vector3(0, 0, 0);

                            if (devCoeffs.x == 0)
                                relPos = new Vector3(i, devCoeffs.y * (int)(size / 2), devCoeffs.z * (int)(size / 2));
                            else if (devCoeffs.y == 0)
                                relPos = new Vector3(devCoeffs.x * (int)(size / 2), i, devCoeffs.z * (int)(size / 2));
                            else if (devCoeffs.z == 0)
                                relPos = new Vector3(devCoeffs.x * (int)(size / 2), devCoeffs.y * (int)(size / 2), i);

                            relPositions.Add(relPos);
                        }
                        break;

                    case "corner":
                        path = "corners/" + name;

                        relPositions.Add(new Vector3(devCoeffs.x * (int)(size / 2), devCoeffs.y * (int)(size / 2), devCoeffs.z * (int)(size / 2)));

                        break;
                }

                GameObject myPrefab = Resources.Load<GameObject>(path);

                foreach (Vector3 pos in relPositions)
                {
                    float deviationX = 0;
                    if (size % 2 == 1)
                        deviationX = edgeSize * pos.x;
                    else
                        deviationX = edgeSize * ((pos.x < 0) ? pos.x + 0.5f : pos.x - 0.5f);

                    float deviationY = 0;
                    if (size % 2 == 1)
                        deviationY = edgeSize * pos.y;
                    else
                        deviationY = edgeSize * ((pos.y < 0) ? pos.y + 0.5f : pos.y - 0.5f);

                    float deviationZ = 0;
                    if (size % 2 == 1)
                        deviationZ = edgeSize * pos.z;
                    else
                        deviationZ = edgeSize * ((pos.z < 0) ? pos.z + 0.5f : pos.z - 0.5f);

                    var prefab = MonoBehaviour.Instantiate(myPrefab, baseObj);
                    prefab.transform.localPosition = new Vector3(deviationX, deviationY, deviationZ);
                    prefab.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    prefab.transform.localScale = new Vector3(partScale, partScale, partScale);

                    foreach (Face face in faces.Where(f => f.Name.Contains("X")))
                        if (Convert.ToInt32(face.Name.Replace("face", "").Replace("X", "")) == pos.x)
                            face.AddPart(prefab);
                            // face.AddPart(new CubePart(prefab, colors));

                    foreach (Face face in faces.Where(f => f.Name.Contains("Y")))
                        if (Convert.ToInt32(face.Name.Replace("face", "").Replace("Y", "")) == pos.y)
                            face.AddPart(prefab);
                            // face.AddPart(new CubePart(prefab, colors));

                    foreach (Face face in faces.Where(f => f.Name.Contains("Z")))
                        if (Convert.ToInt32(face.Name.Replace("face", "").Replace("Z", "")) == pos.z)
                            face.AddPart(prefab);
                            // face.AddPart(new CubePart(prefab, colors));
                }
            }
        }
    }
}
