using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchSlot : MonoBehaviour
{

    public BatchCardGroup group;

    public int index;
    public bool isMine = false;

    public CardBehaviour GetCard()
    {
        return group.batchList[index];
    }

    public void Batch(CardBehaviour card)
    {
        group.BatchCard(index, card);
    }

    public void SendSetBatch(CardBehaviour card, bool isControlHand)
    {
        group.SendSetBatchCard(index, card.index, isControlHand);
    }

    public void UnBatch()
    {
        group.BatchCard(index, null);
    }

    public void SendUnBatch(bool isControlHand)
    {
        group.SendUnBatchCard(index, isControlHand);
    }

    public bool IsAllowBatch()
    {
        if (isMine)
            return true;
        else
            return false;
    }

    public bool IsEmpty()
    {
        if (group.batchList[index] == null)
            return true;
        else
            return false;
    }
}
