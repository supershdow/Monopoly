using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player {

	private static readonly int numRolls = 100000;
	private static readonly float cutoff = .023f;
	private int timesRun = 0;
	private int[] freq;
	private int[] nonHotelables = {0,2,4,5,7,10,12,15,17,20,22,25,28,30,33,35,36,38 };

	public AIPlayer(GameObject go, int m, string n){
		piece = go;
		owns = new LinkedList<Spots> ();
		money = m;
		pname = n;
		freq = new int[40];
		learn ();
	}

	private void learn(){
		timesRun++;
		int roll;
		int pos = 0;
		for (int i = 0; i < numRolls; i++) {
			roll = Random.Range (1, 7) + Random.Range (1, 7);
			pos = (pos + roll) % 40;
			freq [pos]++;
			if (pos == 30)
				pos = 10;
		}
		for (int i = 0; i < nonHotelables.Length; i++)
			freq [nonHotelables [i]] = 0;
	}

	public bool wantsHouses(string[] properties){
		if (canPurchaseHouse (properties))
			for (int i = 1; i < properties.Length; i++)
				if (getPropByName(properties[i]).housePrice * 2 <= money)
					return true;
		return false;
	}

	public string buyHouses(string[] properties){
		string output = "";
		output += pname + " bought: \n";
		int boughtHouses = 0;
		Spots[] props = new Spots[properties.Length - 1];
		for (int i = 0; i < props.Length; i++)
			props [i] = getPropByName (properties [i + 1]);
		Spots best = findBestHouse (props);
		while (best != null && money >= best.housePrice * 2) {
			money -= best.housePrice;
			best.houses++;
			boughtHouses++;
			if (best.houses == 5) {
				output += boughtHouses + " houses on " + best.name + "\n";
				boughtHouses = 0;
				best = findBestHouse (props);
			}
		}
		return output;
	}

	public Spots getPropByName(string p){
		foreach (Spots s in owns) {
			if (s.name.Equals (p))
				return s;
		}
		return null;
	}

	private Spots findBestHouse(Spots[] props){
		float max = 0;
		int index = -1;
		for (int i = 0; i < props.Length; i++)
			if (freq [props [i].index] > max && props[i].houses < 5) {
				max = freq [props [i].index];
				index = i;
				//Debug.Log (index);
			}
		if (index == -1)
			return null;
		return props [index];
	}

}
