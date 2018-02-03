using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour
{

    public int mana;
    public List<CardBehaviour> cardList;

    public float totalTwist = 10f;
    public float cardPerDistance = 4f;

    // Use this for initialization
    void Start()
    {
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

        if (numberOfCards % 2 == 1)
        {
            startTwist = twistPerCard * 2f;
            startX = -cardPerDistance * 2f;
        }
        else
        {
            startTwist = twistPerCard * 2.5f;
            startX = -cardPerDistance * 2.5f;
        }

        //print("startTwist " + startTwist);

        for (int i = 0; i < numberOfCards; ++i)
        {
            float twistForThisCard = startTwist - (i * twistPerCard);

            //print("twistForThisCard " + twistForThisCard);

            cardList[i].transform.rotation = Quaternion.Euler(0f, 0f, twistForThisCard);

            float scalingFactor = 0.25f;

            float nudgeThisCard = Mathf.Abs(twistForThisCard);

            nudgeThisCard *= scalingFactor;
            cardList[i].transform.localPosition = new Vector3(startX + (i * cardPerDistance), -nudgeThisCard, 0f);

            cardList[i].SetIndex(i);
        }

    }

}
