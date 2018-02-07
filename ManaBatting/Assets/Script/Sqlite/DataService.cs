using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{

    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    public void CreateDB()
    {
        _connection.DropTable<Card>();
        _connection.CreateTable<Card>();

        _connection.InsertAll(new[]{
            new Card{
                name = "토마토 맞좀 봐라!",
                explain = "상대에게 5 데미지를 입힙니다.",
                id = 1,
                cost = 1,
                attack = 5,
                depence = 0,
                heal = 0,
                buff = 1,
                effectEventName = "TasteTomato"
            },
            new Card{
                name = "토마토 맞좀 봐라!",
                explain = "상대에게 5 데미지를 입힙니다.",
                id = 2,
                cost = 1,
                attack = 5,
                depence = 0,
                heal = 0,
                buff = 1,
                effectEventName = "TasteTomato"
            },
            new Card{
                name = "토마토 맞좀 봐라!",
                explain = "상대에게 5 데미지를 입힙니다.",
                id = 3,
                cost = 1,
                attack = 5,
                depence = 0,
                heal = 0,
                buff = 1,
                effectEventName = "TasteTomato"
            },
            new Card{
                name = "한글",
                explain = "상대에게 5 데미지를 입힙니다.",
                id = 4,
                cost = 1,
                attack = 5,
                depence = 0,
                heal = 0,
                buff = 1,
                effectEventName = "TasteTomato"
            }
        });
    }

    public IEnumerable<Card> GetCards()
    {
        return _connection.Table<Card>();
    }

    public IEnumerable<Card> GetCardsNamedBanana()
    {
        return _connection.Table<Card>().Where(x => x.name == "한글");
    }

    public Card GetJohnny()
    {
        return _connection.Table<Card>().Where(x => x.name == "바나나").FirstOrDefault();
    }

    public Card CreateCard()
    {
        var p = new Card
        {
            name = "바나나",
            explain = "상대에게 1 데미지를 입힙니다.",
            id = 5,
            cost = 1,
            attack = 5,
            depence = 0,
            heal = 0,
            buff = 1,
            effectEventName = "TasteTomato"
        };
        _connection.Insert(p);
        return p;
    }
}
