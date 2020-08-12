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


public class StarRewordButton : MonoBehaviour
{
    public int EpNo;
    public GameObject lockButton;
    public GameObject MovieIcon;
    public Text StarCountText;

    public string scene = "<Insert scene name>";
    public float duration = 1.0f;
    public Color color = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        SaveDataBase.loadAllLevel();
        int star = 0;
        int star_max = 0;
        int ep_fst = 0;
        int ep_end = 0;

        switch (EpNo)
        {
            case 1:
                ep_fst = 1;
                ep_end = 3;
                break;
            case 2:
                ep_fst = 4;
                ep_end = 9;
                break;
            case 3:
                ep_fst = 10;
                ep_end = 18;
                break;
            case 4:
                ep_fst = 19;
                ep_end = 27;
                break;
            case 5:
                ep_fst = 28;
                ep_end = 36;
                break;
            case 6:
                ep_fst = 37;
                ep_end = 45;
                break;
            default:
                break;
        }

        for (int i = ep_fst - 1; i < ep_end; i++)
        {
            star += DataBase.level_star[i];
            star_max += 3;
        }

        StarCountText.text = "★ " + star.ToString() + "/" + star_max;

        if (star >= star_max)
        {
            SaveDataBase.loadEp();

            if (DataBase.ep_movie[EpNo] == 0)
            {

            }
            else
            {
                MovieIcon.SetActive(false);
            }

            lockButton.transform.localScale = new Vector2(0f, 0f);
        }

        /*
        if(EpNo==1)
        {

        }
        int star = 0;
        for (int i = 9 * (EpNo - 1); i < 9 * EpNo; i++)
        {
            star += DataBase.level_star[i];
        }

        StarCountText.text = "★ "+star.ToString()+"/27";

        if(star >= 27)
        {
            SaveDataBase.loadEp();

            if( DataBase.ep_movie[EpNo] == 0 )
            {

            }
            else
            {
                MovieIcon.SetActive(false);
            }

            lockButton.transform.localScale = new Vector2(0f, 0f);
        }
        */
    }

    public void CompleteButton()
    {
        // インステ広告を表示
        //RandomAd.ShowInterstitial();

        SaveDataBase.loadEp();

        DataBase.SelectEpNo = this.EpNo;

        Debug.Log("selectep" + this.EpNo);

        if (DataBase.ep_movie[EpNo] == 0)
        {
            DataBase.AdFlg = DataBase.AdRewordFlg.Comic;
            // 動画広告を表示
            GameObject.Find("AdMob").GetComponent<AdReward>().UserOptToWatchAd();
        }
        else
        {
            // 取得したシーンへ移動
            //SceneManager.LoadScene("RewordScenePortrait");
            Transition.LoadLevel(scene, duration, color);
        }

        
    }

    // Update is called once per frame
    void Update()
    {

        if (DataBase.AdRewordOK && DataBase.SelectEpNo == EpNo && DataBase.AdFlg == DataBase.AdRewordFlg.Comic)
        {

            DataBase.AdFlg = DataBase.AdRewordFlg.None;

            DataBase.AdRewordOK = false;

            Debug.Log("EPP" + EpNo);
            DataBase.ep_movie[EpNo] = 1; // エピソード開放
            SaveDataBase.saveEp();

            // 取得したシーンへ移動
            //SceneManager.LoadScene("RewordScenePortrait");
            Transition.LoadLevel(scene, duration, color);
        }
    }
}
