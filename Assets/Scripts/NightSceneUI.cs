using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NightSceneUI : MonoBehaviour {

    float zTimer;
    float elapsedTime;
    int zCount;

    public GameManager gm;
    public Text txt;
    public Text recountText;
    public Text dailyCollectionText;
    public Text berryText;
    public Text fishText;
    public Text miscText;
    public Text flavorText;
    public int day;
    public float sceneTimer;
    public float sceneElapsed;
    bool spaceOnEnter;
    bool spaceReleased;
    bool canExit;

	// Use this for initialization
	void Start () {
        zTimer = 1f;
        elapsedTime = 0f;
        txt.text = "z ";
        zCount = 4;
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        day = gm.day;

        dailyCollectionText.text = "Day " + gm.day + " Collection:";
        berryText.text = gm.berries + " / " + gm.berriesNeeded;
        fishText.text = gm.fish + " / " + gm.fishNeeded;
        miscText.text = gm.misc + " / " + gm.miscNeeded;

        sceneTimer = 2f;
        sceneElapsed = 0f;
        spaceOnEnter = Input.GetKeyDown(KeyCode.Space);
        spaceReleased = false;
        canExit = false;

        AssignFlavorText();
    }

    void AssignFlavorText()
    {
        if (!gm.EnoughFood())
        {
            flavorText.text = "You failed to collect enough food for your kids,\n"
                + "so you have to go out and forage again.";
            return;
                
        }
        // biggie boy switch statement for story
        switch (day)
        {
            case 1:
                flavorText.text = "Congratulations, you made it through your first day!\n"
                    + "Your goal is to provide a home for your lovely dinosaur child.\n"
                    + "This Carnivore needs to eat fish to survive";
                break;
            case 2:
                flavorText.text = "While you were gone today hunting for your child,\n"
                    + " another egg has fallen into your nest and hatched.\n"
                    + "This Herbivore needs to eat berries to survive.";
                break;
            case 3:
                flavorText.text = "These dinosaur hatchlings are so grateful for your support.";
                break;
            case 4:
                flavorText.text = "While you were gone today hunting for your two children,\n"
                    + " another egg has fallen into your nest and hatched.\n"
                    + "This Omnivore can eat either fish or berries.";
                break;
            case 5:
                flavorText.text = "Your babies are depending on you for support,\n"
                    + "so make sure to watch out for bigger dinosaurs\n"
                    + "who will try to steal the food you have collected for your children.";
                break;
            case 6:
                flavorText.text = "The day has almost come where your children can\n"
                    + "leave home and go out into the world!\n"
                    + "Keep providing for them for one more day and they will be\n"
                    + "big and strong enough to go out into the world!";
                break;
            case 7:
                flavorText.text = "You have successfully raised your babies and they are\n" 
                    + "ready to go off into the world.\n"
                    + "You realize that even though they're going away,\n"
                    + "your home will always be with them.";
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            spaceReleased = true;
        }

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= zTimer)
        {
            elapsedTime = 0f;
            txt.text += " ... z";
            zCount++;
            if(zCount >= 4)
            {
                zCount = 0;
                txt.text = "z";
            }
        }

        sceneElapsed += Time.deltaTime;
        if(sceneElapsed >= sceneTimer)
        {
            if (spaceOnEnter)
            {
                if (spaceReleased)
                {
                    canExit = true;
                }
            }
            else
            {
                canExit = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canExit)
        {
            gm.GoToNextDay();
            //if (gm.day < gm.maxDays) gm.GoToNextDay();
            //else SceneManager.LoadScene("EndScene");
        }
	}
}
