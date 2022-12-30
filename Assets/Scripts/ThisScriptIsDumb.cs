using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThisScriptIsDumb : MonoBehaviour {

    public NightSceneUI ns;
    public Image thisIm;
    public Image[] backgrounds;
    int day;

    private void Start()
    {
        ns = GameObject.FindGameObjectWithTag("NSCanvas").GetComponent<NightSceneUI>();
        thisIm = gameObject.GetComponent<Image>();
        day = ns.day;
        GetImage();
    }

    void GetImage()
    {
        if (ns.gm.EnoughFood())
        {
            if (day < 2)
            {
                thisIm.sprite = backgrounds[0].sprite;
            }
            else if (day < 4)
            {
                thisIm.sprite = backgrounds[2].sprite;
            }
            else
            {
                print("4!");
                thisIm.sprite = backgrounds[4].sprite;
            }
        }
        else
        {
            if (day < 3)
            {
                thisIm.sprite = backgrounds[1].sprite;
            }
            else if (day < 5)
            {
                thisIm.sprite = backgrounds[3].sprite;
            }
            else
            {
                thisIm.sprite = backgrounds[5].sprite;
            }
        }
    }
}
