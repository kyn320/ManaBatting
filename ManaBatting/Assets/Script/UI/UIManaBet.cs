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

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void UpdateAllBet()
    {
        betManaText[0].text = GameManager.Instance.GetMyBet().ToString();
        betManaText[1].text = GameManager.Instance.GetOtherBet().ToString();

        totalManaText.text = GameManager.Instance.GetTotalBet().ToString();
    }

    public void ViewBet()
    {
        subButton.gameObject.SetActive(true);
        addButton.gameObject.SetActive(true);

        giveUpButton.gameObject.SetActive(true);
        betButton.gameObject.SetActive(true);
    }

    public void HideBet() {
        subButton.gameObject.SetActive(false);
        addButton.gameObject.SetActive(false);

        giveUpButton.gameObject.SetActive(false);
        betButton.gameObject.SetActive(false);
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
        GameManager.Instance.RPCManaBet(manaBetThink);
        manaBetThink = 0;
        betManaText[0].text = manaBetThink.ToString();
    }

}
