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

public class StartGamePopupController : MonoBehaviour {

    public Text HeadlineText;
    public Text BestScoreText;
    public Text Mission1Text;
    public Text Mission2Text;
    public Text Mission3Text;

    public GameObject Star1Obj;
    public GameObject Star2Obj;
    public GameObject Star3Obj;

    // CSVデータ
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    // Use this for initialization
    void Start () {

        HeadlineText.text = "Play level " + DataBase.playLevel;
        SaveDataBase.loadLevel(DataBase.playLevel);
        BestScoreText.text = "Best Score: " + DataBase.BestScore.ToString();

        // CSVReader
        csvDatas = CSVReader.import("CSV/StageListData");

        Mission1Text.text = "Level Clear";
        Mission2Text.text = "Letter " + csvDatas[DataBase.playLevel][(int)CSVColumn.m_letter];
        Mission3Text.text = "Score " + csvDatas[DataBase.playLevel][(int)CSVColumn.m_score];

        switch (DataBase.level_star[DataBase.playLevel - 1])
        { 
            case 0:
                Star1Obj.SetActive(false);
                Star2Obj.SetActive(false);
                Star3Obj.SetActive(false);
                break;
            case 1:
                Star1Obj.SetActive(true);
                Star2Obj.SetActive(false);
                Star3Obj.SetActive(false);
                break;
            case 2:
                Star1Obj.SetActive(true);
                Star2Obj.SetActive(true);
                Star3Obj.SetActive(false);
                break;
            case 3:
                Star1Obj.SetActive(true);
                Star2Obj.SetActive(true);
                Star3Obj.SetActive(true);
                break;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
