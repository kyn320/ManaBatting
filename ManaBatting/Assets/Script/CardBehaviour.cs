using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public int index;

    Vector2 orignPos;
    Quaternion orignRot;

    SpriteRenderer sprRenderer;

    public Card card;

    public HandCard hand;
    public BatchSlot slot;

    public bool isMine = false;

    void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
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
        sprRenderer.sortingOrder = index + 1;
    }

    public void SetCard(Card _card)
    {
        card = _card;
    }

    public void Open()
    {
        sprRenderer.sprite = card.openSprite;
    }

    public void Hide()
    {
        sprRenderer.sprite = card.backSprite;
    }

    public void SetReplaceOrign()
    {
        if (replaceOrign != null)
        {
            StopCoroutine(replaceOrign);
        }
        replaceOrign = StartCoroutine(ReplceOrign());
    }

    Coroutine replaceOrign = null;

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
        replaceOrign = null;
    }

    public void SetSlot(BatchSlot _slot)
    {
        slot = _slot;
        slot.Batch(this);

        SetTargetPlace(_slot.transform);
    }

    public void UnSlot()
    {
        if (slot == null)
            return;

        slot.UnBatch();
        slot = null;
    }

    public void AddHand()
    {
        hand.AddCard(this);
    }

    public void SubHand()
    {
        hand.SubCard(index);
    }

    public void SetTargetPlace(Transform _target)
    {
        if (targetPlace != null)
        {
            StopCoroutine(targetPlace);
        }

        targetPlace = StartCoroutine(TargetPlace(_target));
    }

    Coroutine targetPlace = null;

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
        targetPlace = null;
    }

}
