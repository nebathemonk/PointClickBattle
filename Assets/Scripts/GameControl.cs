using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UI))]
public class GameControl : MonoBehaviour {

    UI gui;

    TurnStep currentStep;

    List<Character> allCharacters;

    Character currentCharacter;
    List<Skill> currentSkills;
    Skill skillBeingUsed;

    bool gameStarted;

    int currentTurn;
    int numberOfCharacters;

	// Use this for initialization
	void Start () {

        gui = gameObject.GetComponent<UI>();
        //empties the list
        allCharacters = new List<Character>();
        currentSkills = new List<Skill>();
       
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

    public Character GetCurrentCharacter()
    {
        return currentCharacter;
    }

    public Skill GetCurrentSkill(int skillNumber)
    {
        return currentSkills[skillNumber];
    }

    public Skill GetSkillBeingUsed()
    {
        return skillBeingUsed;
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
        currentCharacter = allCharacters[currentTurn];
        //put the outline effect on the current character
        HighlightCharacter();
        //call the turn starting stuff from the character
        currentCharacter.MyTurn();
        //Debug.Log("Beginning turn "+currentTurn.ToString());
    }

    void SelectSkill()
    {
        //find skills will get the skills of the current character
        FindSkills();
        //then we update the UI
        gui.SetSkillButtons();
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

    public void UseSkill(int skillNumber)
    {
        //called when the user hits a button during the proper step
        //use the skill from the list in the proper spot
        if(currentStep != TurnStep.skill) { return; }
        try
        {
            Debug.Log(currentCharacter.Name + " used " + currentSkills[skillNumber].Name);
            //have the character use the skill, check to see if they can
            currentCharacter.SelectSkill(skillNumber);
            //everything worked out, so we set that to the skill being used
            skillBeingUsed = currentSkills[skillNumber];
            //move to the next step
            NextStep();
        }
        catch
        {
            Debug.Log(currentCharacter.Name + " does not have that skill.");
        }
        
    }

    void FindSkills()
    {
        //called at the beginning of the select skill step
        //get all the skills of the current character

        currentSkills = new List<Skill>();  //empty list
        //look for the skills of the current character
        currentSkills = currentCharacter.GetSkills();
    }

    void HighlightCharacter()
    {
        //finds the outlineEffect that should be in the main camera
        OutlineEffect outliner = Camera.main.gameObject.GetComponent<OutlineEffect>();
        //sets the current characters sprite renderer to the one that should be outlined.
        outliner.outlineRenderers[0] = currentCharacter.GetComponentInChildren<SpriteRenderer>();
    }

    public void StartGame()
    {
        FindCharacters();
        gameStarted = true;
        currentTurn = -1;        
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
            try
            {
                Character foundCharacter = c.GetComponent<Character>();
                //if there is a living character script attached, add one to the character list
                if (foundCharacter.isAlive)
                {
                    allCharacters.Add(foundCharacter);
                    numberOfCharacters++;
                }
            }
            catch
            {
                //Debug.Log("What the actual fuck? No characters found on "+c.name);
            }
        }

        //Sort the character list by speed, to see who goes first
        allCharacters.Sort((x, y) => y.speed.CompareTo(x.speed));

        //Debug.Log("Total characters found: " + numberOfCharacters.ToString());
    }


    public void TestTarget(Character attacker, Character target, Skill skillToUse)
    {
        bool success = false;

        //make a list of all of the targets, for attacks that hit multiple
        List<Character> targets = new List<Character>();

        //test that the target is legal for the skill
        switch (skillToUse.targetGroup)
        {
            case SkillTarget.singleEnemy:
                //Targetting single enemy
                if(attacker.NPC != target.NPC)
                {
                    //diff teams, attack
                    targets.Add(target);
                    //Debug.Log("target... success");
                    success = true;
                }
                else
                {
                    //didn't work
                    //Debug.Log("must target an enemy with " + skillBeingUsed.Name);
                }
                break;
            case SkillTarget.singleTeam:
                //Targetting single ally
                if (attacker.NPC == target.NPC)
                {
                    targets.Add(target);
                    //Debug.Log("target... success");
                    success = true;
                }
                else
                {
                    //didn't work
                    //Debug.Log("must target one of your teammates with " + skillBeingUsed.Name);
                }
                break;
            case SkillTarget.allEnemy:
                //Targetting all of the enemies
                if (attacker.NPC != target.NPC)
                {
                    //diff teams, attack
                    foreach (Character t in allCharacters)
                    {
                        if (attacker.NPC != t.NPC)
                        {
                            targets.Add(t);
                        }
                    }
                    //Debug.Log("target... success");
                    success = true;
                }
                else
                {
                    //didn't work
                    //Debug.Log("must target an enemy with " + skillBeingUsed.Name);
                }
                break;
            case SkillTarget.allTeam:
                //Targetting all of your team
                if (attacker.NPC == target.NPC)
                {
                    foreach (Character t in allCharacters)
                    {
                        if (attacker.NPC != t.NPC)
                        {
                            targets.Add(t);
                        }
                    }
                    //Debug.Log("target... success");
                    success = true;
                }
                else
                {
                    //didn't work
                    //Debug.Log("must target your team with " + skillBeingUsed.Name);
                }
                break;
            case SkillTarget.self:
                //targetting yourself
                if (attacker == target)
                {
                    //Debug.Log("target... success");
                    success = true;
                }
                else
                {
                    //Debug.Log("must target yourself with " + skillBeingUsed.Name);
                }
                break;

        }

        if(success)
        {
            //play the proper type of animation
            switch (skillBeingUsed.type)
            {
                case SkillType.melee:
                    StartCoroutine(attacker.PlayAnimation("Melee"));
                    break;
                case SkillType.ranged:
                    StartCoroutine(attacker.PlayAnimation("Ranged"));
                    break;
                case SkillType.self:
                    StartCoroutine(attacker.PlayAnimation("Buff"));
                    break;

            }
            //hit everyone that was targetted
            foreach(Character t in targets)
            {
                t.AsTarget(attacker, skillBeingUsed);
            }
            NextStep();
        }
        //play the melee attack animation from the attacker
            //attacker.PlayAnimation(1);
        //play the 'get damaged' animation of the target
            //target.PlayAnimation(3);
    }

}


public enum TurnStep { begin,skill,target,end };
