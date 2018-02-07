using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public HandCard hand;
    public List<Card> cardList;

    public bool isMine;

    void Start()
    {
        for (int i = 0; i < 5; ++i)
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
