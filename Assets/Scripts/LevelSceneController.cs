﻿using System;
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

public class LevelSceneController : MonoBehaviour {

    private Text TextLevel;
    private Text TextWord;

    public Text WordCountText;
    public Text StarCountText;
    public Text LevelCountText;

    public GameObject ScrollViewContent;

    // Use this for initialization
    void Start () {

        int[] posY = { 0,0,0,728,728,728,1228,1228,1228,1868,
                       1868,1868,2436,2436,2436,2864,2864,2864,3550,3550,
                       3550,4035,4035,4035,4463,4463,4463,5138,5138,5138,
                       5641,5641,5641,6060,6060,6060,6721,6721,6721,6721,
                       6721,6721,6721,6721,6721,6721,6721,6721,6721,6721};

    //TextLevel = GameObject.Find("TextLevel").GetComponent<Text>();
    //TextWord = GameObject.Find("TextWord").GetComponent<Text>();

    SaveDataBase.loadData();
        //TextLevel.text = "Level:"+DataBase.openLevel;
        //TextWord.text = "Create Word:" + DataBase.RecordWord;

        if (!DataBase.NewWordBook)
            GameObject.Find("NewIcon").SetActive(false);

        Vector3 pos = new Vector3(0, posY[DataBase.LastPlayLevel - 1], 0);
        
        ScrollViewContent.transform.localPosition = pos;

        // TopBarの情報を更新
        LevelCountText.text = DataBase.openLevel.ToString();
        WordCountText.text = DataBase.WordDataList.Count.ToString();
        int star = 0;
        for (int i = 0; i < DataBase.level_star.Length; i++)
            star += DataBase.level_star[i];

        StarCountText.text = star.ToString();

        Debug.Log("lastplaylevel" + DataBase.LastPlayLevel);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
