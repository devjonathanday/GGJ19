using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour {

    Light sun;
    GameManager gm;

    Color startColor; // morn night
    Color midColor; // noon
    Vector3 morningRot;
    Vector3 eveningRot;

	// Use this for initialization
	void Start ()
    {
        morningRot = new Vector3(10, 90, 0);
        eveningRot = new Vector3(170, 90, 0);
        startColor = new Color32(255, 150, 0, 100); // orange
        midColor = new Color32(255, 255, 255, 255);// white

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        float t = gm.dayTimer / gm.dayLength;
        sun.transform.rotation = Quaternion.Euler(Vector3.Lerp(morningRot, eveningRot, t));
        if (t < 0.5)
        {
            sun.color = Color.Lerp(startColor, midColor, t);
        }
        else
        {
            sun.color = Color.Lerp(midColor, startColor, t);
        }
	}

    void Reset()
    {
        Start();
    }
}
