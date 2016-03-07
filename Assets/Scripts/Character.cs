using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    GameControl GC;

    bool animating;

    public bool isAlive;

    public bool NPC;
    public string Name;
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

        //create the skill list from the set skills
        skills = new List<Skill>();
        skills.Add(skillOne);
        skills.Add(skillTwo);
        skills.Add(skillThree);
        skills.Add(skillFour);

        isAlive = true;
        animating = false;
	}

    void OnMouseDown()
    {
        if (GC.GetStep() == TurnStep.target)
        {
            GC.TestTarget(GC.GetCurrentCharacter(), this, GC.GetSkillBeingUsed());
        }
    }


    //
    //UTILITY METHODS
    //
    public void AsTarget(Character attacker, Skill skillUsed)
    {
        Debug.Log(Name + " was targted by "+attacker.Name+" for the skill "+skillUsed.Name);
        //play the get hit animation
        StartCoroutine(PlayAnimation("Damaged"));
        
    }

    public void MyTurn()
    {
        Debug.Log(Name + " starts their turn.");
        //play the idle animation
        StartCoroutine(PlayAnimation("Idle"));
    }

    public List<Skill> GetSkills()
    {
        return skills;
    }

    public void SelectSkill(int skillNumber)
    {
        Debug.Log(Name + " is targetting for " + skills[skillNumber].name);
        //skill selected, move on to finding a target
    }

    public IEnumerator PlayAnimation(string animationName)
    {
        Animation animation;
        float animationTime;

        try
        {
            animation = gameObject.GetComponent<Animation>();
            animationTime = animation[animationName].length;
            animation.Play(animationName);
        }
        catch
        {
            Debug.Log("no animation found for current action. " + animationName + " for character " + Name);
            animationTime = 2f;
        }

        yield return new WaitForSeconds(animationTime);
        Debug.Log(animationName + " done playing for " + Name);

        /*
        try
        {
            anim.SetTrigger(animationName);
            animating = true;
            //anim.Play(animationName);
        }
        catch
        {
            Debug.Log("no animation found for current action. " + animationName + " for character " + Name);
        }

        if (animating)
        {
            yield return new WaitForEndOfFrame;
        }
        //pass the name of the animation, it finds the animation
        //and waits for the animation to finish
        Debug.Log(animationName + " done playing for "+Name);
        */
    }
    /*
    public void AnimationOver()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Melee", false);
        anim.SetBool("Ranged", false);
        anim.SetBool("Damaged", false);
        anim.SetBool("Buff", false);
        animating = false;
    }*/
}
