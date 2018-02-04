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
    }

    public void UpdateBet()
    {

    }

    public void ViewBet()
    {
        betManaText[0].text = GameManager.instance.GetMyBet().ToString();
        betManaText[1].text = GameManager.instance.GetOtherBet().ToString();

        subButton.gameObject.SetActive(true);
        addButton.gameObject.SetActive(true);

        giveUpButton.gameObject.SetActive(true);
        betButton.gameObject.SetActive(true);
    }

    public void AddMana()
    {
        ++manaBetThink;
    }

    public void SubMana()
    {
        --manaBetThink;
    }

    public void GiveUp()
    {
        manaBetThink = -1;
    }

    public void Bet()
    {
        GameManager.instance.SendManaBet(manaBetThink);
        manaBetThink = 0;
    }

}
