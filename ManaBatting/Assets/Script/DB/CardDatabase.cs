using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDatabase : Singleton<CardDatabase>
{
    public List<Card> itemList;

    private DataService dataService;

    protected override void Awake()
    {
        base.Awake();
        dataService = new DataService("CardDatabase.db");
        //CreateDB();
        StartSync();
    }
    
    private void StartSync()
    {
        itemList = GetCards();
        ToConsole(GetCards());
    }

    private void ToConsole(Card card)
    {
        print(card.ToString());
    }

    private void ToConsole(IEnumerable<Card> cardList)
    {
        foreach (var Card in cardList)
        {
            print(Card.ToString());
        }
    }

    public void CreateDB()
    {
        dataService.Connection.DropTable<Card>();
        dataService.Connection.CreateTable<Card>();

        dataService.Connection.InsertAll(new[]{
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
                frontSpritePath = "eichiRARE2front",
                middleSpritePath = "f050",
                backSpritePath = "1",
                hideSpritePath = "eichiRARE2back"
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
                frontSpritePath = "eichiRARE2front",
                middleSpritePath = "f050",
                backSpritePath = "1",
                hideSpritePath = "eichiRARE2back"
            }
        });
    }

    public List<Card> GetCards()
    {
        return dataService.Connection.Table<Card>().ToList();
    }

    public List<Card> GetCardsWithName(string name)
    {
        return dataService.Connection.Table<Card>().Where(x => x.name == name).ToList();
    }

    public Card GetCardWithID(int id)
    {
        return dataService.Connection.Table<Card>().Where(x => x.id == id).FirstOrDefault();
    }

    public Card GetCardWithName(string name)
    {
        return dataService.Connection.Table<Card>().Where(x => x.name == name).FirstOrDefault();
    }

    public Card CreateCard(Card card)
    {
        var p = card;
        dataService.Connection.Insert(p);
        return p;
    }


}
