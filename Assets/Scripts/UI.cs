using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    GameControl GC;
    Text stepText;
    Text turnText;

    Button SkillOneButton;
    Button SkillTwoButton;
    Button SkillThreeButton;
    Button SkillFourButton;

    Text attackText;
    Text defenseText;
    Text accuracyText;
    Text evasionText;
    Text speedText;
    Text willText;
    Text spiritText;

    // Use this for initialization
    void Start () {

        GC = GameObject.Find("GameControl").GetComponent<GameControl>();
        stepText = GameObject.Find("StepText").GetComponent<Text>();
        turnText = GameObject.Find("TurnText").GetComponent<Text>();

        SkillOneButton = GameObject.Find("SkillOneButton").GetComponent<Button>();
        SkillTwoButton = GameObject.Find("SkillTwoButton").GetComponent<Button>();
        SkillThreeButton = GameObject.Find("SkillThreeButton").GetComponent<Button>();
        SkillFourButton = GameObject.Find("SkillFourButton").GetComponent<Button>();

        attackText = GameObject.Find("TextAttackInfo").GetComponent<Text>();
        defenseText = GameObject.Find("TextDefenseInfo").GetComponent<Text>();
        spiritText = GameObject.Find("TextSpiritInfo").GetComponent<Text>();
        willText = GameObject.Find("TextWillInfo").GetComponent<Text>();
        evasionText = GameObject.Find("TextEvasionInfo").GetComponent<Text>();
        accuracyText = GameObject.Find("TextAccuracyInfo").GetComponent<Text>();
        speedText = GameObject.Find("TextSpeedInfo").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {

        stepText.text = "Current Step: " + GC.GetStep().ToString();
        //turnText.text = "Current Turn: " + GC.GetTurn().ToString();
        turnListUpdate();
	
	}

    public void MouseOverUpdate(Character c)
    {
        //info stats
        if (attackText != null) { attackText.text = "Atk: " + c.tempAttack.ToString(); }
        if (defenseText != null) { defenseText.text = "Def: " + c.tempDefense.ToString(); }
        if (accuracyText != null) { accuracyText.text = "Acc: " + c.tempAccuracy.ToString(); }
        if (evasionText != null) { evasionText.text = "Eva: " + c.tempEvasion.ToString(); }
        if (speedText != null) { speedText.text = "Spd: " + c.tempSpeed.ToString(); }
        if (spiritText != null) { spiritText.text = "Spt: " + c.tempSpirit.ToString(); }
        if (willText != null) { willText.text = "Wil: " + c.tempWill.ToString(); }
    }

    public void MouseOverClear()
    {
        if (attackText != null) { attackText.text = ""; }
        if (defenseText != null) { defenseText.text = ""; }
        if (accuracyText != null) { accuracyText.text = ""; }
        if (evasionText != null) { evasionText.text = ""; }
        if (speedText != null) { speedText.text = ""; }
        if (spiritText != null) { spiritText.text = ""; }
        if (willText != null) { willText.text = ""; }
    }

    void turnListUpdate()
    {
        string turns = "";
        List<Character> turnList = GC.GetTurnList();
        foreach(Character c in turnList)
        {
            turns += c.Name+"\n";
        }

        turnText.text = "Turn List: \n" + turns;
    }

    public void SetSkillButtons()
    {
        //get the skills and change the button functions
        try
        {
            SkillOneButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(0).Name;
            if(GC.GetCurrentSkill(0).CheckCool() > 0)
            {
                SkillOneButton.GetComponentInChildren<Text>().text += " T- " + GC.GetCurrentSkill(0).CheckCool().ToString();
            }
        }
        catch
        {
            SkillOneButton.GetComponentInChildren<Text>().text = "null";
        }
        try
        {
            SkillTwoButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(1).Name;
            if (GC.GetCurrentSkill(1).CheckCool() > 0)
            {
                SkillTwoButton.GetComponentInChildren<Text>().text += " T- " + GC.GetCurrentSkill(1).CheckCool().ToString();
            }
        }
        catch
        {
            SkillTwoButton.GetComponentInChildren<Text>().text = "null";
        }
        try
        {
            SkillThreeButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(2).Name;
            if (GC.GetCurrentSkill(2).CheckCool() > 0)
            {
                SkillThreeButton.GetComponentInChildren<Text>().text += " T- " + GC.GetCurrentSkill(2).CheckCool().ToString();
            }
        }
        catch
        {
            SkillThreeButton.GetComponentInChildren<Text>().text = "null";
        }
        try
        {
            SkillFourButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(3).Name;
            if (GC.GetCurrentSkill(3).CheckCool() > 0)
            {
                SkillFourButton.GetComponentInChildren<Text>().text += " T- " + GC.GetCurrentSkill(3).CheckCool().ToString();
            }
        }
        catch
        {
            SkillFourButton.GetComponentInChildren<Text>().text = "null";
        }
    }
}
