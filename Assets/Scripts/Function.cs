using UnityEngine;
using System.Collections;

[System.Serializable]
public class Function{

    [SerializeField]
    Activation activation; 
    public Activation ActivateOn { get { return activation; } set { return; } }
    public bool hasActivated;

    [SerializeField]
    Verb actionVerb;
    [SerializeField][Tooltip("Percent chance this function will activate")]
    double chance;

    [SerializeField][Tooltip("Any number the function might need, varies by function")]
    double amount;
    [SerializeField][Tooltip("Check to use amount as percent, useful for drain attacks")]
    bool asPercent;

    [SerializeField][Tooltip("The chance that the function will critical hit")]
    double critRate;

    [SerializeField][Tooltip("The stat the function will effect or use, depending")]
    Stat stat;

    [SerializeField][Tooltip("This is the status to add or remove")]
    Status statusEffect;

    public Verb GetVerb()
    {
        return actionVerb;
    }

    public void Reset()
    {
        hasActivated = false;
    }

    //used to determine in the function will activate, out of 100%
    bool RollChance()
    {
        //if check was left blank, attack can't fail. No reason for a 0% chance
        int roll = System.Convert.ToInt32(Random.Range(0f, 100f));
        int check = System.Convert.ToInt32(chance * 100);
        //a zero in the field makes a function that doesn't need to roll
        if(check == 0) { return true; }
        if(roll <= check)
        {
            //the roll was less than the chance, so this was a success
            //Debug.Log("chance was rolled " + roll.ToString() + ", success under " + check.ToString());
            return true;
        } 
        return false;
    }

    public void Activate(Character user, Character target, Element elements = Element.N,
        SkillDamage damageType = SkillDamage.none)
    {
        //roll the check to see if the function activates
        if (RollChance() && actionVerb != Verb.damage)
        {
            switch (actionVerb)
            {
                //drain will hurt a stat
                case Verb.drain:
                    switch (stat)
                    {
                        case Stat.attack:
                            target.ChangeAttack(amount * -1, asPercent);
                            break;
                        case Stat.defense:
                            break;
                        case Stat.health:
                            break;
                        case Stat.stamina:
                            break;
                        case Stat.speed:
                            break;
                        default:
                            break;
                    }
                    Debug.Log(stat.ToString() + " drain on " + target.Name);
                    break;

                //boost will raise a stat
                case Verb.boost:
                    switch (stat)
                    {
                        case Stat.attack:
                            target.ChangeAttack(amount, asPercent);
                            break;
                        case Stat.defense:
                            break;
                        case Stat.health:
                            break;
                        case Stat.stamina:
                            break;
                        case Stat.speed:
                            break;
                        default:
                            break;
                    }
                    Debug.Log(stat.ToString() + " boost on " + target.Name);
                    break;

                //Heal can give back health
                case Verb.heal:
                    target.AddHealth(amount, asPercent);
                    //Debug.Log("heal on " + target.Name);
                    break;

                //stun stops the character from acting on their next turn
                case Verb.stun:
                    target.LoseTurn();
                    //Debug.Log("stun on " + target.Name);
                    break;

                //xturn gives the character an extra action/turn
                case Verb.xturn:
                    target.AddTurn();
                    //Debug.Log("xturn on " + target.Name);
                    break;

                //unBuff will remove all of the buffs on the character
                case Verb.unBuff:
                    target.RemoveAllStatus(StatusType.buff);
                    //Debug.Log("unBuff on " + target.Name);
                    break;

                //unDebuff will remove all debuffs on the character
                case Verb.unDebuff:
                    target.RemoveAllStatus(StatusType.debuff);
                    //Debug.Log("unDebuff on " + target.Name);
                    break;

                //Retaliate attacks or uses a skill when another skill is used on the character
                case Verb.retaliate:
                    //Debug.Log("Retaliate on " + target.Name);
                    target.Retaliate(System.Convert.ToInt32(amount));                    
                    break;

                    //Add a status - for attacks that add burning, super speed, etc
                case Verb.addStatus:
                    target.AddStatus(statusEffect);
                    //Debug.Log("Adding status on " + target.Name);
                    break;

                    //Remove a specific status from the target, like poison or whatever
                case Verb.removeStatus:
                    target.RemoveStatus(statusEffect);
                    //Debug.Log("Removing status on " + target.Name);
                    break;

                case Verb.restoreStamina:
                    //gives back some stamina, like a rest function
                    target.AddStamina(amount, asPercent);
                    break;

                case Verb.damageStamina:
                    //takes away some of the characters stamina
                    target.DamageStamina(amount, asPercent);
                    break;

                default:
                    Debug.Log("You tried to activate a verb that doesn't exist. " + actionVerb.ToString());
                    break;
            }

            //Activated this turn
            hasActivated = true;
        }   //end of roll check 


        //This was an attack function, so we skip that activating stuff
        else
        {
            if(actionVerb == Verb.damage && amount > 0)
            {
                //call the C method to find check if we hit, and how much damage we do
                int hit = C.checkHit(user.tempAccuracy, user.tempCritRatio, target.tempEvasion, chance, critRate);
                if (hit > 0)
                {
                    //figure out the damage
                    int tempDamage = C.Damage((int)amount, user.tempAttack, target.tempDefense, target.tempEvasion);
                    //check for a critical hit
                    if (hit > 1)
                    {
                        tempDamage = C.CritDamage(tempDamage, 2);
                    }
                    //hurt the target
                    target.DamageHealth(tempDamage, elements, damageType, asPercent);
                    //call the display to show the damage or MISS
                }
                else
                {
                    //we missed
                    target.DamageHealth(0, elements, damageType, asPercent);
                }
            }
        }
       
    } //end of activation

}//end of class

public enum Verb { damage, heal, drain, stun, xturn, boost, unBuff, unDebuff, retaliate,
    addStatus, removeStatus, restoreStamina, damageStamina};
public enum Stat { none, attack, defense, speed, health, stamina};
public enum Activation { onSet, perRound, perTurn, onEnd, onSkill};