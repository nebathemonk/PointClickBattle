using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    GameControl GC;

    AudioSource IdleSound;
    AudioSource HurtSound;
    AudioSource BuffSound;

    Text healthText;
    Text staminaText;
    Slider healthSlider;
    Slider staminaSlider;

    CanvasGroup canvas;

    [SerializeField]
    private bool isAlive;
    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }

    [SerializeField]
    private bool NPC;
    public bool IsNPC  { get { return NPC; } set { NPC = value; } }

    [SerializeField]
    public string Name;

    [SerializeField]
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } set { if (value > 0) maxHealth = value; } }
    int health;
    [SerializeField]
    private int maxStamina;
    public int MaxStamina { get { return maxStamina; } set { if (value > 0) maxStamina = value; } }
    int stamina;

    [SerializeField][EnumFlag]
    SkillDamage typeResistance;
    [SerializeField][EnumFlag]
    Element elementResistance;

    [SerializeField]
    private int baseSpeed;
    internal int tempSpeed;

    [SerializeField]
    private int baseAttack;
    internal int tempAttack;

    [SerializeField]
    private int baseSpirit;
    internal int tempSpirit;

    [SerializeField]
    private int baseDefense;
    internal int tempDefense;

    [SerializeField]
    private int baseWill;
    internal int tempWill;

    [SerializeField]
    private int baseAccuracy;
    internal int tempAccuracy;

    [SerializeField]
    private int baseEvasion;
    internal int tempEvasion;
 


    List<Skill> skills;
    public List<Skill> Skills { get { return skills; } set { Debug.Log("Don't clear the skills!"); return; } }

    [SerializeField]
    Skill skillOne = null;
    [SerializeField]
    Skill skillTwo = null;
    [SerializeField]
    Skill skillThree = null;
    [SerializeField]
    Skill skillFour = null;

    [SerializeField]
    List<Status> statuses;
    [SerializeField]
    Status intrinsicStatus = null;


	// Use this for initialization
	void Start () {

        GC = GameObject.Find("GameControl").GetComponent<GameControl>();
        canvas = GetComponentInChildren<CanvasGroup>();
        GetSoundClips();
        SetCanvasElements();

        //Set all the Stats ready, including health and stamina
        SetStartStats();

        //Add intrinsic will reset the status list to blank and add the set intrinsic
        AddIntrinsic();
        SetSkills();

        isAlive = true;



	}

    void SetCanvasElements()
    {
        
        //Canvas elements - get all the texts from the children
        Text[] canvasElements = gameObject.GetComponentsInChildren<Text>();
        foreach (Text t in canvasElements)
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
            /*
            if (t.gameObject.name == "TextAttack")
            {
                attackText = t;
            }
            if (t.gameObject.name == "TextDefense")
            {
                defenseText = t;
            }
            if (t.gameObject.name == "TextAccuracy")
            {
                accuracyText = t;
            }
            if (t.gameObject.name == "TextEvasion")
            {
                evasionText = t;
            }
            if (t.gameObject.name == "TextSpeed")
            {
                speedText = t;
            }
            if (t.gameObject.name == "TextSpirit")
            {
                spiritText = t;
            }
            if (t.gameObject.name == "TextWill")
            {
                willText = t;
            }
            */
        }
        
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
        foreach(Slider s in sliders)
        {
            if(s.gameObject.name == "SliderHealth")
            {
                healthSlider = s;
            }
            if(s.gameObject.name == "SliderStamina")
            {
                staminaSlider = s;
            }
        }
    }

    void GetSoundClips()
    {
        AudioSource[] clips = gameObject.GetComponents<AudioSource>();
        if(clips.Length > 0)
        {
            IdleSound = clips[0];
        }
        if(clips.Length > 1)
        {
            HurtSound = clips[1];
        }
        if (clips.Length > 2)
        {
            BuffSound = clips[2];
        }
    }


    void Update()
    {
        //update the canvas elements to reflect the characters current stats
        healthText.text = "Hp: " + health.ToString() + "/" + maxHealth.ToString();
        staminaText.text = "Sp " + stamina.ToString() + "/" + maxStamina.ToString();
        //slider bars for health/stamina
        if(healthSlider != null)
        {
            healthSlider.value = ((float)health / (float)maxHealth);
        }
        if (staminaSlider != null)
        {
            staminaSlider.value = ((float)stamina / (float)maxStamina);
        }

    }

    void OnMouseDown()
    {
        if (GC.GetStep() == TurnStep.target)
        {
            GC.TestTarget(GC.GetCurrentCharacter(), this, GC.GetSkillBeingUsed());
        }
    }

    void OnMouseExit()
    {
        GC.MouseExit();
        //canvas.alpha = 0;
        //hide the canvas mouse over info
    }

    void OnMouseEnter()
    {
        //display the mouse over info
        GC.MouseOver(this);        
    }

    //
    //GET AND SET METHODS
    //
    void AddIntrinsic()
    {
        statuses = new List<Status>();
        if(intrinsicStatus != null)
        {
            statuses.Add(Instantiate(intrinsicStatus));
            foreach (Status s in statuses)
            {
                s.SetOwner(this);
            }
        }        
    }

    void SetSkills()
    {
        //create the skill list from the set skills
        skills = new List<Skill>();

        //Add the skill if there is one to add from prefabs
        //if there isn't, add a null skill to keep numbering correct
        if(skillOne != null)
        {
            try
            {
                skills.Add(Instantiate(skillOne));
            }
            catch
            {
                //prefab didn't exist, don't freak out
                Debug.Log("Skill does not exist.");
                skills.Add(null);
            }            
        }
        else
        {
            skills.Add(null);
        }
        //  ///////////////////////////////////////////
        if(skillTwo != null)
        {
            try
            {
                skills.Add(Instantiate(skillTwo));
            }
            catch
            {
                //prefab didn't exist, don't freak out
                Debug.Log("Skill does not exist.");
                skills.Add(null);
            }
        }
        else
        {
            skills.Add(null);
        }
        // ////////////////////////////////////////////
        if (skillThree != null)
        {
            try
            {
                skills.Add(Instantiate(skillThree));
            }
            catch
            {
                //prefab didn't exist, don't freak out
                Debug.Log("Skill does not exist.");
                skills.Add(null);
            }
        }
        else
        {
            skills.Add(null);
        }
        // ///////////////////////////////////////////
        if (skillFour != null)
        {
            try
            {
                skills.Add(Instantiate(skillFour));
            }
            catch
            {
                //prefab didn't exist, don't freak out
                Debug.Log("Skill does not exist.");
                skills.Add(null);
            }
        }
        else
        {
            skills.Add(null);
        }
                
        //Set this character as the owner for each copy of these skills
        foreach (Skill s in skills)
        {
            if (s != null)
            {
                s.SetOwner(this);
            }
        }
    }


    //
    //Stat methods
    //

    public void SetStartStats()
    {
        //called when the player is first created, to set all the stats to the base numbers correctly
        NormalizeAttack();
        NormalizeAccuracy();
        NormalizeDefense();
        NormalizeEvasion();
        NormalizeSpeed();
        NormalizeWill();
        NormalizeSpirit();

        SetHealth(maxHealth);
        SetStamina(maxStamina);
    }

    public void ChangeAttack(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempAttack += System.Convert.ToInt32(change * baseAttack);
        }
        else
        {
            tempAttack += System.Convert.ToInt32(change);
        }        
    }

    public void ChangeDefense(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempDefense += System.Convert.ToInt32(change * baseDefense);
        }
        else
        {
            tempDefense += System.Convert.ToInt32(change);
        }
    }

    public void ChangeAccuracy(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempAccuracy += System.Convert.ToInt32(change * baseAccuracy);
        }
        else
        {
            tempAccuracy += System.Convert.ToInt32(change);
        }
    }

    public void ChangeEvasion(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempEvasion += System.Convert.ToInt32(change * baseEvasion);
        }
        else
        {
            tempEvasion += System.Convert.ToInt32(change);
        }
    }

    public void ChangeSpeed(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempSpeed += System.Convert.ToInt32(change * baseSpeed);
        }
        else
        {
            tempSpeed += System.Convert.ToInt32(change);
        }
    }
    public void ChangeWill(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempWill += System.Convert.ToInt32(change * baseWill);
        }
        else
        {
            tempWill += System.Convert.ToInt32(change);
        }
    }
    public void ChangeSpirit(double change, bool asPercent = true)
    {
        if (asPercent)
        {
            tempSpirit += System.Convert.ToInt32(change * baseSpirit);
        }
        else
        {
            tempSpirit += System.Convert.ToInt32(change);
        }
    }

    public void NormalizeAttack()
    {
        tempAttack = baseAttack;
    }
    public void NormalizeDefense()
    {
        tempDefense = baseDefense;
    }
    public void NormalizeAccuracy()
    {
        tempAccuracy = baseAccuracy;
    }
    public void NormalizeEvasion()
    {
        tempEvasion = baseEvasion;
    }
    public void NormalizeSpeed()
    {
        tempSpeed = baseSpeed;
    }
    public void NormalizeSpirit()
    {
        tempSpirit = baseSpirit;
    }
    public void NormalizeWill()
    {
        tempWill = baseWill;
    }

    public int GetSpeed()
    {
        return tempSpeed;
    }

    void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    void SetStamina(int newStamina)
    {
        stamina = newStamina;
    }

   public int GetStamina()
    {
        return stamina;
    }

    //
    //UTILITY METHODS
    //

   public void AddStatus(Status newStatus)
    {
        //add stacking later
        int totalStack = 0;
        foreach(Status s in statuses)
        {
            if(s.Name == newStatus.Name)
            {
                //they already have one of these statuses
                totalStack++;
            }
        }

        if(totalStack < newStatus.Stack)
        {
            //they don't have too many, so add the new one
            Status statusToAdd = Instantiate(newStatus);
            statusToAdd.SetOwner(this);
            statuses.Add(statusToAdd);

        }
    }

    public void RemoveStatus(Status toRemove)
    {
        for(int i = statuses.Count; i > 0; i--)
        {
            //remove the status if it is the one we're looking for
            if(statuses[i-1].GetType() == toRemove.GetType())
            {
                statuses.RemoveAt(i-1);
            }
        }
    }

    public void RemoveAllStatus(StatusType typeToRemove)
    {
        for (int i = statuses.Count; i > 0; i--)
        {
            //remove the status if it is the type we're looking for
            if (statuses[i - 1].Type == typeToRemove)
            {
                statuses.RemoveAt(i - 1);
            }
        }
    }



    public void Rest()
    {
        AddStamina(66, true);
        //Debug.Log(this.Name + " rested this turn. (heal stamina or whatever)");
        StartCoroutine(EndMyTurn());
    }

   public void Retaliate(int skillNum)
    {
        //don't let them retaliate against themself
        if (GC.GetCurrentCharacter() != this)
        {
            //make sure we have a skill to be used, heree
            if (skillNum <= skills.Count - 1)
            {
                Skill RetaliationSkill = skills[skillNum];
                //have the retaliator play their animation and sound
                RetaliationSkill.PlaySound();

                switch (RetaliationSkill.type)
                {
                    case SkillType.melee:
                        StartCoroutine(this.PlayAnimation("Melee"));
                        break;
                    case SkillType.ranged:
                        StartCoroutine(this.PlayAnimation("Ranged"));
                        break;
                    case SkillType.self:
                        StartCoroutine(this.PlayAnimation("Buff"));
                        break;
                }
                //do the do (attack the current character as this character retaliating)
                StartCoroutine(GC.GetCurrentCharacter().AsTarget(this, RetaliationSkill));
            }
        }
    }

   public void LoseTurn()
    {
        GC.LoseTurn(this);
    }

    public void AddTurn()
    {
        GC.AddTurn(this);
    }

    public void DamageHealth(double change, Element elements = Element.N,
        SkillDamage damageType = SkillDamage.none, bool asPercent = false)
    {
        //check the resistances, set so that one resistance in each type
        //will give the full quater resistance. Doubles do not stack.
        double resistMod = 0;
        if(typeResistance != SkillDamage.none && (typeResistance & damageType) != 0)
        {
            resistMod += 0.25;
        }
        if (elementResistance != Element.N && (elementResistance & elements) != 0)
        {
            resistMod += 0.25;
        }

        //incorporate the resistances
        if(resistMod != 0) { change *= resistMod; }

        //figure out the damage
        if (asPercent)
        {
            health -= System.Convert.ToInt32(change * maxHealth);
        }
        else
        {
            health -= System.Convert.ToInt32(change);
        }
        
        //deathcheck
        if (health <= 0)
        {
            Debug.Log(this.Name + " is dead!");
            //DeathCheck()?
        }
    }

    public void AddHealth(double change, bool asPercent = false)
    {
        int tempHealth;
        if (asPercent)
        {
            //add hp as a percentage as the maxhealth
            tempHealth = health + System.Convert.ToInt32(maxHealth * change);
        }
        else
        {
            tempHealth = health + System.Convert.ToInt32(change);
        }
        
        if ((tempHealth) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += System.Convert.ToInt32(change);
        }

    }

    public void DamageStamina(double change, bool asPercent = false)
    {
        if (asPercent)
        {
            stamina -= System.Convert.ToInt32(maxStamina * change);
        }
        else
        {
            stamina -= System.Convert.ToInt32(change);
        }        
        if (stamina < 0)
        {
            stamina = 0;
        }
    }

    public void AddStamina(double change, bool asPercent = false)
    {
        int tempStamina;
        if (asPercent)
        {
            tempStamina = stamina + System.Convert.ToInt32(maxStamina * change);
        }
        else
        {
            tempStamina = stamina + System.Convert.ToInt32(change);
        }

        if (tempStamina > maxStamina)
        {
            stamina = maxStamina;
        }
        else
        {
            stamina += tempStamina;
        }
    }

    public IEnumerator AsTarget(Character attacker, Skill skillUsed)
    {
        //Debug.Log(Name + " was targted by "+attacker.Name+" for the skill "+skillUsed.Name);
        //play the get hit animation
        if (skillUsed.type == SkillType.self)
        {
            yield return StartCoroutine(PlayAnimation("Buff"));
        }
        else
        {
            yield return StartCoroutine(PlayAnimation("Damaged"));
        }

        StartCoroutine(skillUsed.Activate(this));
        CheckStatus(Activation.perTurn);
     
    }

    public void NewRound()
    {
        //called by GC when the new rounds begin
        foreach (Status s in statuses)
        {   //refresh the statuses that work each turn
            s.RefreshFunctions(Activation.perRound);
        }
    }

    public IEnumerator StartMyTurn()
    {
        //Debug.Log(Name + " starts their turn.");       
        //play the idle animation
        yield return StartCoroutine(PlayAnimation("Idle"));
        //look for the player to use a buff or take poison damage
        CheckStatus(Activation.perTurn);

        //cool down all of the characters skills
        foreach(Skill s in skills)
        {
            if (s != null)
            {
                s.Cool();
            }
        }

        //then tell GC to start the next step.
        if(GC.GetStep() == TurnStep.begin)
        {
            GC.NextStep();
        }
    }

    public IEnumerator EndMyTurn()
    {
        yield return new WaitForSeconds(1f);
        //check for any statuses to run first
        CheckStatus(Activation.perTurn);

        //Check all their statuses to see if they have expired
        RemoveOldStatus();

        //look through statuses left that need to refresh
        foreach (Status s in statuses)
        {   //refresh the statuses that work each turn
            s.RefreshFunctions(Activation.perTurn);
        }
        //tell the GC we can move one when we're done.
        yield return new WaitForSeconds(1f);
        GC.NextStep();
    }

    public void CheckStatus(Activation typeToActivate)
    {
        //look through the list of all the statuses the player has
        //and activate them at the current step
        foreach(Status s in statuses)
        {
            if(s.TurnStep == GC.GetStep())
            {
                StartCoroutine(s.Activate(typeToActivate));
            }            
        }
        //if(s.activateStep == GC.currentStep)
        //s.Activate()
    }

    public void RemoveOldStatus()
    {
        //statuses.Remove(oldStatus);
        
        //Debug.Log(Name + " is looking through list of status: "+statuses.Count.ToString());
        //called by a status when it has run out of time
        for (int i=statuses.Count; i > 0; i--)
        {
            //Debug.Log("Testing status " + (i-1).ToString());
            if(statuses[i-1].TurnsLeft < 1)
            {
                //Debug.Log(Name + " is removing status " + statuses[i-1].Name);
                statuses[i - 1].Remove();
                statuses.Remove(statuses[i-1]);
                //statuses.RemoveAt(i-1);
                //Destroy(oldStatus.gameObject);
            }
        }
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
            if(animationName == "Idle")
            {
                IdleSound.Play();
            }
            if (animationName == "Damaged")
            {
                HurtSound.Play();
            }
            if (animationName == "Buff")
            {
                BuffSound.Play();
            }
        }
        catch
        {
            Debug.Log("no animation found for current action. " + animationName + " for character " + Name);
            animationTime = 2f;
        }
        yield return new WaitForSeconds(animationTime+0.5f);
        //Debug.Log(animationName + " done playing for " + Name);

    }

}
