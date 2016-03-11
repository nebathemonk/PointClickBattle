using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {

    GameControl GC;
    Text stepText;
    Text turnText;

    Button SkillOneButton;
    Button SkillTwoButton;
    Button SkillThreeButton;
    Button SkillFourButton;

	// Use this for initialization
	void Start () {

        GC = GameObject.Find("GameControl").GetComponent<GameControl>();
        stepText = GameObject.Find("StepText").GetComponent<Text>();
        turnText = GameObject.Find("TurnText").GetComponent<Text>();

        SkillOneButton = GameObject.Find("SkillOneButton").GetComponent<Button>();
        SkillTwoButton = GameObject.Find("SkillTwoButton").GetComponent<Button>();
        SkillThreeButton = GameObject.Find("SkillThreeButton").GetComponent<Button>();
        SkillFourButton = GameObject.Find("SkillFourButton").GetComponent<Button>();

    }
	
	// Update is called once per frame
	void Update () {

        stepText.text = "Current Step: " + GC.GetStep().ToString();
        turnText.text = "Current Turn: " + GC.GetTurn().ToString();
	
	}

    public void SetSkillButtons()
    {
        //get the skills and change the button functions
        try
        {
            SkillOneButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(0).Name;
        }
        catch
        {
            SkillOneButton.GetComponentInChildren<Text>().text = "null";
        }
        try
        {
            SkillTwoButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(1).Name;
        }
        catch
        {
            SkillTwoButton.GetComponentInChildren<Text>().text = "null";
        }
        try
        {
            SkillThreeButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(2).Name;
        }
        catch
        {
            SkillThreeButton.GetComponentInChildren<Text>().text = "null";
        }
        try
        {
            SkillFourButton.GetComponentInChildren<Text>().text = GC.GetCurrentSkill(3).Name;
        }
        catch
        {
            SkillFourButton.GetComponentInChildren<Text>().text = "null";
        }
    }
}
