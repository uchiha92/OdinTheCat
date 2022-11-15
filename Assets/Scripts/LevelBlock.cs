using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField]
    private Transform exitPoint;

    public Transform GetExitPoint()
    {
        return exitPoint;
    }
}
