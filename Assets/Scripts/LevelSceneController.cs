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

        int[] posY = { 0,0,0,
                       728,728,728,
                       1430,1430,1430,1430,1430,1430,
                       2590,2590,2590,2590,2590,2590,
                       3793,3793,3793,3793,3793,3793,3793,3793,3793,
                       5385,5385,5385,5385,5385,5385,5385,5385,5385,
                       6996,6996,6996,6996,6996,6996,6996,6996,6996,
                       8580,8580,8580,8580,8580,8580,8580,8580,8580,
                       10153,10153,10153,10153,10153,10153,10153,10153,10153};

    //TextLevel = GameObject.Find("TextLevel").GetComponent<Text>();
    //TextWord = GameObject.Find("TextWord").GetComponent<Text>();

        SaveDataBase.loadData();
        //TextLevel.text = "Level:"+DataBase.openLevel;
        //TextWord.text = "Create Word:" + DataBase.RecordWord;

        if (DataBase.openLevel > 1)
        {
            GameObject.Find("tutorial").SetActive(false);
            GameObject.Find("tutorial2").SetActive(false);
        }


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
