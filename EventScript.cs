using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour {

	private static readonly int NUM_PARAMS = 6;

	public GameScript game;
	private bool goTo;
	private bool[] param;
	private string place;
	// Use this for initialization
	void Start () {
		param = new bool[NUM_PARAMS];
		game.color = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void setTrue(){
		for (int i = 0; i < param.Length; i++)
			param [i] = true;
	}

	public void startRoll(){
		StartCoroutine (startTimer ());
		setTrue ();
		param[0] = false;
	}

	public void startBuy(){
		StartCoroutine (startTimer ());
		setTrue ();
		param[1] = false;
	}

	public void startNothing(){
		StartCoroutine (startTimer ());
		setTrue ();
		param[2] = false;
	}

	public void startHouses(bool to){
		StartCoroutine (startTimer ());
		setTrue ();
		param[3] = false;
		goTo = to;
	}

	public void stop(){
		setTrue ();
	}

	public void buyHouse(string co){
		
		setTrue ();
		game.color = co;
		if (!game.color.Equals ("") && game.housePos != 0) {
			StartCoroutine (startTimer ());
			param [4] = false;
		}
	}

	public void addPosition(int i){
		setTrue ();
		game.housePos = i;
		if (!game.color.Equals ("") && game.housePos != 0) {
			param [4] = false;
			StartCoroutine (startTimer ());
		}
	}

	public void zoom(string p){
		setTrue ();
		place = p;
		param [5] = false;
		StartCoroutine (startTimer ());
	}

	public IEnumerator startTimer(){
		yield return new WaitForSeconds (1f);
		if (!param [0]) {
			game.roll ();
		} else if (!param [1]) {
			game.buy ();
		} else if (!param [2]) {
			game.doNothing ();
		} else if (!param [3] && goTo) {
			game.switchToHouses ();
		} else if (!param [3] && !goTo) {
			game.switchToBoard ();
		} else if (!param [4]) {
			game.addHouse ();
			game.color = "";
			game.housePos = 0;
		} else if (!param [5]) {
			game.zoom (place);
		}
	}


}
