using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public GameObject piece;
	public LinkedList<Spots> owns;
	public int money;
	public int position = 0;
	public bool lost = false;
	public string pname;
	public bool inJail = false;
	public int rollsInJail = 0;

	public Player(GameObject go, int m, string n){
		piece = go;
		owns = new LinkedList<Spots> ();
		money = m;
		pname = n;
	}

	public Player(){
		
	}

	public bool canBuy(Spots s){
		return s.owner == null && money >= s.price;
	}

	public bool updateLoss(){
		if (money < 0 && !lost) {
			lost = true;
			return true;
		}
		return false;
	}

	public bool canPurchaseHouse(string[] properties){
		for (int i = 1; i < properties.Length; i++)
			if (!hasPropByName(properties[i]))
				return false;
		return true;
	}

	public bool hasPropByName(string n){
		foreach (Spots s in owns)
			if (s.name.Equals (n))
				return true;
		return false;
	}
}
