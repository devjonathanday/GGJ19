using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public float transitionTimer; 
    float elapsedTime;
    bool isNight;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SceneManager");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        transitionTimer = 4f;
        elapsedTime = 0f;
        isNight = false;
    }

    private void Update()
    {
        if (isNight)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= transitionTimer)
            {
                elapsedTime = 0;
                isNight = false;
                // SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("NightScene"));
                LoadMainScene(); // test me
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainScene()
    {
        LoadScene("MainScene");
    }

    public void LoadTransitionScene()
    {
        isNight = true;
        LoadScene("NightScene");
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("NightScene"));
    }
}