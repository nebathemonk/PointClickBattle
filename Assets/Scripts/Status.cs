using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status : MonoBehaviour {

    Character owner;

    [SerializeField]
    public string Name;

    [SerializeField]
    TurnStep turnStep;
    public TurnStep TurnStep { get { return turnStep; } set { Debug.Log("Can't change status step during runtime"); return; } }

    [SerializeField]
    StatusType type;
    public StatusType Type { get { return type; } set { return; } }

    [SerializeField][Tooltip ("How many copies of the status a character can have at a time")]
    int stack;
    public int Stack { get { return stack; } set { if (value > 0) stack = value; } }

    [SerializeField]
    int turnsLeft;
    public int TurnsLeft { get { return turnsLeft; } set { if(value > 0) turnsLeft = value; } }

    [SerializeField]
    List<Function> functionList = new List<Function>();

    [SerializeField]
    [EnumFlag]
    public Element element;

    //
    //GET AND SET METHODS
    //
    public void SetOwner(Character newOwner)
    {
        owner = newOwner;
        //activate the functions that activate OnSet
        foreach (Function f in functionList)
        {
            if (!f.hasActivated && f.ActivateOn == Activation.onSet)
            {
                f.Activate(owner, owner);
            }
        }
    }

    //
    //Utility Methods
    //

    void CountDownTurns()
    {
        //intrinsics never expire
        if(type != StatusType.intrinsic)
        {
            turnsLeft -= 1;
        }        
    }

    public void Remove()
    {
        //the status is being removed from the player
        foreach(Function f in functionList)
        {
            if(!f.hasActivated && f.ActivateOn == Activation.onEnd)
            {
                f.Activate(owner, owner, element);
            }
        }

        //kills the gameobject, removing itself from the character, hopefully
        //owner.RemoveStatus(this);
        Destroy(this.gameObject);
    }

    public void RefreshFunctions(Activation typeToRefresh)
    {
        //usually called by the character ending a turn, or the gc ending a round
        foreach(Function f in functionList)
        {
            if(f.ActivateOn == typeToRefresh)
            {
                //tell the function it can be used again
                f.Reset();
            }
        }
    }

    public IEnumerator Activate(Activation typeToActivate)
    {
        //called from character.CheckStatus()
        foreach (Function f in functionList)
        {
            if (!f.hasActivated && f.ActivateOn == typeToActivate)
            {
                f.Activate(owner, owner, element);
            }
            
        }

        //play the correct animation
        if (this.type == StatusType.buff)
        {
            yield return StartCoroutine(owner.PlayAnimation("Buff"));
        }
        if (this.type == StatusType.debuff)
        {
            yield return StartCoroutine(owner.PlayAnimation("Damaged"));
        }

        CountDownTurns();
    }
}

public enum StatusType { buff, debuff, intrinsic};