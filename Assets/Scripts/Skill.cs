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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetOwner(Character nOwner)
    {
        owner = nOwner;
    }
}

    public enum SkillTarget { self, singleEnemy, singleTeam, allEnemy, allTeam };
    public enum SkillType { self, melee, ranged };
[System.Flags]
    public enum SkillDamage { none, blunt, pierce, slash, magic};
    public enum SkillMethod { touch, physical, magic, psychic};
    public enum SkillElement { earth, wind, water, fire, lite, dark, ice, pois, elec};


