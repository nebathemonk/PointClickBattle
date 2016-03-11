using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    GameControl GC;
    Text healthText;
    Text staminaText;

    public bool isAlive;

    public bool NPC;
    public string Name;
    public int maxHealth;
    int health;
    public int maxStamina;
    int stamina;
    public int speed;

    List<Skill> skills;

    [SerializeField]
    Skill skillOne;
    [SerializeField]
    Skill skillTwo;
    [SerializeField]
    Skill skillThree;
    [SerializeField]
    Skill skillFour;


	// Use this for initialization
	void Start () {

        GC = GameObject.Find("GameControl").GetComponent<GameControl>();

        //Canvas elements - get all the texts from the children
        Text[] canvasElements = gameObject.GetComponentsInChildren<Text>();
        foreach(Text t in canvasElements)
        {
            //health text object
            if (t.gameObject.name == "TextHealth")
            {
                healthText = t;
            }
            if (t.gameObject.name == "TextStamina")
            {
                staminaText = t;
            }
        }

        //create the skill list from the set skills
        skills = new List<Skill>();
        skills.Add(Instantiate(skillOne));
        skills.Add(skillTwo);
        skills.Add(skillThree);
        skills.Add(skillFour);
        //Set this character as the owner for each copy of these skills
        foreach (Skill s in skills)
        {
            if (s != null)
            {
                s.SetOwner(this);
            }
        }

        isAlive = true;

        //Set all the Stats ready
        SetHealth(maxHealth);
        SetStamina(maxStamina);
	}


    void Update()
    {
        //update the canvas elements to reflect the characters current stats
        healthText.text = "Health: " + health.ToString();
        staminaText.text = "Stamina " + stamina.ToString();
    }

    void OnMouseDown()
    {
        if (GC.GetStep() == TurnStep.target)
        {
            GC.TestTarget(GC.GetCurrentCharacter(), this, GC.GetSkillBeingUsed());
        }
    }

    //
    //GET AND SET METHODS
    //
    void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    void SetStamina(int newStamina)
    {
        stamina = newStamina;
    }

   public int GetStamina()
    {
        return stamina;
    }

    public int GetMaxStamina()
   {
       return maxStamina;
   }

    //
    //UTILITY METHODS
    //

    public void DamageHealth(int change)
    {
        health -= change;
        if (health <= 0)
        {
            Debug.Log(this.Name + " is dead!");
            //DeathCheck()?
        }
    }

    public void AddHealth(int change)
    {
        int tempHealth = health + change;
        if ((tempHealth) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += change;
        }

    }

    public void DamageStamina(int change)
    {
        stamina -= change;
        if (stamina < 0)
        {
            stamina = 0;
        }
    }

    public void AddStamina(int change)
    {
        int tempStamina = stamina + change;
        if (tempStamina > maxStamina)
        {
            stamina = maxStamina;
        }
        else
        {
            stamina += change;
        }
    }

    public IEnumerator AsTarget(Character attacker, Skill skillUsed)
    {
        //Debug.Log(Name + " was targted by "+attacker.Name+" for the skill "+skillUsed.Name);
        //play the get hit animation
        skillUsed.Activate(this);
        yield return StartCoroutine(PlayAnimation("Damaged"));
        
    }

    public IEnumerator StartMyTurn()
    {
        //Debug.Log(Name + " starts their turn.");
        //play the idle animation
        yield return StartCoroutine(PlayAnimation("Idle"));
        //look for the player to use a buff or take poison damage
        //then tell GC to start the next step.
        GC.NextStep();
    }

    public IEnumerator EndMyTurn()
    {
        //Debug.Log(Name + " ends their turn.");
        //look for the player to use a buff or take poison damage
        //then tell GC to start the next step.
        yield return new WaitForSeconds(1f);
        GC.NextStep();
    }

    public void CheckStatus()
    {
        //look through the list of all the statuses the player has
        //and activate them at the current step
        //foreach(Status s in statuses)
        //if(s.activateStep == GC.currentStep)
        //s.Activate()
    }

    public List<Skill> GetSkills()
    {
        return skills;
    }

    public bool SelectSkill(int skillNumber)
    {
        //run a test to make sure we have enough stamina
        bool staminaTest;
        try
        {
            staminaTest = skills[skillNumber].CheckStamina();
        }
        catch
        {
            staminaTest = false;
        }
        

        if (skills[skillNumber] != null && staminaTest)
        {
            //they have the skill, and enough stamina to use it
            return true;
        }
        else
        {
            return false;
        }
        //Debug.Log(Name + " is targetting for " + skills[skillNumber].name);
        //skill selected, move on to finding a target
    }

    public IEnumerator PlayAnimation(string animationName)
    {
        Animation animation;
        float animationTime;

        try
        {
            animation = gameObject.GetComponentInChildren<Animation>();
            animationTime = animation[animationName].length;
            animation.Play(animationName);
        }
        catch
        {
            Debug.Log("no animation found for current action. " + animationName + " for character " + Name);
            animationTime = 2f;
        }

        yield return new WaitForSeconds(animationTime);
        //Debug.Log(animationName + " done playing for " + Name);

    }

}
