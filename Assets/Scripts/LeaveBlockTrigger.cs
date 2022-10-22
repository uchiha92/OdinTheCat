using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveBlockTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelGenerator.Instance.AddNewBlock();
            LevelGenerator.Instance.RemoveOldBlocks();
        }
    }
}
