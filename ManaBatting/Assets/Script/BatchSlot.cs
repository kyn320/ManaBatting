using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchSlot : MonoBehaviour
{

    public BatchCardGroup group;

    public int index;
    public bool isMine = false;


    public void Batch(CardBehaviour _card)
    {
        group.batchList[index] = _card;
    }

    public void UnBatch() {
        group.batchList[index] = null;
    }

    public bool IsAllowBatch()
    {
        if (isMine && group.batchList[index] == null)
            return true;
        else
            return false;
    }

}
