using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SQLite4Unity3d;

[System.Serializable]
public class Card
{
    public string name { get; set; }

    public string explain { get; set; }

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    public Sprite frontSprite;// { get; set; }
    public Sprite middleSprite;// { get; set; }
    public Sprite backSprite; //{ get; set; }
    public Sprite hideSprite;// { get; set; }

    public int cost { get; set; }
    public CardType type { get; set; }
    public int attack { get; set; }
    public int depence { get; set; }
    public int heal { get; set; }
    public float buff { get; set; }

    public string effectEventName { get; set; }

    public Card()
    {
        name = "토마토 맞좀 봐라!";
        explain = "상대에게 5 데미지를 입힙니다.";
        id = 1;
        cost = 1;
        attack = 5;
        depence = 0;
        heal = 0;
        buff = 1;
        effectEventName = "TasteTomato";
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}:{2}:{3}", name, explain, id, effectEventName);
    }
}

[System.Serializable]
public enum CardType
{
    Attack,
    Depence,
    Heal,
    Buff
}
