using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public HandDeck handDeck;
    public List<Card> cardList;

    public bool isMine;

    void Start()
    {
        AddDeck();

        for (int i = 0; i < 12; ++i)
        {
            DrawCard();
        }
    }

    public void SetHandDeck(HandDeck handDeck)
    {
        this.handDeck = handDeck;
    }

    public void AddDeck()
    {
        for (int i = 0; i < 12; ++i)
        {
            cardList.Add(CardDatabase.Instance.itemList[i]);
        }
    }

    public void DrawCard()
    {
        if (cardList.Count < 1)
            return;

        Card card = cardList[0];
        handDeck.AddCard(card, isMine);
        cardList.RemoveAt(0);
    }

}
