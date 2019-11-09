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

public class LevelSceneController : MonoBehaviour {

    private Text TextLevel;
    private Text TextWord;

    public Text WordCountText;
    public Text StarCountText;
    public Text LevelCountText;

    public GameObject ScrollViewContent;

    // Use this for initialization
    void Start () {

        int[] posY = { 4430, 4430, 4430, 4430, 4430, 3974, 3664, 3403, 3142, 2911,
                        2714, 2420, 2175, 1940, 1705, 1590, 1360, 1210, 1077, 656,
                         340, 190, -74, -330, -595, -800, -1105, -1165, -1728, -1981,
                          -2186, -2375, -2811, -3128, -3325, -3710, -3950, -4160, -4430, -4430,-4430};

    //TextLevel = GameObject.Find("TextLevel").GetComponent<Text>();
    //TextWord = GameObject.Find("TextWord").GetComponent<Text>();

    SaveDataBase.loadData();
        //TextLevel.text = "Level:"+DataBase.openLevel;
        //TextWord.text = "Create Word:" + DataBase.RecordWord;

        if (!DataBase.NewWordBook)
            GameObject.Find("NewIcon").SetActive(false);

        Vector3 pos = new Vector3(0, posY[DataBase.openLevel - 1], 0);
        
        ScrollViewContent.transform.localPosition = pos;

        // TopBarの情報を更新
        LevelCountText.text = DataBase.openLevel.ToString();
        WordCountText.text = DataBase.WordDataList.Count.ToString();
        int star = 0;
        for (int i = 0; i < DataBase.level_star.Length; i++)
            star += DataBase.level_star[i];

        StarCountText.text = star.ToString();
}

    // Update is called once per frame
    void Update () {
		
	}
}
