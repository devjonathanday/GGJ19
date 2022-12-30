using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SceneController sc;

    public enum DIET_TYPE { Carnivore, Herbivore, Omnivore };

    [System.Serializable]
    public struct DinoChild
    {
        public DIET_TYPE diet;
    }

    [Header("Gameplay Variables")]
    public DinoChild[] dinoChildren;
    private DinoChild cBaby;
    private DinoChild oBaby;
    public int berries;
    public int fish;
    public int misc;
    public int fishNeeded = 0;
    public int berriesNeeded = 0;
    public int miscNeeded = 0;
    public bool instructionsRead = false;

    [Header("Timers")]
    public float dayTimer;
    public float dayLength; //Length of a day in seconds
    public int day;
    public int maxDays;
    public bool counting;
    public Text recountText;

    public bool isEnough; // player has enough food for the day

    [Header("Sounds")]
    public AudioSource audioChannel;
    public AudioSource ambientMusic;

    void Awake()
    {
        //Check for and delete clones of GameManagers
        tag = "GameController";
        GameObject[] clones = GameObject.FindGameObjectsWithTag("GameController");
        if (clones.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        sc = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>();
        if (!ambientMusic.isPlaying)
            ambientMusic.Play();

        isEnough = true;
        maxDays = 7;

        cBaby = new DinoChild();
        cBaby.diet = DIET_TYPE.Carnivore;
        oBaby = new DinoChild();
        oBaby.diet = DIET_TYPE.Omnivore;
    }

    void Update()
    {
        if (counting) //Only enabled when playing the game
            dayTimer += Time.deltaTime;
        if (dayTimer > dayLength && SceneManager.GetActiveScene().name != "NightScene")
        {
            berries = 0;
            fish = 0;
            misc = 0;
            SceneManager.LoadScene("NightScene");
            isEnough = EnoughFood();
        }
        //if(day > maxDays && EnoughFood())
        //{
        //    SceneManager.LoadScene("EndScene");
        //}
        counting = instructionsRead;
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            counting = false;
            ambientMusic.volume = 0;
        }
        else ambientMusic.volume = 1;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // reset values, progress
    public void NextDay()
    {
        dayTimer = 0;
        day++;

        fish = 0;
        berries = 0;
        misc = 0;
    }

    // restart
    public void RestartDay()
    {
        NextDay();
        day--;
        if(day < 1)
        {
            day = 1;
        }
    }

    // see if enough food to return
    public bool EnoughFood()
    {
        return (berries == berriesNeeded && fish == fishNeeded && misc == miscNeeded);
    }

    // go to next scene
    public void GoToNextDay()
    {
        if (EnoughFood()) day++;
        if (day > maxDays) SceneManager.LoadScene("EndScene");
        else
        {
            dayTimer = 0;
            AssignFoodRequirements();
            SceneManager.LoadScene("MainScene");
        }
    }

    public void AssignFoodRequirements()
    {
        berries = 0;
        fish = 0;
        misc = 0;
        switch (day)
        {
            case 1:
                fishNeeded = 1;
                berriesNeeded = 0;
                miscNeeded = 0;
                break;
            case 2:
                fishNeeded = 2;
                berriesNeeded = 0;
                miscNeeded = 0;
                break;
            case 3:
                fishNeeded = 2;
                berriesNeeded = 1;
                miscNeeded = 0;
                break;
            case 4:
                fishNeeded = 2;
                berriesNeeded = 2;
                miscNeeded = 0;
                break;
            case 5:
                fishNeeded = 3;
                berriesNeeded = 2;
                miscNeeded = 1;
                break;
            case 6:
                fishNeeded = 3;
                berriesNeeded = 3;
                miscNeeded = 2;
                break;
            case 7:
                fishNeeded = 3;
                berriesNeeded = 3;
                miscNeeded = 3;
                break;
        }
    }
}
