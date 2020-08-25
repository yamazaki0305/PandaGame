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

        change_time = 5f;

        now_hour = DateTime.Now.Hour;
        Debug.Log("hour:" + now_hour);

        //now_hour = UnityEngine.Random.Range(0, 24);

        if ( 0 <= now_hour && now_hour < 5 )
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            panda_no[0] = 2;
            panda_no[1] = 44;
            panda_no[2] = 10;
            panda_no[3] = 12;
            panda_no[4] = 2;
            panda_no[5] = 44;
            panda_no[6] = 10;
            panda_no[7] = 12;
            panda_text[0] = "ZZZ";
            panda_text[1] = "(Dreaming of a dog)";
            panda_text[2] = "The reason the title screen is beach is to sleep";
            panda_text[3] = "while listening to the sound of the waves.";
            panda_text[4] = "グーグー";
            panda_text[5] = "(犬の夢を見ているよ)";
            panda_text[6] = "タイトル画面がビーチなのは、";
            panda_text[7] = "波の音を聞きながら寝るためっス!";

            //GameObject.Find("AdMob").GetComponent<AdBanner>().RequestBanner();
        }
        else if (5 <= now_hour && now_hour < 7)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            panda_no[0] = 17;
            panda_no[1] = 6;
            panda_no[2] = 35;
            panda_no[3] = 44;
            panda_no[4] = 17;
            panda_no[5] = 6;
            panda_no[6] = 35;
            panda_no[7] = 44;
            panda_text[0] = "Good morning,dog cute!";
            panda_text[1] = "I'm looking at my sleeping dog.";
            panda_text[2] = "It's bad to wake the dog up,";
            panda_text[3] = "so let's sleep a little more.";
            panda_text[4] = "おはよう、犬かわいい!";
            panda_text[5] = "今、寝ている犬を見ているよ。";
            panda_text[6] = "犬を起こすのも悪いから、";
            panda_text[7] = "もう少し寝よう。";
        }
        else if (7 <= now_hour && now_hour < 10)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            panda_no[0] = 17;
            panda_no[1] = 4;
            panda_no[2] = 1;
            panda_no[3] = 3;
            panda_no[4] = 17;
            panda_no[5] = 4;
            panda_no[6] = 1;
            panda_no[7] = 3;
            panda_text[0] = "Good morning,dog cute!";
            panda_text[1] = "I'm preparing the dog's food right now.";
            panda_text[2] = "My dog is eating.";
            panda_text[3] = "My dog is in the poo.";
            panda_text[4] = "おはよう、犬かわいい!";
            panda_text[5] = "今、犬のごはんの支度をしているよ。";
            panda_text[6] = "食事中だよ。";
            panda_text[7] = "トイレ中だよ。";
        }
        else if (10 <= now_hour && now_hour < 12)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            panda_no[0] = 7;
            panda_no[1] = 8;
            panda_no[2] = 15;
            panda_no[3] = 33;
            panda_no[4] = 7;
            panda_no[5] = 8;
            panda_no[6] = 15;
            panda_no[7] = 33;
            panda_text[0] = "I'm taking a walk with my dog right now.";
            panda_text[1] = "Cute, cute dog.";
            panda_text[2] = "It’s been a long time.";
            panda_text[3] = "How are you? Do you see my dog?";
            panda_text[4] = "今、犬と散歩しているよ。";
            panda_text[5] = "かわいい、犬かわいい!";
            panda_text[6] = "お久しぶり。";
            panda_text[7] = "元気？犬、見る？";
        }
        else if (12 <= now_hour && now_hour < 14)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            panda_no[0] = 34;
            panda_no[1] = 41;
            panda_no[2] = 43;
            panda_no[3] = 4;
            panda_no[4] = 34;
            panda_no[5] = 41;
            panda_no[6] = 43;
            panda_no[7] = 4;
            panda_text[0] = "Hello,dog cute!";
            panda_text[1] = "GRR";
            panda_text[2] = "Haha!I'm hungry.";
            panda_text[3] = "I'll prepare lunch.";
            panda_text[4] = "こんにちは、犬かわいい!";
            panda_text[5] = "グー";
            panda_text[6] = "あはは、お腹空いてきた。";
            panda_text[7] = "お昼ごはんの準備をするよ。";
        }
        else if (14 <= now_hour && now_hour < 16)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_m");

            panda_no[0] = 35;
            panda_no[1] = 2;
            panda_no[2] = 44;
            panda_no[3] = 40;
            panda_no[4] = 35;
            panda_no[5] = 2;
            panda_no[6] = 44;
            panda_no[7] = 40;
            panda_text[0] = "Hello,dog cute!";
            panda_text[1] = "ZZZ";
            panda_text[2] = "ZZZZZZ";
            panda_text[3] = "It's time to take a nap.";
            panda_text[4] = "こんにちは、犬かわいい!";
            panda_text[5] = "グーグー";
            panda_text[6] = "グーグーグーグー";
            panda_text[7] = "お昼寝の時間だよ。";
        }
        else if (16 <= now_hour && now_hour < 19)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_sunset");

            panda_no[0] = 14;
            panda_no[1] = 40;
            panda_no[2] = 31;
            panda_no[3] = 23;
            panda_no[4] = 14;
            panda_no[5] = 40;
            panda_no[6] = 31;
            panda_no[7] = 23;
            panda_text[0] = "I'm playing with a dog right now.";
            panda_text[1] = "Shashashasha!";
            panda_text[2] = "Ohhhhh!";
            panda_text[3] = "I can't move because I have a dog right now.";
            panda_text[4] = "今、犬と遊んでいるよ。";
            panda_text[5] = "シャシャシャシャ!";
            panda_text[6] = "おおおおおー！";
            panda_text[7] = "今、犬を乗せているから動けないよ。";

        }
        else if (19 <= now_hour && now_hour < 21)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            panda_no[0] = 41;
            panda_no[1] = 1;
            panda_no[2] = 34;
            panda_no[3] = 44;
            panda_no[4] = 41;
            panda_no[5] = 1;
            panda_no[6] = 34;
            panda_no[7] = 44;
            panda_text[0] = "GRR";
            panda_text[1] = "It's dinner time.";
            panda_text[2] = "After eating dinner,";
            panda_text[3] = "I became sleepy.";
            panda_text[4] = "グー";
            panda_text[5] = "夕食の時間だよ。";
            panda_text[6] = "夕食を食べたら、";
            panda_text[7] = "眠くなってきたよ。";
        }
        else if (21 <= now_hour && now_hour <= 24)
        {
            bg_img = GameObject.Find("Beach").GetComponent<Image>();
            bg_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            wave_img = GameObject.Find("Wave_anime").GetComponent<Image>();
            wave_img.sprite = Resources.Load<Sprite>("StageIMG/beach_night");

            panda_no[0] = 20;
            panda_no[1] = 5;
            panda_no[2] = 8;
            panda_no[3] = 42;
            panda_no[4] = 20;
            panda_no[5] = 5;
            panda_no[6] = 8;
            panda_no[7] = 42;
            panda_text[0] = "(It's bath time)"; 
            panda_text[1] = "I'm taking a bath in my dog right now.";
            panda_text[2] = "When I get out of the bath,I wipe my dog's body.";
            panda_text[3] = "It's time to go to bed now.Good night.";
            panda_text[4] = "(お風呂タイム)";
            panda_text[5] = "今、犬をお風呂に入れているよ。";
            panda_text[6] = "お風呂から出たら、犬の体をふくよ。";
            panda_text[7] = "そろそろ寝る時間だよ。おやすみなさい。";
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
