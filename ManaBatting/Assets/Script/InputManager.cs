using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public Transform selectCard;

    Vector2 pos;

    void Update()
    {
        if (selectCard == null && Input.GetMouseButtonDown(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward, 10f, LayerMask.GetMask("Card"));
            if (hit.collider != null)
            {
                selectCard = hit.transform;
            }
        }

        if (selectCard != null && Input.GetMouseButtonUp(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapBox(pos, new Vector2(6, 8), 0f, LayerMask.GetMask("BatchSlot"));
            if (hit != null)
            {
                print("batch");
                selectCard.position = hit.transform.position;
            }
            else {
                selectCard.GetComponent<CardBehaviour>().SetReplaceOrign();
            }

            selectCard = null;
        }

        if (selectCard != null && Input.GetMouseButton(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectCard.position = Vector2.Lerp(selectCard.position, pos, Time.deltaTime * 10f);
        }



    }

}
