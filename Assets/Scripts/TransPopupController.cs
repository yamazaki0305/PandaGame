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

public class TransPopupController : MonoBehaviour {

    public GameObject NewIcon;
    public Text EngText;
    public Text JapText;

    // Use this for initialization
    void Start () {

        //DBの定義
        SqliteDatabase sqlDB;
        //DBの設定
        sqlDB = new SqliteDatabase("ejdict.sqlite3");

        string query = "select item_id,word,mean from items where item_id ='" + DataBase.Word_item_id + "'";
        DataTable dataTable = sqlDB.ExecuteQuery(query);

        foreach (DataRow dr in dataTable.Rows)
        {

            string id = dr["item_id"].ToString();
            EngText.text = (string)dr["word"];
            JapText.text = (string)dr["mean"];

            // 半角スペースで改行してしまう！？のを解決しよう（Unity）
            // https://qiita.com/sharp_6/items/4001cd0b534226e0c287
            string str = JapText.text.Replace(" ", "\u00A0").Replace("/", "/\r\n").Replace("〈C〉", "").Replace("〈U〉", "").Replace("〈", "<").Replace("〉", ">");

            //string str = JapText.text;

            //先頭から16行✕3列分の文字列を取得
            if (str.Length < 120)
                str = "\r\n" + str;

            JapText.text = str;
        }
        if (DataBase.Word_new_flg)
            NewIcon.SetActive(true);
        else
            NewIcon.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
