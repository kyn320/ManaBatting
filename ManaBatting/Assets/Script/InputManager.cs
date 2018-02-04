using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public CardBehaviour selectCard;

    Vector2 pos;

    BatchSlot slot;
    CardBehaviour card;

    void Update()
    {
        if (selectCard == null && Input.GetMouseButtonDown(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward, 10f, LayerMask.GetMask("Card"));
            if (hit.collider != null)
            {
                card = hit.collider.GetComponent<CardBehaviour>();

                if (card.isMine)
                    selectCard = card;

            }
        }

        if (selectCard != null && Input.GetMouseButtonUp(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapBox(pos, new Vector2(6, 8), 0f, LayerMask.GetMask("BatchSlot"));

            if (hit != null)
            {
                slot = hit.GetComponent<BatchSlot>();

                if (slot.IsAllowBatch())
                {
                    if (selectCard.slot != null)
                    {
                        selectCard.UnSlot();
                    }
                    else
                    {
                        selectCard.SubHand();
                    }

                    selectCard.SetSlot(slot);
                }
            }
            else
            {
                if (selectCard.slot != null)
                {
                    selectCard.UnSlot();
                }

                selectCard.AddHand();
            }

            selectCard = null;
        }

        if (selectCard != null && Input.GetMouseButton(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectCard.transform.position = Vector2.Lerp(selectCard.transform.position, pos, Time.deltaTime * 10f);
        }
    }

}
