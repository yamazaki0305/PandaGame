using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HomeSceneController : MonoBehaviour
{
    int change_no = 0;
    public Text fukidashi_text;
    int[] panda_no = { 19, 2, 5, 1, 30, 29, 12, 7 };
    string [] panda_text = {"Dog cute！",
                            "いぬかわいいYo！",
                            "Learn English with puzzle games！",
                            "パズルゲームで\nえいごがまねべるYo！",
                            "It's a game that makes you sleepy,",
                            "Recommended for sleepless nights！",
                            "だんだんねむくなる\nゲームだから",
                            "ねむれないよるに\nオススメだYo！",
    };


    Image dogimg;
    Image bg_img, wave_img;
    float change_time;
    private int now_hour;

    // Start is called before the first frame update
    void Start()
    {

        change_time = 3.5f;

        now_hour = DateTime.Now.Hour;
        Debug.Log("hour:" + now_hour);

        now_hour = 0;

        if ( 0 <= now_hour && now_hour < 5 )
        {
            bg_img = GameObject.Find("Background").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            panda_no[0] = 1;
            panda_no[1] = 1;
            panda_no[2] = 1;
            panda_no[3] = 1;
            panda_no[4] = 1;
            panda_no[5] = 1;
            panda_no[6] = 1;
            panda_no[7] = 1;
            panda_text[0] = "aaaaaa";
            panda_text[1] = "aaaaaa";
            panda_text[2] = "aaaaaa";
            panda_text[3] = "aaaaaa";
            panda_text[4] = "aaaaaa";
            panda_text[5] = "aaaaaa";
            panda_text[6] = "aaaaaa";
            panda_text[7] = "aaaaaa";
        }
        else if (5 <= now_hour && now_hour < 7)
        {
            bg_img = GameObject.Find("Background").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");
        }
        else if (7 <= now_hour && now_hour < 16)
        {
            bg_img = GameObject.Find("Background").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");
        }
        else if (16 <= now_hour && now_hour < 19)
        {
            bg_img = GameObject.Find("Background").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            panda_no[0] = 1;
            panda_no[1] = 1;
            panda_no[2] = 1;
            panda_no[3] = 1;
            panda_no[4] = 1;
            panda_no[5] = 1;
            panda_no[6] = 1;
            panda_no[7] = 1;
            panda_text[0] = "aaaaaa";
            panda_text[1] = "aaaaaa";
            panda_text[2] = "aaaaaa";
            panda_text[3] = "aaaaaa";
            panda_text[4] = "aaaaaa";
            panda_text[5] = "aaaaaa";
            panda_text[6] = "aaaaaa";
            panda_text[7] = "aaaaaa";



        }
        else if (19 <= now_hour && now_hour <= 24)
        {
            bg_img = GameObject.Find("Background").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");
        }

        dogimg = GameObject.Find("PandaDogImg").GetComponent<Image>();
        dogimg.sprite = Resources.Load<Sprite>("LessonIMG/" + panda_no[0]);
        fukidashi_text.text = panda_text[change_no];

    }

    // Update is called once per frame
    void Update()
    {
        change_time -= Time.deltaTime;

        if(change_time < 0)
        {
            change_time = 3.5f;

            change_no++;
            if (change_no >= 8)
                change_no = 0;

            fukidashi_text.text = panda_text[change_no];
            dogimg.sprite = Resources.Load<Sprite>("LessonIMG/" + panda_no[change_no]);
        }
    }
}
