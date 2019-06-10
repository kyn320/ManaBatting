using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardManager : Singleton<CardManager>
{
    public int lastOpenIndex = -1;
    public HandDeck[] handCards;
    public BatchCardGroup[] batchCardGroups;

    public CardBehaviour[] openCard;

  //  PhotonView photonView;

    protected override void Awake()
    {
        base.Awake();
        FindHandCards();
  //      photonView = PhotonView.Get(this);
    }


    void FindHandCards() {
        handCards = GameObject.FindObjectsOfType<HandDeck>();
    }

    public void SetID(int myID)
    {
        handCards[0].id = batchCardGroups[0].id = myID;

        handCards[1].id = batchCardGroups[1].id = myID == 0 ? 1 : 0;
    }

    public void GiveManaAll(int mana)
    {
        for (int i = 0; i < handCards.Length; ++i)
        {
            handCards[i].mana += mana;
        }
    }

    public void GiveManaToIndex(int mana, int index)
    {
        handCards[index].mana += mana;
    }

    public void OpenCard()
    {
        ++lastOpenIndex;

        if (lastOpenIndex > 4)
        {
            print("end all open >> new betting");
            return;
        }

        for (int i = 0; i < batchCardGroups.Length; ++i)
        {
            batchCardGroups[i].OpenCard(lastOpenIndex);
        }
    }

    public void OpenCardToIndex(int index)
    {
        for (int i = 0; i < batchCardGroups.Length; ++i)
        {
            batchCardGroups[i].OpenCard(0);
        }
    }

    public void SetOpenCard(int id, CardBehaviour card)
    {
        openCard[id] = card;
        if (openCard[0] != null && openCard[1] != null)
        {
            RunAction();
        }
        else
            print("is open null");
    }

    public void RunAction()
    {
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        print("action run");
        WaitForAction waitAction = new WaitForAction();
        if (openCard[0].card.type > openCard[1].card.type)
        {
            waitAction.isFinish = false;
            openCard[0].SetEndAction(waitAction.Finish);
            openCard[0].StartAction();
            yield return waitAction;
            waitAction.isFinish = false;
            openCard[1].SetEndAction(waitAction.Finish);
            openCard[1].StartAction();
            yield return waitAction;
        }
        else if (openCard[0].card.type < openCard[1].card.type)
        {
            waitAction.isFinish = false;
            openCard[1].SetEndAction(waitAction.Finish);
            openCard[1].StartAction();
            yield return waitAction;
            waitAction.isFinish = false;
            openCard[0].SetEndAction(waitAction.Finish);
            openCard[0].StartAction();
            yield return waitAction;
        }
        else {

        }
        print("all out");
        openCard[0] = openCard[1] = null;
        OpenCard();
    }

}
