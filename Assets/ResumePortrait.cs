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

public class ResumePortrait : MonoBehaviour
{
    public Text EngText;
    public Text JapText;
    public Image dogimg;

    // CSVデータ
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    // Start is called before the first frame update
    void Start()
    {
        int rand = UnityEngine.Random.Range(1, DataBase.MAXSTAGE+1);
        dogimg.sprite = Resources.Load<Sprite>("LessonIMG/" + rand);

        // CSVReader
        csvDatas = CSVReader.import("CSV/KuronekoText");
        rand = UnityEngine.Random.Range(1, csvDatas.Count);
        Debug.Log("rand:" + rand);
        //rand = 3;
        //FukidashiText.text = csvDatas[DataBase.playLevel][0]+"の名言を教えるっス!!";
        EngText.text = csvDatas[rand][1].Replace("+", ",");
        JapText.text = csvDatas[rand][2] + "\r\n" + "by " + csvDatas[rand][0];
    }

}
