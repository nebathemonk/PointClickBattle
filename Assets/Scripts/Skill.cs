using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    Character owner;

    public string Name;
            
    public SkillType type;
    public SkillMethod damageMethod;
    public SkillTarget targetGroup;
    [SerializeField][EnumFlag]
    public SkillDamage damageType;    
    [SerializeField][EnumFlag]
    public SkillElement damageElement;

    [SerializeField]
    int damage;
    [SerializeField]
    [Tooltip("Percentage of characters total stamina to use")]
    double staminaUse;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //
    //Get and Set methods
    //

    public void SetOwner(Character nOwner)
    {
        owner = nOwner;
    }

    //
    //Utility methods
    //

    public void Activate(Character target)
    {

        Debug.Log(owner.Name + " uses " + this.Name + " on " + target.Name);

        //Damaging skill
        if (damageType != SkillDamage.none)
        {
            target.DamageHealth(damage);
        }

        //use the owners stamina
        UseStamina();

    }

    void UseStamina()
    {
        int oStamina = owner.GetMaxStamina();
        int totalStaminaUsed = System.Convert.ToInt32(oStamina / staminaUse);
        owner.DamageStamina(totalStaminaUsed);
    }

    public bool CheckStamina()
    {
        //get the players max stamina, find the amount each skill use costs
        int oStamina = owner.GetMaxStamina();
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
[System.Flags]
    public enum SkillDamage { none, blunt, pierce, slash, magic};
    public enum SkillMethod { touch, physical, magic, psychic};
    public enum SkillElement { earth, wind, water, fire, lite, dark, ice, pois, elec};


