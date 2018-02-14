using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    private static CardManager instance;

    public static CardManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int lastOpenIndex = -1;
    public HandCard[] handCards;
    public BatchCardGroup[] batchCardGroups;

    public CardBehaviour[] openCard;

    PhotonView photonView;

    void Awake()
    {
        instance = this;
        photonView = PhotonView.Get(this);
    }

    public void SetID(int _myID)
    {
        handCards[0].id = batchCardGroups[0].id = _myID;

        handCards[1].id = batchCardGroups[1].id = _myID == 0 ? 1 : 0;
    }

    public void GiveManaAll(int _mana)
    {
        for (int i = 0; i < handCards.Length; ++i)
        {
            handCards[i].mana += _mana;
        }
    }

    public void GiveManaToIndex(int _mana, int _index)
    {
        handCards[_index].mana += _mana;
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

    public void OpenCardToIndex(int _index)
    {
        for (int i = 0; i < batchCardGroups.Length; ++i)
        {
            batchCardGroups[i].OpenCard(0);
        }
    }

    public void SetOpenCard(int _id, CardBehaviour _card)
    {
        openCard[_id] = _card;
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
