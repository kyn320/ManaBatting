using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SQLite4Unity3d;

[System.Serializable]
public class Card
{
    public string Name;
    public string name
    {
        get { return Name; }
        set { Name = value; }
    }

    public string Explain;
    public string explain
    {
        get { return Explain; }
        set { Explain = value; }
    }

    public int ID;
    [PrimaryKey, AutoIncrement]
    public int id
    {
        get { return ID; }
        set { ID = value; }
    }

    public int Cost;
    public int cost
    {
        get { return Cost; }
        set { Cost = value; }
    }

    public CardType Type;
    public CardType type
    {
        get { return Type; }
        set { Type = value; }
    }

    public int Attack;
    public int attack
    {
        get { return Attack; }
        set { Attack = value; }
    }

    public int Depence;
    public int depence
    {
        get { return Depence; }
        set { Depence = value; }
    }

    public int Heal;
    public int heal
    {
        get { return Heal; }
        set { Heal = value; }
    }

    public float Buff;
    public float buff
    {
        get { return Buff; }
        set { Buff = value; }
    }

    public string EffectEventName;
    public string effectEventName
    {
        get { return EffectEventName; }
        set { EffectEventName = value; }
    }

    public string FrontSpritePath;
    public string frontSpritePath
    {
        get { return FrontSpritePath; }
        set
        {
            FrontSpritePath = value;
            if (FrontSpritePath != "")
                frontSprite = Resources.Load<Sprite>("Card/Front/" + FrontSpritePath);
        }
    }
    public Sprite frontSprite;

    public string MiddleSpritePath;
    public string middleSpritePath
    {
        get { return MiddleSpritePath; }
        set
        {
            MiddleSpritePath = value;
            if (MiddleSpritePath != "")
                middleSprite = Resources.Load<Sprite>("Card/Middle/" + MiddleSpritePath);
        }
    }
    public Sprite middleSprite;

    public string BackSpritePath;
    public string backSpritePath
    {
        get { return BackSpritePath; }
        set
        {
            BackSpritePath = value;
            if (BackSpritePath != "")
                backSprite = Resources.Load<Sprite>("Card/Back/" + BackSpritePath);
        }
    }
    public Sprite backSprite;

    public string HideSpritePath;
    public string hideSpritePath
    {
        get { return HideSpritePath; }
        set
        {
            HideSpritePath = value;
            if (HideSpritePath != "")
                hideSprite = Resources.Load<Sprite>("Card/Hide/" + HideSpritePath);
        }
    }
    public Sprite hideSprite;

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
