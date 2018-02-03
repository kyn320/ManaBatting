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

    void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();

    }

    public void SetIndex(int _index)
    {
        index = _index;
        sprRenderer.sortingOrder = index + 1;

        orignPos = transform.position;
        orignRot = transform.rotation;
    }

    public void SetCard(Card _card)
    {
        card = _card;
        sprRenderer.sprite = card.cardSprite;
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
        float dist = 1;
        while (dist > 0.01f)
        {
            dist = Vector2.SqrMagnitude(orignPos - (Vector2)transform.position);
            transform.position = Vector2.Lerp(transform.position, orignPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, orignRot, Time.deltaTime * 10f);
            yield return null;
        }

        replaceOrign = null;
    }

}
