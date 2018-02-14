using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase instance;

    public List<Card> dataList;

    private DataService ds;

    void Awake() {
        instance = this;

        ds = new DataService("CardDatabase.db");
        //CreateDB();
        StartSync();
    }

    void Start()
    {

    }

    private void StartSync()
    {
        dataList = GetCards();
        ToConsole(GetCards());
    }

    private void ToConsole(Card _card)
    {
        print(_card.ToString());
    }

    private void ToConsole(IEnumerable<Card> _cardList)
    {
        foreach (var Card in _cardList)
        {
            print(Card.ToString());
        }
    }

    public void CreateDB()
    {
        ds.Connection.DropTable<Card>();
        ds.Connection.CreateTable<Card>();

        ds.Connection.InsertAll(new[]{
            new Card{
                id = 1,
                name = "토마토 맞좀 봐라!",
                explain = "상대에게 5 데미지를 입힙니다.",
                cost = 1,
                type = CardType.Attack,
                attack = 5,
                depence = 0,
                heal = 0,
                buff = 1,
                effectEventName = "TasteTomato",
                frontSpritePath = "eichi_RARE2_front",
                middleSpritePath = "f050",
                backSpritePath = "1",
                hideSpritePath = "eichi_RARE2_back"
            },
            new Card{
                id = 1,
                name = "토마토 맞좀 봐라!",
                explain = "상대에게 5 데미지를 입힙니다.",
                cost = 1,
                type = CardType.Heal,
                attack = 5,
                depence = 0,
                heal = 0,
                buff = 1,
                effectEventName = "TasteTomato",
                frontSpritePath = "eichi_RARE2_front",
                middleSpritePath = "f050",
                backSpritePath = "1",
                hideSpritePath = "eichi_RARE2_back"
            }
        });
    }

    public List<Card> GetCards()
    {
        return ds.Connection.Table<Card>().ToList();
    }

    public List<Card> GetCardsWithName(string _name)
    {
        return ds.Connection.Table<Card>().Where(x => x.name == _name).ToList();
    }

    public Card GetCardWithID(int _id)
    {
        return ds.Connection.Table<Card>().Where(x => x.id == _id).FirstOrDefault();
    }

    public Card GetCardWithName(string _name)
    {
        return ds.Connection.Table<Card>().Where(x => x.name == _name).FirstOrDefault();
    }

    public Card CreateCard(Card _card)
    {
        var p = _card;
        ds.Connection.Insert(p);
        return p;
    }


}
