using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDataManager : MonoBehaviour
{
    public static PlayDataManager instance;

    public string playerName;

    public int win, lose, tie;
    public Color playerColor;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            win = PlayerPrefs.GetInt("Win", 0);
            lose = PlayerPrefs.GetInt("Lose", 0);
            tie = PlayerPrefs.GetInt("Tie", 0);
            ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("PlayerColor", "#FFFFFF"), out playerColor);
            playerName = PlayerPrefs.GetString("PlayerName", "Player" + Random.Range(1,1000));
        }
    }

    public void SetColor(Color color)
    {
        playerColor = color;
        PlayerPrefs.SetString("PlayerColor", "#" + ColorUtility.ToHtmlStringRGBA(playerColor));
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void AddPlayRecord(int win, int lose, int tie)
    {
        win += win;
        lose += lose;
        this.tie = tie;

        PlayerPrefs.SetInt("Win", win);
        PlayerPrefs.SetInt("Lose", lose);
        PlayerPrefs.SetInt("Tie", tie);
    }

}
