using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public int index;

    public Card card;

    Vector2 orignPos;
    Quaternion orignRot;

    public SpriteRenderer frontSprRenderer, middleSprRenderer, backSprRenderer;

    public HandCard hand;
    public BatchSlot slot;

    public bool isMine = false;

    public Canvas infoView;
    public Text cardNameText, cardCostText, cardTypeText, cardExplainText;

    public PhotonView photonView;

    public UnityAction endAction;

    CardEffect effect;

    void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    void Start()
    {
        effect = GetComponent<CardEffect>();
    }

    public void SetHand(HandCard _hand)
    {
        hand = _hand;
    }

    public void SetHandOrign(Vector2 _pos, Quaternion _rot)
    {
        orignPos = _pos;
        orignRot = _rot;

        SetReplaceOrign();
    }

    public void SetIndex(int _index)
    {
        index = _index;
        SetSortingOrder();
    }

    public void SetSortingOrder()
    {
        frontSprRenderer.sortingOrder = index * 10 + +6;
        middleSprRenderer.sortingOrder = index * 10 + 5;
        backSprRenderer.sortingOrder = index * 10 + 4;
        infoView.sortingOrder = index * 10 + 7;
    }

    public void SetSortingOrder(int _order)
    {
        frontSprRenderer.sortingOrder = _order * 10 + +6;
        middleSprRenderer.sortingOrder = _order * 10 + 5;
        backSprRenderer.sortingOrder = _order * 10 + 4;
        infoView.sortingOrder = _order * 10 + 7;
    }

    public void SetCard(Card _card)
    {
        card = _card;

        gameObject.name = photonView.owner.NickName + " | " + card.name;

        ViewInfo();

        System.Type type = System.Type.GetType(card.effectEventName + "Effect");

        if (type != null)
            gameObject.AddComponent(type);
        else
            Debug.LogError(card.effectEventName + "Effect is not found.");
    }

    public void SetIsMine()
    {
        isMine = photonView.isMine;
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

    public void SetSlot(BatchSlot _slot, bool _isSend, bool _isControlHand)
    {
        slot = _slot;
        slot.Batch(this);

        if (_isSend)
            slot.SendSetBatch(this, _isControlHand);

        if (_isControlHand)
            SubHand();

        index = -1;
        SetTargetPlace(_slot.transform);
    }

    public void UnSlot(bool _isSend, bool _isControlHand)
    {
        if (slot == null)
            return;

        slot.UnBatch();

        if (_isSend)
            slot.SendUnBatch(_isControlHand);

        if (_isControlHand)
            AddHand();

        slot = null;
    }

    public void AddHand()
    {
        hand.AddCard(this);
    }

    public void SendAddHand()
    {
        photonView.RPC("OnAddHand", PhotonTargets.OthersBuffered, null);
    }

    [PunRPC]
    public void OnAddHand()
    {
        print("add hand");
        hand.AddCard(this);
    }

    public void SubHand()
    {
        hand.SubCard(index);
    }

    public void SendSubHand()
    {
        photonView.RPC("OnSubHand", PhotonTargets.OthersBuffered, null);
    }

    [PunRPC]
    public void OnSubHand()
    {
        print("sub hand");
        hand.SubCard(index);
    }

    public void SetTargetPlace(Transform _target)
    {
        if (movePosition != null)
        {
            StopCoroutine(movePosition);
        }

        movePosition = StartCoroutine(TargetPlace(_target));
    }

    IEnumerator TargetPlace(Transform _target)
    {
        float dist = 1, rot = 1f;

        while (dist > 0.01f || rot > 0.01f)
        {
            dist = Vector2.SqrMagnitude(_target.position - transform.position);
            rot = Quaternion.Angle(transform.rotation, _target.rotation);
            transform.position = Vector2.Lerp(transform.position, _target.position, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, Time.deltaTime * 10f);
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
        effect.SetEndAction(waitAction.Finish);
        effect.StartAction();
        yield return waitAction;
        print("card behaviour out");
        endAction.Invoke();
    }

    public void SetEndAction(UnityAction _action)
    {
        endAction = _action;
    }

}
