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
    public Text FukidashiText;
    public Text KuronekoEngText;
    public Text KuronekoJapText;

    private StageStatus StatusData;

    // CSVデータ
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    float currentTime = 1f;

    // Use this for initialization
    void Start () {


        //RandomAd.ShowInterstitial();

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

        this.LevelText.text = "レッスン "+DataBase.playLevel.ToString();
        this.YourScoreText.text = StatusData.Score.ToString();
        this.BestScoreText.text = DataBase.BestScore.ToString();

        LoadKuronekoText();

    }

    // Update is called once per frame
    void Update() {

        /*
        // 残り時間を計算する
        currentTime -= Time.deltaTime;

        // ゼロ秒以下にならないようにする
        if (currentTime <= 0.0f)
        {
            Debug.Log("aeseeeeeeeeeeeeeeee");

        }
        */
    }

    void LoadKuronekoText()
    {
        // CSVReader
        csvDatas = CSVReader.import("CSV/KuronekoText");
        int rand = UnityEngine.Random.Range(1, csvDatas.Count);
        Debug.Log("rand:" + rand);
        //rand = 3;
        //FukidashiText.text = csvDatas[DataBase.playLevel][0]+"の名言を教えるっス!!";
        KuronekoEngText.text = csvDatas[DataBase.playLevel][1].Replace("+", ",");
        KuronekoJapText.text = csvDatas[DataBase.playLevel][2]+"\r\n"+ "by"+csvDatas[DataBase.playLevel][0];

    }
}
