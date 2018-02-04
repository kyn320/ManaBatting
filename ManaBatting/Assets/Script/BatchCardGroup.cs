using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchCardGroup : MonoBehaviour
{
    public int id;

    bool isOpen = false;

    public CardBehaviour[] batchList;

    public void OpenCard(int _index)
    {
        StartCoroutine(Open(_index));
    }

    public void BatchCard(int _index, CardBehaviour _card)
    {
        batchList[_index] = _card;
    }

    IEnumerator Open(int _index)
    {
        Quaternion rot = Quaternion.Euler(0, 360f, 0);
        Transform cardTransform = batchList[_index].transform;
        float checkRot = Quaternion.Angle(cardTransform.rotation, rot);
        while (checkRot > 0.1f)
        {
            cardTransform.localRotation = Quaternion.Lerp(cardTransform.localRotation, rot, Time.deltaTime * 20f);
            checkRot = Quaternion.Angle(cardTransform.localRotation, rot);
            print(checkRot);
            if (checkRot < 0.5f) {
                batchList[_index].Open();
                print("test");
            }
            yield return null;
        }
        print("end open");
        isOpen = true;
    }



}
