using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameManager GM;
    //Assigned in inspector window
    public Text foodNeededText;
    public Text dayCounter;
    public Text dinoList;
    public bool displayGameUI;
    public Image sunTimer;
    public Sprite[] sunTimerFrames;
    public GameObject instructions;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainScene" &&
           !GM.instructionsRead)
        {
            instructions.SetActive(true);
            if (Input.GetKey(KeyCode.Space))
                GM.instructionsRead = true;
        }
        else if (Input.GetKey(KeyCode.E)) instructions.SetActive(true);
        else instructions.SetActive(false);
        if (displayGameUI)
        {
            foodNeededText.enabled = true;
            //GM.GetFoodRequirements();
            foodNeededText.text = "";
            foodNeededText.text += GM.fish + "/" + GM.fishNeeded + "\n";
            foodNeededText.text += GM.berries + "/" + GM.berriesNeeded + "\n";
            foodNeededText.text += GM.misc + "/" + GM.miscNeeded;
            sunTimer.enabled = true;
            sunTimer.sprite = sunTimerFrames[(int)((GM.dayTimer / GM.dayLength) * sunTimerFrames.Length)];
            sunTimer.color = Color.Lerp(Color.white, Color.red, GM.dayTimer / GM.dayLength);
            dayCounter.text = "Day " + GM.day.ToString();
        }

    }
}