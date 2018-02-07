using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ExistingDBScript : MonoBehaviour {

	public Text DebugText;

	// Use this for initialization
	void Start () {
		var ds = new DataService ("tempDatabase.db");
		//ds.CreateDB ();
		var people = ds.GetCards ();
		ToConsole (people);

		people = ds.GetCardsNamedBanana();
		ToConsole("Searching for Roberto ...");
		ToConsole (people);

		ds.CreateCard ();
		ToConsole("New Card has been created");
		var p = ds.GetJohnny ();
		ToConsole(p.ToString());

	}
	
	private void ToConsole(IEnumerable<Card> people){
		foreach (var Card in people) {
			ToConsole(Card.ToString());
		}
	}

	private void ToConsole(string msg){
		DebugText.text += System.Environment.NewLine + msg;
		Debug.Log (msg);
	}

}
