using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {

    Character owner;
    [SerializeField]
    public string Name;
            
    public SkillType type;
    public SkillMethod damageMethod;
    public SkillTarget targetGroup;
    [SerializeField][EnumFlag]
    public SkillDamage damageType;    
    [SerializeField][EnumFlag]
    public Element damageElement;

    [SerializeField]
    List<Function> functionList = new List<Function>();

    [SerializeField][Tooltip("Percentage of characters total stamina to use")]
    double staminaUse;
    public double StaminaUse { get { return staminaUse; } set { if(value > 0.01) staminaUse = value; } }

    [SerializeField][Tooltip("How many turns a character must wait to use the skill again")]
    int coolDown;
    public int CoolDown { get { return coolDown; } set { if (value > 0) coolDown = value; } }
    int coolDownTimer;

    //the sound effect to play when the character uses this skill
    AudioSource SFX;

    void Start()
    {
        SFX = gameObject.GetComponent<AudioSource>();
    }

    //
    //Get and Set methods
    //
    public void SetOwner(Character nOwner)
    {
        owner = nOwner;
    }

    public List<Function> GetFunctions()
    {
        return functionList;
    }

    //
    //Utility methods
    //

    public void Cool()
    {
        //called at the beginning of a characters turn
        //to cool a skill down if it was used before and has a timer
        if(coolDownTimer > 0)
        {
            coolDownTimer--;
        }        
    }

    public void CoolComplete()
    {
        //called to completely cool a skill down
        //used as part of a refresh skill function
        coolDownTimer = 0;
    }

    public void PlaySound()
    {
        //play the soundclip
        if(SFX != null)
        {
            SFX.Play();
        }        
    }

    public IEnumerator Activate(Character target)
    {
        //use the owners stamina
        UseStamina();

        //use the verbs functions we gave the skill
        foreach (Function f in functionList)
        {
            f.Activate(owner, target, damageElement, damageType);
        }

        //set the cooldowntimer, if there is a cool down on the skill
        coolDownTimer = coolDown;

        yield return new WaitForSeconds(1f);
    }

    void UseStamina()
    {
        int oStamina = owner.MaxStamina;
        int totalStaminaUsed = System.Convert.ToInt32(oStamina / staminaUse);
        owner.DamageStamina(totalStaminaUsed);
    }

    public int CheckCool()
    {
        return coolDownTimer;
    }

    public bool CheckStamina()
    {
        //check to make sure the skill is not on cool down
        if(coolDownTimer > 0) { return false; }

        //get the players max stamina, find the amount each skill use costs
        int oStamina = owner.MaxStamina;
        int totalStaminaUsed = System.Convert.ToInt32(oStamina / staminaUse);
        //check to see if they have enough stamina to use the skill
        if (totalStaminaUsed <= owner.GetStamina())
        {
            //tell them they can use the skill
            return true;
        }
        return false;
    }



}

    public enum SkillTarget { self, singleEnemy, singleTeam, allEnemy, allTeam };
    public enum SkillType { self, melee, ranged };
    public enum SkillMethod { touch, physical, magic, psychic };
[System.Flags]
    public enum SkillDamage { none = (1 << 0), blunt = (1 << 1), pierce = (1 << 2), slash = (1 << 3), magic = (1 << 4) };
//none, earth, water, air, fire, light, dark, psychic, poison, electric, ice
[System.Flags]
    public enum Element { N = (1 << 0), E = (1 << 1), W = (1 << 2), A = (1 << 3), F = (1 << 4),
    L = (1 << 5), D = (1 << 6), Py = (1 << 7), Pn = (1 << 8), El = (1 << 9), I = (1 << 10)};


