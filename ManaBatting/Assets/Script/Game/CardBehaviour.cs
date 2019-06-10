using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class CardBehaviour : MonoBehaviour
{
    public int index;

    public Card card;

    Vector2 orignPos;
    Quaternion orignRot;

    public SpriteRenderer frontSprRenderer, middleSprRenderer, backSprRenderer;

    public HandDeck hand;
    public BatchSlot slot;

    public bool isMine = false;

    public Canvas infoView;
    public Text cardNameText, cardCostText, cardTypeText, cardExplainText;

    public PhotonView photonView;

    public UnityAction endAction;

    void Awake()
    {
        photonView = PhotonView.Get(this);

    }

    void Start()
    {

    }

    public void SetHand(HandDeck hand)
    {
        this.hand = hand;
    }

    public void SetHandOrign(Vector2 pos, Quaternion rot)
    {
        orignPos = pos;
        orignRot = rot;

        SetReplaceOrign();
    }

    public void SetIndex(int index)
    {
        this.index = index;
        SetSortingOrder();
    }

    public void SetSortingOrder()
    {
        frontSprRenderer.sortingOrder = index * 10 + +6;
        middleSprRenderer.sortingOrder = index * 10 + 5;
        backSprRenderer.sortingOrder = index * 10 + 4;
        infoView.sortingOrder = index * 10 + 7;
    }

    public void SetSortingOrder(int order)
    {
        frontSprRenderer.sortingOrder = order * 10 + +6;
        middleSprRenderer.sortingOrder = order * 10 + 5;
        backSprRenderer.sortingOrder = order * 10 + 4;
        infoView.sortingOrder = order * 10 + 7;
    }

    public void SetCard(Card card)
    {
        this.card = card;

        ViewInfo();
    }

    public void SetIsMine()
    {
        isMine = photonView.IsMine;
    }

    public void SetOwner(Player player)
    {
        photonView.TransferOwnership(player);

        isMine = PhotonNetwork.LocalPlayer.Equals(player);
    }


    public void Open()
    {
        frontSprRenderer.sprite = card.frontSprite;
        middleSprRenderer.sprite = card.middleSprite;
        backSprRenderer.sprite = card.backSprite;
        infoView.gameObject.SetActive(true);
    }

    public void Hide()
    {
        frontSprRenderer.sprite = null;
        middleSprRenderer.sprite = card.hideSprite;
        backSprRenderer.sprite = null;
        infoView.gameObject.SetActive(false);
    }

    Coroutine movePosition = null;

    public void SetReplaceOrign()
    {
        if (movePosition != null)
        {
            StopCoroutine(movePosition);
        }
        movePosition = StartCoroutine(ReplceOrign());
    }

    IEnumerator ReplceOrign()
    {
        float dist = 1, rot = 1f;
        while (dist > 0.01f || rot > 0.01f)
        {
            dist = Vector2.SqrMagnitude(orignPos - (Vector2)transform.position);
            rot = Quaternion.Angle(transform.rotation, orignRot);
            transform.position = Vector2.Lerp(transform.position, orignPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, orignRot, Time.deltaTime * 10f);
            yield return null;
        }
        movePosition = null;
    }

    public void SetReplaceSlot()
    {
        SetTargetPlace(slot.transform);
    }

    public void SetSlot(BatchSlot slot, bool isSend, bool isControlHand)
    {
        this.slot = slot;
        slot.Batch(this);

        if (isSend)
            slot.RPCSetBatch(this, isControlHand);

        if (isControlHand)
            SubHand();

        index = -1;
        SetTargetPlace(slot.transform);
    }

    public void UnSlot(bool isSend, bool isControlHand)
    {
        if (slot == null)
            return;

        slot.UnBatch();

        if (isSend)
            slot.RPCUnBatch(isControlHand);

        if (isControlHand)
            AddHand();

        slot = null;
    }

    public void AddHand()
    {
        hand.AddCard(this);
    }

    public void RPCAddHand()
    {
        photonView.RPC("RemoteAddHand", RpcTarget.OthersBuffered, null);
    }

    [PunRPC]
    public void RemoteAddHand()
    {
        print("add hand");
        hand.AddCard(this);
    }

    public void SubHand()
    {
        hand.SubCard(index);
    }

    public void RPCSubHand()
    {
        photonView.RPC("RemoteSubHand", RpcTarget.OthersBuffered, null);
    }

    [PunRPC]
    public void RemoteSubHand()
    {
        print("sub hand");
        hand.SubCard(index);
    }

    public void SetTargetPlace(Transform target)
    {
        if (movePosition != null)
        {
            StopCoroutine(movePosition);
        }

        movePosition = StartCoroutine(TargetPlace(target));
    }

    IEnumerator TargetPlace(Transform target)
    {
        float dist = 1, rot = 1f;

        while (dist > 0.01f || rot > 0.01f)
        {
            dist = Vector2.SqrMagnitude(target.position - transform.position);
            rot = Quaternion.Angle(transform.rotation, target.rotation);
            transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 10f);
            yield return null;
        }
        movePosition = null;
    }

    public void ViewInfo()
    {
        infoView.gameObject.SetActive(true);
        cardNameText.text = card.name;
        cardCostText.text = card.cost.ToString();
        cardTypeText.text = card.type.ToString();
        cardExplainText.text = card.explain;
    }

    public void StartAction()
    {
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        WaitForAction waitAction = new WaitForAction();
        //effect.SetEndAction(waitAction.Finish);
        //effect.StartAction();
        yield return waitAction;
        print("card behaviour out");
        endAction.Invoke();
    }

    public void SetEndAction(UnityAction action)
    {
        endAction = action;
    }

}
