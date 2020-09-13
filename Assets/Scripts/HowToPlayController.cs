using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Globalization;
using Ricimi;

public class HowToPlayController : MonoBehaviour {

    int page_no;
    bool JapLanguage = true;

    public GameObject JapObject;
    public GameObject EngObject;
    public Text pagenotext;

    // Use this for initialization
    void Start () {
        page_no = 1;
        pagenotext.text = page_no.ToString() + "/8";

        DataBase.bGamePause = true;

        EngObject.SetActive(false);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Update is called once per frame
    public void NextButton()
    {
        page_no++;
        pagenotext.text = page_no.ToString() + "/8";

        if (page_no == 2)
        {
            GameObject.Find("PlayJap1").SetActive(false);
        }
        else if (page_no == 3)
        {
            GameObject.Find("PlayJap2").SetActive(false);
        }
        else if (page_no == 4)
        {
            GameObject.Find("PlayJap3").SetActive(false);
        }
        else if (page_no == 5)
        {
            GameObject.Find("PlayJap4").SetActive(false);
        }
        else if (page_no == 6)
        {
            GameObject.Find("PlayJap5").SetActive(false);
        }
        else if (page_no == 7)
        {
            GameObject.Find("PlayJap6").SetActive(false);
        }
        else if (page_no == 8)
        {
            GameObject.Find("PlayJap7").SetActive(false);
            GameObject.Find("Button Next").SetActive(false);
        }




    }
}
