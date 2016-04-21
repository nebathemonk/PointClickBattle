using UnityEngine;
using System.Collections;

public static class C {

    //find the damage of the attack
    public static int Damage(int SkillDmg, int AttackerAtk, int TargetDef, int TargetEva)
    {
        int baseDmg = (((SkillDmg * AttackerAtk) * AttackerAtk) / (TargetDef * TargetEva));
        return baseDmg;
    }

    public static int CritDamage(int baseDamage, int SkillCritMultiplier)
    {
        int critDamage = (baseDamage * (SkillCritMultiplier));
        return critDamage;
    }

    public static int Weaknesses()
    {
        return 0;
    }

    //find out how average the attack COULD do
    public static int EstimateDamage(int Dmg, int CritDmg, int HitChance, int CritChance)
    {
        int avgDmg = ((Dmg * HitChance) + (CritDmg * CritChance));
        return avgDmg;
    }

    //check to see if the attack hits
    public static int checkHit(int AttackerAtk, double AttackerCrit, int TargetEva, double SkillAcc, double SkillCrit)
    {

        double chance = (((AttackerAtk - TargetEva) / 100) + (SkillAcc)) * 100;
        double critChance = ((AttackerCrit) + (SkillCrit) + ((chance/100) - 1)) * 100;
        Debug.Log("AttackerCrit: " + AttackerCrit.ToString() + " SkillCrit:" + SkillCrit.ToString() + " Chance/100 -1 :" + ((chance / 100) - 1).ToString() + " Total: " + critChance.ToString());

        //checkCrit();
        int roll = Random.Range(0, 100);
        Debug.Log("Roll: "+roll.ToString());
        if(roll <= chance)
        {
            Debug.Log("hit");
            if (roll <= critChance)
            {
                Debug.Log("Crit");
                return 2;
            }
            return 1;
        }
        return 0;
    }

    //check to see if the attack would be a critical hit
    /*
    public static bool checkCrit(double AttackerCrit, double SkillCrit, double hitChance)
    {
        double critChance = (AttackerCrit * 100) + (SkillCrit * 100) + ((hitChance) - 1);
        int roll = Random.Range(0, 100);
        
        if (roll <= critChance)
        {
            return true;
        }
        return false;
    }*/

}
