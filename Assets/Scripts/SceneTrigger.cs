using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public SceneController sceneController;
    public GameManager GM;

    private void Start()
    {
        sceneController = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>();
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GM.berries > 0 || GM.fish > 0 || GM.misc > 0)
            if(other.gameObject.tag == "Player")
            {
                SceneManager.LoadScene("NightScene");
            }
    }
}