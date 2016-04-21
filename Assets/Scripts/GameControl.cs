using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UI))]
public class GameControl : MonoBehaviour {

    UI gui;
    //the current step of the turn, used to tell what we're doing right now
    TurnStep currentStep;
    //all of the characters in the combat
    List<Character> allCharacters;
    //the turn order list, where a characte can have multiple turns
    List<Character> turnList;
    // info about the current turn that is being played
    Character currentCharacter;
    List<Skill> currentSkills;
    Skill skillBeingUsed;
    //Are we playing the game right now?
    bool gameStarted;
    //the current turn and the number of characters in easy to digest int format
    int currentTurn;
    int numberOfCharacters;

	// Use this for initialization
	void Start () {

        gui = gameObject.GetComponent<UI>();
        //empties the list
        allCharacters = new List<Character>();
        turnList = new List<Character>();
        currentSkills = new List<Skill>();
       
        //calls some features that we need to begin, such as finding characters
        //StartGame();
        gameStarted = false;
	
	}

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && currentStep == TurnStep.target)
        {
            //they hit escape, they want to cancel a move that they are targeting with
            CancelSkill();
        }
    }

    //
    //GET AND SET COMMANDS
    //

    public List<Character> GetTurnList()
    {
        return turnList;
    }

    public TurnStep GetStep()
    {
        return currentStep;
    }

    void SetStep(TurnStep newStep)
    {
        currentStep = newStep;
        //NextStep();
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
        if (currentTurn < turnList.Count-1)
        {
            currentTurn++;
        }
        else
        {
            //new round begins
            StartRound();
            //set turn counter back to zero because we're at the end of the round
            currentTurn = 0;            
        }
        currentCharacter = turnList[currentTurn];
        //put the outline effect on the current character
        HighlightCharacter();
        //call the turn starting stuff from the character
        StartCoroutine(currentCharacter.StartMyTurn());
        //Debug.Log("Beginning turn "+currentTurn.ToString());
    }

    public void StartRound()
    {
        //Debug.Log("New Round starting");
        turnList.Clear();
        turnList = new List<Character>();
        //Called from BeginTurn() when turn number is 0(new round)
        //update the turn order list now for the round, by speed stat
        //Sort the character list by speed, to see who goes first
        allCharacters.Sort((x, y) => y.GetSpeed().CompareTo(x.GetSpeed()));
        //set the turn list to be all of the characters
        turnList.AddRange(allCharacters);
        foreach (Character c in allCharacters)
        {
            //if a character has a skill that makes them go more than once, it will activate here
            c.NewRound();
        }
        /*
        //for debugging turnlist
        foreach(Character c in turnList)
        {
            Debug.Log("In Turnlist: " + c.Name);
        }
        */
    }

    void SelectSkill()
    {
        //find skills will get the skills of the current character
        FindSkills();
        //then we update the UI
        gui.SetSkillButtons();
        //Debug.Log("Selecting Skill...");
    }

    void SelectTarget()
    {
        //Debug.Log("Selecting Target...");
    }

    void EndTurn()
    {
        //Debug.Log("Ending turn " + currentTurn.ToString());
        StartCoroutine(currentCharacter.EndMyTurn());        
    }

    //
    //UTILITY METHODS
    //

    public void EstimateDamage(Character target)
    {
        int newLife;
        int newStamina;
        if(skillBeingUsed != null)
        {
            foreach(Function f in skillBeingUsed.GetFunctions())
            {
                if(f.GetVerb() == Verb.damage)
                {
                    //attack damages health, estimate that
                }
                if(f.GetVerb() == Verb.damageStamina)
                {
                    //attack damages stamina, estimate that
                }
            }
        }
    }

    public void MouseOver(Character pTarget)
    {
        if(currentStep == TurnStep.target)
        {
            //estimate an attack on the character
        }
        else
        {
            //not targetting, so show the characters info
            gui.MouseOverUpdate(pTarget);
        }
    }

    public void MouseExit()
    {
        gui.MouseOverClear();
    }

    public void Rest()
    {
        //The user hit the rest button, skipping turn, etc
        //make sure the game is started
        //and that they aren't in the middle of using a skill
        if (gameStarted && currentStep != TurnStep.target)
        {
            currentCharacter.Rest();
            SetStep(TurnStep.end);
        }
    
    }

    public void LoseTurn(Character character)
    {
        if(character == currentCharacter && currentStep == TurnStep.begin)
        {
            //they are losing their turn right now
            SetStep(TurnStep.end);
        }
        else
        {
            //they aren't taking their turn right now, so just take their next
            //one out of the turn list this round
            turnList.Remove(character);
        }
        
    }

    public void AddTurn(Character character)
    {
        //put the character into the turn list, default is next in line
        turnList.Insert(currentTurn+1, character);
    }

    public void UseSkill(int skillNumber)
    {
        //called when the user hits a button during the proper step
        //use the skill from the list in the proper spot
        if(currentStep != TurnStep.skill) { return; }
        //have the character use the skill, check to see if they can
        if (currentCharacter.SelectSkill(skillNumber))
        {
           //everything worked out, so we set that to the skill being used
            skillBeingUsed = currentSkills[skillNumber];
            //move to the next step
            NextStep();
        }
        else
        {
            Debug.Log("You can't use that right now...");
        }
    }

    void CancelSkill()
    {
        //use this when the player decides they did not mean to use that skill
        skillBeingUsed = null;
        currentStep = TurnStep.skill;
    }

    void FindSkills()
    {
        //called at the beginning of the select skill step
        //get all the skills of the current character

        currentSkills = new List<Skill>();  //empty list
        //look for the skills of the current character
        currentSkills = currentCharacter.Skills;
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
        //StartRound();     
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
                if (foundCharacter.IsAlive)
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
        allCharacters.Sort((x, y) => y.GetSpeed().CompareTo(x.GetSpeed()));

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
                if(attacker.IsNPC != target.IsNPC)
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
                if (attacker.IsNPC == target.IsNPC)
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
                if (attacker.IsNPC != target.IsNPC)
                {
                    //diff teams, attack
                    foreach (Character t in allCharacters)
                    {
                        if (attacker.IsNPC != t.IsNPC)
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
                if (attacker.IsNPC == target.IsNPC)
                {
                    foreach (Character t in allCharacters)
                    {
                        if (attacker.IsNPC != t.IsNPC)
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
                    targets.Add(attacker);
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
            //play the proper type of animation and sound
            skillBeingUsed.PlaySound();

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
                StartCoroutine(t.AsTarget(attacker, skillBeingUsed));
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
