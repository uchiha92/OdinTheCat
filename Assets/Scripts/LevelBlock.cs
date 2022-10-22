using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField]
    private Transform _exitPoint;
    // Start is called before the first frame update

    public Transform GetExitPoint()
    {
        return this._exitPoint;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
