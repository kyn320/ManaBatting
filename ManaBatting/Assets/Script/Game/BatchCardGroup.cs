using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatchCardGroup : MonoBehaviour
{
    CardManager cardManager;

    public int id;

//    bool isOpen = false;

    public CardBehaviour[] batchList;
    public BatchSlot[] batchSlot;

    PhotonView photonView;

    void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    void Start()
    {
        cardManager = CardManager.Instance;
    }

    public void OpenCard(int index)
    {
        if (open != null)
            return;

        print("open to group = " + index);
        open = StartCoroutine(Open(index));
    }

    public void BatchCard(int index, CardBehaviour card)
    {
        batchList[index] = card;
    }

    public void SendSetBatchCard(int index, int handCardIndex, bool isControlHand)
    {
        photonView.RPC("OnSetBatchCard", RpcTarget.OthersBuffered, 1, handCardIndex, index, isControlHand);
    }

    [PunRPC]
    public void OnSetBatchCard(int playerID, int handCardIndex, int index, bool isControlHand)
    {
        if (handCardIndex == -1)
            cardManager.batchCardGroups[playerID].batchSlot[index].GetCard();
        else
            cardManager.handCards[playerID].cardList[handCardIndex].SetSlot(cardManager.batchCardGroups[playerID].batchSlot[index], false, isControlHand);
    }

    public void SendUnBatchCard(int index, bool isControlHand)
    {
        photonView.RPC("OnUnBatchCard", RpcTarget.OthersBuffered, 1, index, isControlHand);
    }

    [PunRPC]
    public void OnUnBatchCard(int playerID, int index, bool isControlHand)
    {
        cardManager.batchCardGroups[playerID].batchList[index].UnSlot(false, isControlHand);
    }

    Coroutine open = null;

    IEnumerator Open(int index)
    {
        Quaternion rot = Quaternion.Euler(0, 180f, 0);
        Transform cardTransform = batchList[index].transform;
        float checkRot = Quaternion.Angle(cardTransform.rotation, rot);
        while (checkRot > 0.1f)
        {
            cardTransform.localRotation = Quaternion.Lerp(cardTransform.localRotation, rot, Time.deltaTime * 20f);
            checkRot = Quaternion.Angle(cardTransform.localRotation, rot);
            print(checkRot);
            rot = Quaternion.Euler(0, 360f, 0);
            if (checkRot < 0.5f)
            {
                batchList[index].Open();
            }
            yield return null;
        }

        print("action card" + index);
        open = null;
        cardManager.SetOpenCard(id, batchList[index]);
    }

}
