using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlockManager : MonoBehaviour
{
    public Transform childBlock;
    public Transform parentBlock;


    public void Remove(Transform block)
    {
        if (block.GetComponent<BlockManager>().childBlock)
        {
            block.GetComponent<BlockManager>().childBlock.GetComponent<BlockManager>().parentBlock = null;
        }
        if (block.GetComponent<BlockManager>().parentBlock)
        {
            block.GetComponent<BlockManager>().parentBlock.GetComponent<BlockManager>().childBlock = null;
        }

        block.GetComponent<BlockManager>().childBlock = null;
        block.GetComponent<BlockManager>().parentBlock = null;
    }
    public void RemovePar(Transform block)
    {
        if (block.GetComponent<BlockManager>().parentBlock)
        {
            block.GetComponent<BlockManager>().parentBlock.GetComponent<BlockManager>().childBlock = null;
        }

        block.GetComponent<BlockManager>().parentBlock = null;
    }
    public void RemoveCh(Transform block)
    {
        if (block.GetComponent<BlockManager>().childBlock)
        {
            block.GetComponent<BlockManager>().childBlock.GetComponent<BlockManager>().parentBlock = null;
        }

        block.GetComponent<BlockManager>().childBlock = null;
    }
}
