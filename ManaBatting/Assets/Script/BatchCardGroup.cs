using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchCardGroup : MonoBehaviour
{
    CardManager cardManager;

    public int id;

    bool isOpen = false;

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

    public void OpenCard(int _index)
    {
        if (open != null)
            return;

        print("open to group = " + _index);
        open = StartCoroutine(Open(_index));
    }

    public void BatchCard(int _index, CardBehaviour _card)
    {
        batchList[_index] = _card;
    }

    public void SendSetBatchCard(int _index, int _handCardIndex, bool _isControlHand)
    {
        photonView.RPC("OnSetBatchCard", PhotonTargets.OthersBuffered, 1, _handCardIndex, _index, _isControlHand);
    }

    [PunRPC]
    public void OnSetBatchCard(int _playerID, int _handCardIndex, int _index, bool _isControlHand)
    {
        if (_handCardIndex == -1)
            cardManager.batchCardGroups[_playerID].batchSlot[_index].GetCard();
        else
            cardManager.handCards[_playerID].cardList[_handCardIndex].SetSlot(cardManager.batchCardGroups[_playerID].batchSlot[_index], false, _isControlHand);
    }

    public void SendUnBatchCard(int _index, bool _isControlHand)
    {
        photonView.RPC("OnUnBatchCard", PhotonTargets.OthersBuffered, 1, _index, _isControlHand);
    }

    [PunRPC]
    public void OnUnBatchCard(int _playerID, int _index, bool _isControlHand)
    {
        cardManager.batchCardGroups[_playerID].batchList[_index].UnSlot(false, _isControlHand);
    }

    Coroutine open = null;

    IEnumerator Open(int _index)
    {
        Quaternion rot = Quaternion.Euler(0, 180f, 0);
        Transform cardTransform = batchList[_index].transform;
        float checkRot = Quaternion.Angle(cardTransform.rotation, rot);
        while (checkRot > 0.1f)
        {
            cardTransform.localRotation = Quaternion.Lerp(cardTransform.localRotation, rot, Time.deltaTime * 20f);
            checkRot = Quaternion.Angle(cardTransform.localRotation, rot);
            print(checkRot);
            rot = Quaternion.Euler(0, 360f, 0);
            if (checkRot < 0.5f)
            {
                batchList[_index].Open();
            }
            yield return null;
        }

        print("action card" + _index);
        open = null;
        cardManager.SetOpenCard(id, batchList[_index]);
    }

}
