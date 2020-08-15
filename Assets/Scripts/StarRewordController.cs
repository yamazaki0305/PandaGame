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

public class StarRewordController : MonoBehaviour
{
    // CSVデータ
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    [SerializeField] Text Title;
    [SerializeField] Image Img1, Img2, Img3, Img4;
    [SerializeField] Text Eng1, Eng2, Eng3, Eng4;
    [SerializeField] Text Jap1, Jap2, Jap3, Jap4;
    [SerializeField] GameObject toggle;
    [SerializeField] GameObject Content;
    bool checkbox = false;

    // Start is called before the first frame update
    void Start()
    {
        // CSVReader
        csvDatas = CSVReader.import("CSV/EpisodeText");

        Title.text = "Episode " + DataBase.SelectEpNo + "\r\n～" +csvDatas[DataBase.SelectEpNo][1]+"～";
        Img1.sprite = Resources.Load<Sprite>("LessonIMG/"+csvDatas[DataBase.SelectEpNo][2]);
        Img2.sprite = Resources.Load<Sprite>("LessonIMG/" + csvDatas[DataBase.SelectEpNo][3]);
        Img3.sprite = Resources.Load<Sprite>("LessonIMG/" + csvDatas[DataBase.SelectEpNo][4]);
        Img4.sprite = Resources.Load<Sprite>("LessonIMG/" + csvDatas[DataBase.SelectEpNo][5]);
        Eng1.text = csvDatas[DataBase.SelectEpNo][6].Replace("+", ",");
        Eng2.text = csvDatas[DataBase.SelectEpNo][7].Replace("+", ",");
        Eng3.text = csvDatas[DataBase.SelectEpNo][8].Replace("+", ",");
        Eng4.text = csvDatas[DataBase.SelectEpNo][9].Replace("+", ",");
        Jap1.text = "";
        Jap2.text = "";
        Jap3.text = "";
        Jap4.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(toggle.GetComponent<Toggle>().isOn)
        {
            if(checkbox==false)
            {
                checkbox = true;
                Content.transform.localPosition = new Vector2(0, 0);

                Jap1.text = csvDatas[DataBase.SelectEpNo][10];
                Jap2.text = csvDatas[DataBase.SelectEpNo][11];
                Jap3.text = csvDatas[DataBase.SelectEpNo][12];
                Jap4.text = csvDatas[DataBase.SelectEpNo][13];

            }

        }
        else
        {
            if(checkbox==true)
            {
                checkbox = false;
                Content.transform.localPosition = new Vector2(0, 0);

                Jap1.text = "";
                Jap2.text = "";
                Jap3.text = "";
                Jap4.text = "";
            }

        }
        
    }
}
