using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    private const int InitialBlocks = 2;

    private bool _isGeneratingInitialBlocks;
    public static LevelGenerator Instance;
    [SerializeField]
    private List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();
    [SerializeField] 
    private List<LevelBlock> allLevelBlocks = new List<LevelBlock>();
    [SerializeField]
    private Transform levelInitialPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    public void GenerateInitialBlocks()
    {
        _isGeneratingInitialBlocks = true;
        for (int i = 0; i < InitialBlocks + 1; i++)
        {
            AddNewBlock();
        }
        _isGeneratingInitialBlocks = false;
    }

    public void AddNewBlock()
    {
        int randomIndex;

        if (_isGeneratingInitialBlocks)
        {
            randomIndex = 0;
        }
        else
        { 
            randomIndex = Random.Range(0, allLevelBlocks.Count);  
        }

        LevelBlock block = (LevelBlock) Instantiate(allLevelBlocks[randomIndex]);
        block.transform.SetParent(this.transform, false);
        Vector3 blockPosition = Vector3.zero;
        if (currentLevelBlocks.Count == 0)
        {
            blockPosition = levelInitialPoint.position;
        }
        else
        {
            blockPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].GetExitPoint().position;
        }

        block.transform.position = blockPosition;
        currentLevelBlocks.Add(block);
    }

    public void RemoveOldBlocks()
    {
        LevelBlock block = currentLevelBlocks[0];
        currentLevelBlocks.Remove(block);
        Destroy(block.gameObject);
    }

    public void RemoveAllTheBlocks()
    {
        while (currentLevelBlocks.Count > 0)
        {
            RemoveOldBlocks();
        }
    }
}
