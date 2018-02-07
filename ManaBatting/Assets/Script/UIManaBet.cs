using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManaBet : MonoBehaviour
{
    public Text totalManaText;

    public int manaBetThink;

    [Header("항상 플레이어는 index 0번 고정")]
    public Text[] betManaText;

    public Button subButton, addButton, giveUpButton, betButton;

    public void View()
    {
        gameObject.SetActive(true);
        betManaText[0].text = betManaText[1].text = manaBetThink.ToString();
    }

    public void UpdateBet(int _mana)
    {
        betManaText[1].text = _mana.ToString();
    }

    public void ViewBet()
    {
        betManaText[0].text = GameManager.Instance.GetMyBet().ToString();
        betManaText[1].text = GameManager.Instance.GetOtherBet().ToString();

        subButton.gameObject.SetActive(true);
        addButton.gameObject.SetActive(true);

        giveUpButton.gameObject.SetActive(true);
        betButton.gameObject.SetActive(true);
    }

    public void AddMana()
    {
        ++manaBetThink;
        manaBetThink = Mathf.Clamp(manaBetThink, 0, 100);
        betManaText[0].text = manaBetThink.ToString();
    }

    public void SubMana()
    {
        --manaBetThink;
        manaBetThink = Mathf.Clamp(manaBetThink, 0, 100);
        betManaText[0].text = manaBetThink.ToString();
    }

    public void GiveUp()
    {
        manaBetThink = -1;
        Bet();
    }

    public void Bet()
    {
        GameManager.Instance.SendManaBet(manaBetThink);
        manaBetThink = 0;
        betManaText[0].text = manaBetThink.ToString();
    }

}
