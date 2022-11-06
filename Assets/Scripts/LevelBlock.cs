using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField]
    private Transform _exitPoint;

    public Transform GetExitPoint()
    {
        return this._exitPoint;
    }
}
