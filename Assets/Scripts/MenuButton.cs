using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    //public Image i;
    public GameManager GM;

	// Use this for initialization
	void Start () {
        //i.enabled = false;
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
	
	public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //i.enabled = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //i.enabled = false;
    }

    public void StartGame()
    {
        GM.day = 1;
        GM.dayTimer = 0;
        GM.AssignFoodRequirements();
        SceneManager.LoadScene("MainScene");
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
