using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HandDeck : MonoBehaviour
{
    public int id;

    public int mana;

    public List<CardBehaviour> cardList;

    public float totalTwist = 10f;
    public float cardPerDistance = 4f;

    CardManager cardmanager;

    PhotonView photonView;

    void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    void Start()
    {
        cardmanager = CardManager.Instance;

        Batch();
    }

    private void Update()
    {
        Batch();
    }

    public void AddCard(Card card, bool isMine)
    {
        RPCAddCard(card.id, PhotonNetwork.LocalPlayer);
    }

    public void RPCAddCard(int cardId, Player player)
    {
        photonView.RPC("RemoteAddCard", RpcTarget.AllBuffered, cardId, player);
    }

    [PunRPC]
    public void RemoteAddCard(int cardId, Player player)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/CardObject"));
        g.transform.parent = transform;
        CardBehaviour cardBehaviour = g.GetComponent<CardBehaviour>();
        cardBehaviour.SetOwner(player);
        cardBehaviour.SetCard(CardDatabase.Instance.GetCardWithID(cardId));

        AddCard(cardBehaviour);
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

            cardList[i].Open();

            cardList[i].SetHand(this);
            cardList[i].SetIndex(i);
        }

    }



}
