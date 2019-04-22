using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HandCard : MonoBehaviour
{
    public int id;

    public int mana;

    public GameObject cardPrefab;
    public List<CardBehaviour> cardList;

    public float totalTwist = 10f;
    public float cardPerDistance = 4f;

    CardManager cardmanager;
    PhotonView photonView;

    void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    // Use this for initialization
    void Start()
    {
        cardmanager = CardManager.Instance;
        Batch();
    }

    public void DrawCard(Card card, bool isMine)
    {
        SendDrawCard(photonView.ViewID, card.id, PhotonNetwork.LocalPlayer);
    }

    public void SendDrawCard(int viewId, int cardId, Player player)
    {
        photonView.RPC("OnDrawCard", RpcTarget.AllBuffered, viewId, cardId, player);
    }

    [PunRPC]
    public void OnDrawCard(int viewId, int cardId, Player player)
    {
        GameObject g = Instantiate(cardPrefab);
        CardBehaviour cardBehaviour = g.GetComponent<CardBehaviour>();
        cardBehaviour.photonView.ViewID = viewId;
        cardBehaviour.photonView.TransferOwnership(player);
        cardBehaviour.SetCard(CardDatabase.Instance.GetCardWithID(cardId));
        cardBehaviour.SetIsMine();

        if (cardBehaviour.isMine)
            cardmanager.handCards[0].AddCard(cardBehaviour);
        else
            cardmanager.handCards[1].AddCard(cardBehaviour);
    }

    public void AddCard(CardBehaviour card)
    {
        cardList.Add(card);
        Batch();
    }

    public void SubCard(int index)
    {
        cardList.RemoveAt(index);
        Batch();
    }

    void Batch()
    {
        // 20f for example, try various values
        float numberOfCards = cardList.Count;

        float twistPerCard = totalTwist / numberOfCards;

        //print("per card " + twistPerCard);

        float startTwist = 0;
        float startX = 0;

        startTwist = twistPerCard * ((numberOfCards / 2) - 0.5f);
        startX = -cardPerDistance * ((numberOfCards / 2) - 0.5f);

        //print("startTwist " + startTwist);

        for (int i = 0; i < numberOfCards; ++i)
        {
            float twistForThisCard = startTwist - (i * twistPerCard);

            //print("twistForThisCard " + twistForThisCard);

            float scalingFactor = 0.25f;

            float nudgeThisCard = Mathf.Abs(twistForThisCard);

            nudgeThisCard *= scalingFactor;

            cardList[i].SetHandOrign(transform.TransformPoint(new Vector2(startX + (i * cardPerDistance), -nudgeThisCard)), Quaternion.Euler(0f, 0f, twistForThisCard));

            //if (id != GameManager.Instance.myID)
            //    cardList[i].Hide();
            //else
            //    cardList[i].Open();

            cardList[i].SetHand(this);
            cardList[i].SetIndex(i);
        }

    }

}
