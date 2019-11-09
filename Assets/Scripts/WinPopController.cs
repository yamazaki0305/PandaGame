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
    public Text Mission1Text;
    public Text Mission2Text;
    public Text Mission3Text;
    public Text WordMadeText;
    public Text WordNowText;
    public Text WordRecordText;
   
    private StageStatus StatusData;

    // Use this for initialization
    void Start () {

        RandomAd.ShowInterstitial();

        StatusData = GameObject.Find("GameRoot").GetComponent<PuzzleMain>().StatusData;

        Mission1Text.text = "<color='#FFFF99'>Level Clear</color>";
        if (StatusData.b_letter)
            Mission2Text.text = "<color='#FFFF99'>Letter " + StatusData.StarLetter + "</color>";
        else
            Mission2Text.text = "Letter " + StatusData.StarLetter;

        if (StatusData.b_score)
            Mission3Text.text = "<color='#FFFF99'>Score " + StatusData.StarScore + "</color>";
        else
            Mission3Text.text = "Score " + StatusData.StarScore;

        if (StatusData.star == 1)
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

        this.LevelText.text = "Level "+DataBase.playLevel.ToString();
        this.YourScoreText.text = StatusData.Score.ToString();
        this.BestScoreText.text = DataBase.BestScore.ToString();
        this.WordMadeText.text = DataBase.WordIDAddList.Count().ToString();
        this.WordNowText.text = DataBase.newword_count.ToString();
        this.WordRecordText.text = DataBase.RecordWord.ToString();

    }
	
	// Update is called once per frame
	void Update () {

       
    }
}
