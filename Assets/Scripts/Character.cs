using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    GameControl GC;

    public bool isAlive;

    public bool NPC;
    public string Name;

    public int speed;

	// Use this for initialization
	void Start () {

        GC = GameObject.Find("GameControl").GetComponent<GameControl>();

        isAlive = true;

	}

    void OnMouseDown()
    {
        if (GC.GetStep() == TurnStep.target)
        {
            AsTarget();
        }
    }


    //
    //UTILITY METHODS
    //
    void AsTarget()
    {
        Debug.Log(Name + " was selected as target!");
        GC.TestTarget(this);
    }

    public void MyTurn()
    {
        Debug.Log(Name + " starts their turn.");
    }
}
