using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour
{
    public int id;

    public int mana;
    public List<CardBehaviour> cardList;

    public float totalTwist = 10f;
    public float cardPerDistance = 4f;

    // Use this for initialization
    void Start()
    {
        // Batch();
    }

    void Update()
    {
        Batch();
    }

    public void AddCard(CardBehaviour _card)
    {
        cardList.Add(_card);
        Batch();
    }

    public void SubCard(int _index)
    {
        cardList.RemoveAt(_index);
        Batch();
    }

    void Batch()
    {
        // 20f for example, try various values
        float numberOfCards = cardList.Count;

        float twistPerCard = totalTwist / numberOfCards;

        print("per card " + twistPerCard);

        float startTwist = 0;
        float startX = 0;

        startTwist = twistPerCard * ((numberOfCards / 2) - 0.5f);
        startX = -cardPerDistance * ((numberOfCards / 2) - 0.5f);

        print("startTwist " + startTwist);

        for (int i = 0; i < numberOfCards; ++i)
        {
            float twistForThisCard = startTwist - (i * twistPerCard);

            print("twistForThisCard " + twistForThisCard);

            float scalingFactor = 0.25f;

            float nudgeThisCard = Mathf.Abs(twistForThisCard);

            nudgeThisCard *= scalingFactor;

            cardList[i].SetHandOrign(transform.TransformPoint(new Vector2(startX + (i * cardPerDistance), -nudgeThisCard)), Quaternion.Euler(0f, 0f, twistForThisCard));

            if (id != GameManager.instance.myID)
                cardList[i].Hide();
            else
                cardList[i].Open();

            cardList[i].SetHand(this);
            cardList[i].SetIndex(i);
        }

    }

}
