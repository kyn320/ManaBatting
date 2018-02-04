using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{

    public UIManaBet manaBet;

    public void OnFinshBatch()
    {
        GameManager.instance.SendReadyBatch(GameManager.instance.myID);
    }


}
