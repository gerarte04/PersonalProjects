using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceColor { Black, White, Yellow, Blue, Orange, Green, Red }

public class CubePart
{
    public int TurnIndex { get; private set; }

    public GameObject obj;
    List<FaceColor> colors; // up, down, right, left, front, back

    public CubePart(GameObject obj, List<FaceColor> colors)
    {
        this.obj = obj;
        this.colors = colors;
    }
}
