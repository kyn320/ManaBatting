using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardEffect : MonoBehaviour {

    public UnityAction endAction;

    public virtual void StartAction() {
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        print("Effect is Run");
        yield return new WaitForSeconds(0.1f);
        endAction.Invoke();
        print("out effect");
    }

    public virtual void SetEndAction(UnityAction _action)
    {
        endAction = _action;
    }
}
