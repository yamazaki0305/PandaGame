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

public class WinPopController : MonoBehaviour {

    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;
    public Text LevelText;
    public Text YourScoreText;
    public Text BestScoreText;
   
    private StageStatus StatusData;

    // Use this for initialization
    void Start () {

        RandomAd.ShowInterstitial();

        StatusData = GameObject.Find("GameRoot").GetComponent<PuzzleMain>().StatusData;

        if (StatusData.star == 0)
        {
            Star1.SetActive(false);
            Star2.SetActive(false);
            Star3.SetActive(false);
        }
        else if (StatusData.star == 1)
        {
            Star1.SetActive(true);
            Star2.SetActive(false);
            Star3.SetActive(false);
        }
        else if (StatusData.star == 2)
        {
            Star1.SetActive(true);
            Star2.SetActive(true);

            Star3.SetActive(false);
        }
        else if (StatusData.star == 3)
        {
            Star1.SetActive(true);
            Star2.SetActive(true);
            Star3.SetActive(true);
        }

        this.LevelText.text = "Lesson "+DataBase.playLevel.ToString();
        this.YourScoreText.text = StatusData.Score.ToString();
        this.BestScoreText.text = DataBase.BestScore.ToString();


    }
	
	// Update is called once per frame
	void Update () {

       
    }
}
