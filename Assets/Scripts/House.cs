using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Vector3 LeftEdgePosition { get; private set; }
    public Vector3 RightEdgePosition { get; private set; }

    private void Awake()
    {
        LeftEdgePosition = transform.position + new Vector3(-1, 0, 0);
        RightEdgePosition = transform.position + new Vector3(1, 0, 0);
    }
}
