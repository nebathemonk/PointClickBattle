using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    TurnStep currentStep;

    List<Character> allCharacters;

    bool gameStarted;

    int currentTurn;
    int numberOfCharacters;

	// Use this for initialization
	void Start () {

        //empties the list
        allCharacters = new List<Character>();
       
        //calls some features that we need to begin, such as finding characters
        //StartGame();
        gameStarted = false;
	
	}

    //
    //GET AND SET COMMANDS
    //

    public TurnStep GetStep()
    {
        return currentStep;
    }

    public int GetTurn()
    {
        return currentTurn;
    }

    //
    //TURN CONTROL PROCEDURES
    //
    public void NextStep()
    {
        if(!gameStarted)
        {
            currentStep = TurnStep.begin;
            StartGame();
            return;
        }
        if (currentStep == TurnStep.end)
        {
            currentStep = 0;
        }
        else
        {
            currentStep++;
        }


        switch (currentStep)
        {
            case TurnStep.begin:
                BeginTurn();
                break;
            case TurnStep.skill:
                SelectSkill();
                break;
            case TurnStep.target:
                SelectTarget();
                break;
            case TurnStep.end:
                EndTurn();
                break;
            default:
                Debug.Log("Step counter broken.");
                break;
        }


    }

    void BeginTurn()
    {
        if (currentTurn < numberOfCharacters)
        {
            currentTurn++;
        }
        else
        {
            //set turn counter back to zero because we're at the end of the round
            currentTurn = 0;
        }
        allCharacters[currentTurn].MyTurn();
        //Debug.Log("Beginning turn "+currentTurn.ToString());
    }

    void SelectSkill()
    {
        Debug.Log("Selecting Skill...");
    }

    void SelectTarget()
    {
        Debug.Log("Selecting Target...");
    }

    void EndTurn()
    {
        Debug.Log("Ending turn " + currentTurn.ToString());
    }

    //
    //UTILITY METHODS
    //

    public void StartGame()
    {
        gameStarted = true;
        currentTurn = -1;
        FindCharacters();
        BeginTurn();
    }

    public void FindCharacters()
    {
        //start the number at -1 so that we'll have 0 with one character
        numberOfCharacters = -1;
        //find all of the characters currently in the battle
        GameObject[] GO = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject c in GO)
        {
            Character foundCharacter = c.GetComponent<Character>();
            //if there is a living character script attached, add one to the character list
            if (foundCharacter.isAlive)
            {
                allCharacters.Add(foundCharacter);
                numberOfCharacters++;
            }
        }

        //Sort the character list by speed, to see who goes first
        allCharacters.Sort((x, y) => y.speed.CompareTo(x.speed));

        Debug.Log("Total characters found: " + numberOfCharacters.ToString());
    }


    public void TestTarget(Character targ)
    {
        Debug.Log("Testing target... success");
        NextStep();
    }

}


public enum TurnStep { begin,skill,target,end };
