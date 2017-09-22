using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
	public Player bank;
	public GameObject[] gamePieces;
	private int turn = 0;
	private bool hasRolled = false, hasDecided = false /*,doBuy = false*/;
	private int[] numRoll = new int[2];
	public GameObject roller, buyer, nothing, houses;
	private Spots[] board;
	public Player[] players;
	public GameObject text, output, hotels, hotelsOut;
	public GameObject[] properties;
	private int rollsInARow = 0;
	private string[][] hotelGroups = new string[][] {
		new string[]{ "Brown", "Meditteranean Avenue", "Baltic Avenue" },
		new string[]{ "Light Blue", "Oriental Avenue", "Vermont Avenue", "Connecticut Avenue" },
		new string[]{ "Purple", "St. Charles Place", "States Avenue", "Virginia Avenue" },
		new string[]{ "Orange", "St. James Place", "Tennessee Avenue", "New York Avenue" },
		new string[]{ "Red", "Kentucky Avenue", "Indiana Avenue", "Illinois Avenue" },
		new string[]{ "Yellow", "Atlantic Avenue", "Ventnor Avenue", "Marvin Gardens" },
		new string[]{ "Green", "Pacific Avenue", "No. Carolina Avenue", "Pennsylvania Avenue" },
		new string[]{ "Dark Blue", "Park Place", "Broadwalk" }
	};

	public string color;
	public int housePos;

	public GameObject[] zooms;
	public GameObject[] backs;


	public static readonly int START_MONEY = 1500;

	public static readonly float NOTIFICATIONS_WAIT = 5f;

	public static bool AIMODE = false;


	// Use this for initialization
	void Start () {
		bank = new Player (null, int.MaxValue/2, "Bank");

		string[] names = {"Go", "Meditteranean Avenue", "Community Chest", "Baltic Avenue", "Income Tax",
			"Reading Railroad", "Oriental Avenue", "Chance", "Vermont Avenue", "Connecticut Avenue", "Jail",
			"St. Charles Place", "Electric Company", "States Avenue", "Virginia Avenue", "Pennsylvania Railroad",
			"St. James Place", "Community Chest", "Tennessee Avenue", "New York Avenue", "Free Parking", "Kentucky Avenue",
			"Chance", "Indiana Avenue", "Illinois Avenue", "B&O Railroad", "Atlantic Avenue", "Ventnor Avenue", "Waterworks",
			"Marvin Gardens", "Go To Jail", "Pacific Avenue", "No. Carolina Avenue", "Community Chest", "Pennsylvania Avenue",
			"Short Line", "Chance", "Park Place", "Luxury Tax", "Broadwalk"
		};

		int[] prices = {0, 60, 0, 60, 0, 200, 100, 0, 100, 120, 0, 140, 150, 140, 160, 200, 180, 0, 180, 200, 0, 220, 0, 220, 240, 200, 260, 260, 150,
			280, 0, 300, 300, 0, 320, 200, 0, 350, 0, 400
		};

		int[][] rents = {new int[]{ 0 }, new int[]{ 2, 10, 30, 90, 160, 250 }, new int[]{ 0 }, new int[]{ 4, 20, 60, 180, 320, 450 }, 
			new int[]{ 0 }, new int[]{ 0 }, new int[]{ 6, 30, 90, 270, 400, 550 }, new int[]{ 0 }, new int[]{ 6, 30, 90, 270, 400, 550 },
			new int[]{ 8, 40, 100, 300, 450, 600 }, new int[]{ 0 }, new int[]{ 10, 50, 150, 450, 625, 750 }, new int[]{ 0 },
			new int[]{ 10, 50, 150, 450, 625, 750 }, new int[]{ 12, 60, 180, 500, 700, 900 }, new int[]{ 0 }, new int[]{ 14, 70, 200, 550, 750, 950 },
			new int[]{ 0 }, new int[]{ 14, 70, 200, 550, 750, 950 }, new int[]{ 16, 80, 220, 600, 800, 100 }, new int[]{ 0 }, 
			new int[]{ 18, 90, 250, 700, 875, 1050 }, new int[]{ 0 }, new int[]{ 18, 90, 250, 700, 875, 1050 }, new int[] {
				20,
				100,
				300,
				750,
				925,
				1100
			},
			new int[]{ 0 }, new int[]{ 22, 110, 330, 800, 975, 1150 }, new int[]{ 22, 110, 330, 800, 975, 1150 }, new int[]{ 0 }, 
			new int[]{ 24, 120, 360, 850, 1025, 1200 }, new int[]{ 0 }, new int[]{ 26, 130, 390, 900, 1100, 1275 }, new int[] {
				26,
				130,
				390,
				900,
				1100,
				1275
			},
			new int[]{ 0 }, new int[]{ 28, 150, 450, 1000, 1200, 1400 }, new int[]{ 0 }, new int[]{ 0 }, new int[]{ 35, 175, 500, 1100, 1300, 1500 }, 
			new int[]{ 0 }, new int[]{ 50, 200, 600, 1400, 1700, 2000 }
		};
		int[] comm = {2,17,33};
		int[] chance = {7,22,36};
		int[] special = {0,10,12,20,28,30 };
		string[] spec = {"Go","Jail","Utility","Free Parking","Utility","Go to Jail"};
		int[] tax = {4,38};
		string[] ttype = {"I","L"};
		int[] RRs = {5,15,25,35};
		board = new Spots[40];
		for (int i = 0; i < board.Length; i++) {
			board [i] = new Spots (i, names [i]);
			board [i].housePrice = 50 * (i / 10 + 1);
		}
		for (int i = 0; i < comm.Length; i++) {
			board [comm [i]].setCommunity ();
			board [comm [i]].owner = bank;
		}
		for (int i = 0; i < chance.Length; i++) {
			board [chance [i]].setChance ();
			board [chance [i]].owner = bank;
		}
		for (int i = 0; i < special.Length; i++) {
			board [special [i]].setSpecial (spec [i]);
			if (!spec[i].Equals("Utility"))
				board [special [i]].owner = bank;
		}
		for (int i = 0; i < tax.Length; i++) {
			board [tax[i]].setTax (ttype [i]);
			board [tax[i]].owner = bank;
		}
		for (int i = 0; i < prices.Length; i++) {
			board [i].setPrice (prices [i]);
			board [i].setRent (rents [i]);
		}
		for (int i = 0; i < RRs.Length; i++)
			board [RRs [i]].setRR ();
		players = new Player[gamePieces.Length];
		string[] nameMap = { "You", "Invader Zim" };
		players [0] = new Player (gamePieces[0], START_MONEY, nameMap[0]);
		if (AIMODE)
			players [0] = new AIPlayer (gamePieces [0], START_MONEY, nameMap [0]);
		for (int i = 1; i < players.Length; i++)
			players [i] = new AIPlayer (gamePieces [i], START_MONEY, nameMap[i]);

		/*foreach (Spots s in board) {
			s.owner = players [0];
			players [0].owns.AddFirst (s);
		}*/
		//board [3].setPrice (1000);
		//board [3].setRent (200);
		//goToJail();
		if (AIMODE) {
			doNothing ();
			roller.SetActive (false);
			houses.SetActive (false);
		}
		StartCoroutine (gameLoop ());
	}
	
	// Update is called once per frame
	void Update () {
		string t = "Scoreboard:\n";
		for (int i = 0; i < players.Length; i++) {
			t += players [i].pname + ": " + players [i].money;
			if (players [i].lost)
				t += " LOST";
			t += "\n";
		}
		text.GetComponent<UnityEngine.UI.Text> ().text = t;
		for (int i = 0; i < properties.Length; i++) {
			string prop = players [i].pname + "\n";
			foreach (Spots p in players[i].owns)
				prop += p.name + " Houses: " + p.houses + "\n";
			properties [i].GetComponent<UnityEngine.UI.Text> ().text = prop;
		}
		string hotes = "You can purchase hotels on the following colors:\n";
		for (int i = 0; i < hotelGroups.Length; i++)
			if (players [0].canPurchaseHouse (hotelGroups [i]))
				hotes += hotelGroups [i][0] + "\n";
		hotes += "Select a valid color and then select which property you want to put it on";
		hotels.GetComponent<UnityEngine.UI.Text> ().text = hotes;
	}

	IEnumerator gameLoop(){
		while (true) {
			if (gameOver ())
				break;
			hasRolled = false;
			if (players [turn].GetType ().FullName.Equals("Player")) {
				buyer.SetActive (false);
				nothing.SetActive (false);
				houses.SetActive (true);
				roller.SetActive (true);
			} else {
				for (int i = 0; i < hotelGroups.Length; i++)
					if (((AIPlayer)players [turn]).wantsHouses (hotelGroups [i])) {
						outputString (((AIPlayer)players [turn]).buyHouses (hotelGroups [i]));
						yield return new WaitForSeconds (NOTIFICATIONS_WAIT);
					}
				
				
				roll ();

			}
			while (!hasRolled)
				yield return new WaitForSeconds (1f);
			if (players[turn].inJail && numRoll [0] == numRoll [1] || players [turn].rollsInJail == 2)
				outOfJail ();
			else if (players [turn].inJail && numRoll [0] != numRoll [1]) {
				players [turn].rollsInJail++;
				outputString (players [turn] + " is stuck in jail");
				yield return new WaitForSeconds (NOTIFICATIONS_WAIT);
				turn = (turn + 1) % gamePieces.Length;
				continue;
			} 
			int startIndex = players[turn].position;
			for (int i = 1; i <= numRoll [0] + numRoll [1]; i++) {
				board [(startIndex + i + 39) % 40].tempHolder = null;
				board [(startIndex + i) % 40].tempHolder = gamePieces [turn];
				if ((startIndex + i) % 40 == 0)
					players [turn].money += 200;
				yield return StartCoroutine (pieceForward (turn));
				if (needsToRotate (turn))
					yield return StartCoroutine (rotatePiece (turn, 90, gamePieces [turn].transform.rotation));
				if (i == numRoll [0] + numRoll [1])
					players [turn].position = (startIndex + i) % 40;
			}
			hasDecided = false;
			if (players [turn].GetType ().FullName.Equals("Player") && board [players [turn].position].owner == null) {
				buyer.SetActive (true);
				nothing.SetActive (true);
				roller.SetActive (false);
				houses.SetActive (false);
			} else if (players [turn].GetType ().FullName.Equals("Player"))
				doNothing ();
			else {
				if (players [turn].canBuy (board[players[turn].position]))
					buy ();
				else
					doNothing ();
			}
			if (players [turn].position == 30) {
				outputString (players [turn] + " went to Jail");
				goToJail ();
				doNothing ();
			}

			//Debug.Log (board [players [turn].position].owner.pname);
			if (board [players [turn].position].isCommunity)
				drawCommunity ();
			else if (board [players [turn].position].isChance)
				drawChance ();
			else if (board [players [turn].position].owner != null && !board [players [turn].position].owner.pname.Equals (players [turn].pname)) {
				//Debug.Log (board [players [turn].position].getRent (numRoll [0] + numRoll [1]));
				board [players [turn].position].owner.money += board [players [turn].position].getRent (numRoll [0] + numRoll [1]);
				players [turn].money -= board [players [turn].position].getRent (numRoll [0] + numRoll [1]);
				if (!(board[players[turn].position].name.Equals("Go") || board[players[turn].position].name.Equals("Jail")
					|| board[players[turn].position].name.Equals("Free Parking") || board[players[turn].position].name.Equals("Go To Jail")))
						outputString (board [players [turn].position].owner.pname + " received $" +
						board [players [turn].position].getRent (numRoll [0] + numRoll [1]) + " from "
						+ players [turn].pname);
				hasDecided = true;
			}


			while (!hasDecided)
				yield return new WaitForSeconds (1f);
			doNothing ();
			yield return new WaitForSeconds (NOTIFICATIONS_WAIT);

			if (numRoll [0] == numRoll [1]) {
				rollsInARow++;
				if (rollsInARow == 3) {
					players [turn].money += board [players [turn].position].getRent (numRoll [0] + numRoll [1]);
					goToJail ();
					outputString (players [turn].pname + " went to Jail for rolling 3 doubles in a row");
					rollsInARow = 0;
					yield return new WaitForSeconds (NOTIFICATIONS_WAIT);
				} else
					continue;
			} else
				rollsInARow = 0;
			turn = (turn + 1)% gamePieces.Length;
		}
		yield return null;
	}

	public void roll(){
		for (int i = 0; i < 2; i++)
			numRoll [i] = Random.Range (1, 7);
		roller.SetActive (false);
		houses.SetActive (false);
		outputString (players [turn].pname + " rolled a " + numRoll [0] + " and a " + numRoll [1]
			+ " moving " + (numRoll[0] + numRoll[1]) + " spaces");
		hasRolled = true;
	}

	IEnumerator pieceForward(int index){
		//gamePieces [index].transform.Translate (25f * Vector3.forward);
		Vector3 direction = gamePieces[index].transform.forward;
		float speed = 1f;
		float startime = Time.time;
		Vector3 start_pos = gamePieces[index].transform.position; //Starting position.
		Vector3 end_pos = gamePieces[index].transform.position + direction; //Ending position.

		while (start_pos != end_pos && ((Time.time - startime)*speed) < 1f) { 
			float move = Mathf.Lerp (0,.68f, (Time.time - startime)*speed);
			gamePieces[index].transform.position += direction*move;

			yield return null;
		}
	}

	IEnumerator rotatePiece(int index, float rotationAmount, Quaternion startingRotation){
		Quaternion finalRotation = Quaternion.Euler( 0, 0, -rotationAmount ) * startingRotation;
		while(gamePieces[index].transform.rotation != finalRotation){
			gamePieces[index].transform.rotation = Quaternion.Lerp(gamePieces[index].transform.rotation, finalRotation, 5f * Time.deltaTime);
			yield return null;
		}
	}

	public bool needsToRotate(int index){
		for (int j = 0; j < 40 ; j += 10)
			if (board[j].tempHolder != null && board [j].tempHolder.Equals (gamePieces [index]))
				return true;
		return false;
	}

	public void buy(){
		players [turn].money -= board [players [turn].position].price;
		board [players [turn].position].owner = players [turn];
		players [turn].owns.AddLast (board [players [turn].position]);
		//doBuy = true;
		outputString(players[turn].pname + " bought " + board[players[turn].position].name);
		hasDecided = true;
		buyer.SetActive (false);
		nothing.SetActive (false);
	}

	public void doNothing(){
		hasDecided = true;
		buyer.SetActive (false);
		nothing.SetActive (false);
	}
	public bool gameOver(){
		int playersIn = 0;
		for (int i = 0; i < players.Length; i++) {
			if (players [i].updateLoss ())
				outputString (players [i].pname + " Lost");
			if (!players [i].lost)
				playersIn++;
		}
		return playersIn <= 1;
	}

	public void goToJail(){
		players [turn].position = 10;
		players [turn].inJail = true;
		players [turn].rollsInJail = 0;
		gamePieces [turn].transform.position = new Vector3 (-101f,-107.3f,47.37f);
		gamePieces [turn].transform.eulerAngles = new Vector3 (-90, -90, 90);
	}

	public void outOfJail(){
		players [turn].inJail = false;
		players [turn].rollsInJail = 0;
	}

	public void outputString(string s){
		output.GetComponent<UnityEngine.UI.Text> ().text = s;
	}

	public void drawChance(){
		int mo = Random.Range (0, 201);
		players [turn].money += mo;
		outputString (players [turn].pname + " received $" + mo + " from chance");
	}

	public void drawCommunity(){
		int mo = Random.Range (0, 101);
		players [turn].money += mo;
		outputString (players [turn].pname + " received $" + mo + " from community chest");
	}

	public void switchToHouses(){
		this.transform.position = new Vector3 (0, 0, 386);
		hotels.SetActive (true);
		hotelsOut.SetActive (true);
	}

	public void switchToBoard(){
		this.transform.position = new Vector3 (0, 0, -25);
		this.transform.rotation = new Quaternion();
		hotels.SetActive (false);
		hotelsOut.SetActive (false);
		for (int i = 0; i < zooms.Length; i++) {
			zooms [i].SetActive (true);
			backs [i].SetActive (false);
		}
	}

	public void addHouse(){
		string s = "";
		string propname = "";
		for (int i = 0; i < hotelGroups.Length; i++)
			if (color.Equals (hotelGroups [i] [0])) {
				propname = hotelGroups [i] [housePos];
				for (int j = 1; j < hotelGroups [i].Length; j++)
					if (!players [turn].hasPropByName (hotelGroups [i] [j]))
						return;
			}
		for (int i = 0; i < board.Length; i++)
			if (board [i].name.Equals (propname)) {
				if (players [turn].money >= board [i].housePrice && board[i].houses < 5) {
					board [i].houses++;
					players [turn].money -= board [i].housePrice;
					s = board [i].name + "'s rent went from $" + board [i].rent [board [i].houses - 1] + " to $"
					+ board [i].rent [board [i].houses];
					hotelsOut.GetComponent<UnityEngine.UI.Text> ().text = s;
				}
			}
	}
	public void zoom(string place){
		if (place.Equals ("B"))
			zoomBottom ();
		else if (place.Equals ("T"))
			zoomTop ();
		else if (place.Equals ("L"))
			zoomLeft ();
		else if (place.Equals ("R"))
			zoomRight ();
		for (int i = 0; i < zooms.Length; i++) {
			zooms [i].SetActive (false);
			backs [i].SetActive (true);
		}
	}

	public void zoomBottom(){
		this.transform.position = new Vector3 (7,-100,0);
	}

	public void zoomTop(){
		this.transform.position = new Vector3 (7,100,0);
		this.transform.Rotate (new Vector3 (0, 0, 180));
	}

	public void zoomLeft(){
		this.transform.position = new Vector3 (-75,0,0);
		this.transform.Rotate (new Vector3 (0, 0, -90));
	}

	public void zoomRight(){
		this.transform.position = new Vector3 (75,0,0);
		this.transform.Rotate (new Vector3 (0, 0, 90));
	}
}
