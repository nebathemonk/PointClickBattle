using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {

    GameControl GC;
    Text stepText;
    Text turnText;

	// Use this for initialization
	void Start () {

        GC = GameObject.Find("GameControl").GetComponent<GameControl>();
        stepText = GameObject.Find("StepText").GetComponent<Text>();
        turnText = GameObject.Find("TurnText").GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {

        stepText.text = "Current Step: " + GC.GetStep().ToString();
        turnText.text = "Current Turn: " + GC.GetTurn().ToString();
	
	}
}
