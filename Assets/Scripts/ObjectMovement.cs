using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private bool _moveToEnd;
    private bool _moveToOrigin;

    [SerializeField]
    private bool shouldMove;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform originPosition;
    [SerializeField]
    private Transform endPosition;
    
    void Start()
    {
        _moveToEnd = true;
        _moveToOrigin = false;
    }
    
    void Update()
    {
        if (shouldMove)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        float distanceToEnd = Vector2.Distance(transform.position, endPosition.position);
        float distanceToOrigin = Vector2.Distance(transform.position, originPosition.position);

        if (distanceToEnd > Mathf.Epsilon && _moveToEnd)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition.position, speed * Time.deltaTime);
            if (distanceToEnd < 0.3f)
            {
                _moveToEnd = false;
                _moveToOrigin = true;
            }
        }
        
        if (distanceToOrigin > Mathf.Epsilon && _moveToOrigin)
        {
            transform.position = Vector2.MoveTowards(transform.position, originPosition.position, speed * Time.deltaTime);
            if (distanceToOrigin < 0.3f)
            {
                _moveToEnd = true;
                _moveToOrigin = false;
            }
        }
    }
}
