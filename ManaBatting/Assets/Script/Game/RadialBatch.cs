using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RadialBatch : MonoBehaviour
{
    public float fDistance;

    [Range(0f, 360f)]
    public float MinAngle, MaxAngle, StartAngle;

    void Update()
    {
        Batch();
    }

    void Batch()
    {
        if (transform.childCount == 0)
            return;
        float fOffsetAngle = ((MaxAngle - MinAngle)) / (transform.childCount - 1);

        float fAngle = StartAngle;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                Vector3 vPos = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                child.localPosition = vPos * fDistance;
                fAngle += fOffsetAngle;
            }
        }

    }


}
