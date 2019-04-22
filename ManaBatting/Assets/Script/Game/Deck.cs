using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public HandCard hand;
    public List<Card> cardList;

    public bool isMine;

    public void AddDeck()
    {
        for (int i = 0; i < 12; ++i)
        {
            cardList.Add(CardDatabase.Instance.dataList[i]);
        }
    }

    void Start()
    {
        AddDeck();

        for (int i = 0; i < 12; ++i)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (cardList.Count < 1)
            return;

        Card card = cardList[0];
        hand.DrawCard(card, isMine);
        cardList.RemoveAt(0);
    }

}
