using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public CardBehaviour selectCard;

    public float dragSpeed = 50f;

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
                {
                    selectCard = card;
                    selectCard.SetSortingOrder(100);
                }
            }
        }

        if (selectCard != null && Input.GetMouseButtonUp(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapBox(pos, new Vector2(6, 8), 0f, LayerMask.GetMask("BatchSlot"));

            //놓은 곳이 배치 슬롯
            if (hit != null)
            {
                slot = hit.GetComponent<BatchSlot>();

                if (slot.IsAllowBatch())
                {
                    if (selectCard.slot != null && selectCard.slot == slot)
                    {
                        print("replace");
                        //replace slot
                        selectCard.SetReplaceSlot();
                    }
                    else if (!slot.IsEmpty())
                    {
                        CardBehaviour crossCard = slot.GetCard();
                        //슬롯 < = > 슬롯 크로스 버그로 인한 개발 중지
                        //if (selectCard.slot != null)
                        //{
                        //    BatchSlot crossSlot = selectCard.slot;
                        //    print("slot to slot cross");
                        //    //cross slot to slot change
                        //    selectCard.SetSlot(slot, false, false);
                        //    crossCard.SetSlot(crossSlot, false, false);
                        //
                        //    selectCard.slot.SendSetBatch(selectCard, false);
                        //    crossCard.slot.SendSetBatch(crossCard, false);
                        //}
                        if (selectCard.slot == null)
                        {
                            print("hand to slot cross");
                            //cross hand to slot change
                            //crossCard.AddHand();
                            //selectCard.SubHand();
                            crossCard.UnSlot(true, true);
                            selectCard.SetSlot(slot, true, true);
                        }
                    }
                    else if (slot.IsEmpty())
                    {
                        print("new set");
                        //new set slot
                        if (selectCard.slot != null)
                        {
                            selectCard.UnSlot(true, true);
                        }
                        //else {
                        //    selectCard.SubHand();
                        //}
                        selectCard.SetSlot(slot, true, true);
                    }
                }
                else
                {
                    print("other player card + " + slot.IsAllowBatch());
                    selectCard.SetReplaceOrign();
                    selectCard.SetSortingOrder();
                }
            }
            //그 외의 경우
            else
            {
                if (selectCard.slot != null)
                {
                    selectCard.UnSlot(true, true);
                    //selectCard.AddHand();
                }
                else
                {
                    selectCard.SetReplaceOrign();
                }
                selectCard.SetSortingOrder();
            }

            selectCard = null;
        }

        if (selectCard != null && Input.GetMouseButton(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectCard.transform.position = Vector2.Lerp(selectCard.transform.position, pos, Time.deltaTime * dragSpeed);
            selectCard.transform.rotation = Quaternion.Lerp(selectCard.transform.rotation, Quaternion.identity, Time.deltaTime * dragSpeed);
        }
    }

}
