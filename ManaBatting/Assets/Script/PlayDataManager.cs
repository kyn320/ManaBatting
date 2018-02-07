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

    public void SetColor(Color _color)
    {
        playerColor = _color;
        PlayerPrefs.SetString("PlayerColor", "#" + ColorUtility.ToHtmlStringRGBA(playerColor));
    }

    public void SetPlayerName(string _name)
    {
        playerName = _name;
        PlayerPrefs.SetString("PlayerName", _name);
    }

    public void AddPlayRecord(int _win, int _lose, int _tie)
    {
        win += _win;
        lose += _lose;
        tie = _tie;

        PlayerPrefs.SetInt("Win", win);
        PlayerPrefs.SetInt("Lose", lose);
        PlayerPrefs.SetInt("Tie", tie);
    }

}
