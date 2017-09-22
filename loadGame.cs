using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void load(){
		StartCoroutine (startLoad ());

	}
	
	public IEnumerator startLoad(){
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("Monopoly");
	}
}
