using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spots {
	public Object tempHolder;
	//public LinkedList<Object> currentPieces;
	public Player owner;
	public int price;
	public string specialType, taxType, name;
	public bool isCommunity, isChance, isTax, isSpecial, isRailroad;
	public int index;
	public int[] rent;
	public int houses, housePrice;

	public Spots(int place, string n){
		//currentPieces = new LinkedList<Object> ();
		owner = null;
		isCommunity = false;
		isChance = false;
		isTax = false;
		isSpecial = false;
		isRailroad = false;
		index = place;
		name = n;
	}

	public void setCommunity(){
		isCommunity = true;
	}

	public void setChance(){
		isChance = true;
	}

	public void setTax(string t){
		isTax = true;
		taxType = t;
	}

	public void setSpecial(string s){
		isSpecial = true;
		specialType = s;
	}

	public void setRR(){
		isRailroad = true;
	}

	public void setPrice(int p){
		price = p;
	}

	public void setRent(int[] r){
		rent = r;
	}

	public int getRent(int roll){
		if (isTax) {
			if (taxType.Equals ("I"))
				return 200;
			else
				return 100;
		} else if (isSpecial) {
			if (specialType.Equals ("Free Parking"))
				return -500;
			if (!specialType.Equals ("Utility"))
				return 0;
			//Debug.Log ("Special");
			int numSpecial = 0;
			foreach (Spots s in owner.owns) {
				if (s.specialType.Equals("Utility"))
					numSpecial++;
			}
			if (numSpecial == 1)
				return roll * 4;
			return roll * 10;
		} else if (isRailroad) {
			int numRR = 0;
			foreach (Spots s in owner.owns) {
				if (s.isRailroad)
					numRR++;
			}
			return 25 * (int)Mathf.Pow (2, numRR - 1);
		}
		return rent[houses];
	}
}
