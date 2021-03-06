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

// ボタンのタイプを定義
public enum ButtonFlg
{
    NORMAL = 1,
    PRESSED = 2,
    EIGO = 3,
}

// ゲームループを定義
public enum GameLoopFlg
{
    StartInfo, //ステージ紹介中
    PlayNow,    //プレイ中
    Translate, //和訳表示中（ブロックを消す）
    BlockMove,  //ブロック移動中
    UndderArrow, //地面の下にブロックがある時の処理
    PlayBefore, //ゲーム中処理に戻る前の処理
    Hand0,      //手数が0になった時
    PlayEnd,    //プレイ終了
    Pause,      //一時停止中
}

public class StageStatus
{
    public int AnimalSum;
    public int Animal1OK, Animal1;
    public int Animal2OK, Animal2;
    public int Animal3OK, Animal3;
    public int Hand;
    public int Score;
    public int star;
    public bool[] bstar = new bool[3];
    public int StarLetter;
    public bool b_letter = false;
    public int StarScore;
    public bool b_score = false;
    public bool timeflg = false;
    public int time; //カウントダウン前の残り時間タイマー
    public float currentTime; // 残り時間タイマー
    public float gameoverTime; // 手数0になってからゲームオーバーまでのタイマー
    public float hammerTime; // ハンマーのふきだしを表示するまでのタイマー

    public AnimalType AnimalTypeOK1; // スコアのAnimal1のAnimalType
    public AnimalType AnimalTypeOK2; // スコアのAnimal2のAnimalType
    public AnimalType AnimalTypeOK3; // スコアのAnimal3のAnimalType

    public GameObject[] StarObjStar = new GameObject[3];
    private GameObject BarObj; //スコアバーを更新

    //ヘッダーに表示するステータスのclass
    // コンストラクタでインスタンスを生成した時に情報を渡す
    public StageStatus(int animsum, int anim1, int anim2, int anim3, int hand,int letter,int starscore,int tflg,int _time)
    {
        this.AnimalSum = animsum;
        this.Animal1 = anim1;
        this.Animal1OK = 0;
        this.Animal2 = anim2;
        this.Animal2OK = 0;
        this.Animal3 = anim3;
        this.Animal3OK = 0;
        this.StarLetter = letter;
        this.StarScore = starscore;
        if (tflg == 1) this.timeflg = true;
        else this.timeflg = false;

        this.time = _time;
        this.currentTime = _time;

        this.Hand = hand;
        this.Score = 0;
        this.star = 0;

        for(int i=0;i<3;i++)
            bstar[i] = false;

        BarObj = GameObject.Find("Bar");
        StatusUpdate();
    }

    public void AnimalUpdate(AnimalType type)
    {
        if (type == AnimalTypeOK1)
            this.Animal1OK++;
        else if (type == AnimalTypeOK2)
            this.Animal2OK++;
        else if (type == AnimalTypeOK3)
            this.Animal3OK++;

        /*
        if (type == AnimalType.BIRD)
            this.Animal1OK++;
        else if (type == AnimalType.KAERU)
            this.Animal2OK++;
        else if (type == AnimalType.RABBIT)
            this.Animal3OK++;
        */

        this.AnimalSum--;
        StatusUpdate();
    }
    public void HandUpdate()
    {
        this.Hand--;
        StatusUpdate();
    }
    public void ScoreUpdate(int point)
    {
        this.Score += point;
        StatusUpdate();
    }
    public void StatusUpdate()
    {

        /*
        GameObject.Find("TextAnimal1").GetComponent<Text>().text = "/"+this.Animal1.ToString();
        //if(Animal1 == Animal1OK)
        //    GameObject.Find("TextAnimal1OK").GetComponent<Text>().text = "<color='#FFFF99'>"+this.Animal1OK.ToString() + "</color>";
        //else
        GameObject.Find("TextAnimal1OK").GetComponent<Text>().text = this.Animal1OK.ToString();

        GameObject.Find("TextAnimal2").GetComponent<Text>().text = "/" + this.Animal2.ToString();
        //if (Animal2 == Animal2OK)
        //    GameObject.Find("TextAnimal2OK").GetComponent<Text>().text = "<color='#FFFF99'>" + this.Animal2OK.ToString() + "</color>";
        //else
        GameObject.Find("TextAnimal2OK").GetComponent<Text>().text = this.Animal2OK.ToString();
        
        GameObject.Find("TextAnimal3").GetComponent<Text>().text = "/" + this.Animal3.ToString();
        //if (Animal3 == Animal3OK)
        //    GameObject.Find("TextAnimal3OK").GetComponent<Text>().text = "<color='#FFFF99'>" + this.Animal3OK.ToString() + "</color>";
        //else
        GameObject.Find("TextAnimal3OK").GetComponent<Text>().text = this.Animal3OK.ToString();
        */

        // Letterを表示
        if (b_letter)
            GameObject.Find("Letter Amount").GetComponent<Text>().text = "<color='#FFFF99'>" + this.StarLetter.ToString() + "</color>";
        else
            GameObject.Find("Letter Amount").GetComponent<Text>().text = this.StarLetter.ToString();

        // ScoreStarを表示
        GameObject.Find("Score Star").GetComponent<Text>().text = "/" + this.StarScore.ToString();

        GameObject StatusCat = GameObject.Find("CatText");
        StatusCat.GetComponent<Text>().text =  this.AnimalSum + "匹";
        GameObject StatusHand = GameObject.Find("Text Moves Amount");
        StatusHand.GetComponent<Text>().text = this.Hand.ToString();
        GameObject StatusScore = GameObject.Find("Score Amount");
        if (b_score)
            StatusScore.GetComponent<Text>().text = "<color='#FFFF99'>" + this.Score.ToString() + "</color>";
        else
            StatusScore.GetComponent<Text>().text = this.Score.ToString();

        StarObjStar[0] = GameObject.Find("Star 1");
        //StarObjStar[0].SetActive(false);
        StarObjStar[1] = GameObject.Find("Star 2");
        //StarObjStar[1].SetActive(false);
        StarObjStar[2] = GameObject.Find("Star 3");
        //StarObjStar[2].SetActive(false);

        float fill = (float)this.Score / this.StarScore;
        BarObj.GetComponent<Image>().fillAmount = fill;

        hammerTime = 30f;
    }

    // 動画広告を見た時の手数の更新
    public void AdRewordUpdate()
    {
        Debug.Log("tttttttttttttttt");

        if(this.Hand < 5)
            this.Hand = 5;

        this.currentTime = this.time;

        GameObject StatusHand = GameObject.Find("Text Moves Amount");
        StatusHand.GetComponent<Text>().text = this.Hand.ToString();

        //this.Score = 0 + (this.Animal1OK + this.Animal2OK + this.Animal3OK) * 300;
        this.Score = this.Score -1000;
        if (this.Score < 0)
            this.Score = 0;

        GameObject StatusScore = GameObject.Find("Score Amount");
        StatusScore.GetComponent<Text>().text = this.Score.ToString();

    }

    public bool StarCheck(string str)
    {
        bool bAdd = false;

        // スコアバーを小数点で記録
        float fill = (float)this.Score / this.StarScore;

        // スコアバーが40%を達成したら☆をつける
        if(!bstar[0])
        {
            if (fill >= 0.4f)
            {
                bstar[0] = true;
                StarObjStar[0].GetComponent<Animator>().SetTrigger("StarTrigger");
                bAdd = true;
                star++;
            }
        }
        // スコアバーが70%を達成したら☆をつける
        if (!bstar[1])
        {
            if (fill >= 0.7f)
            {
                bstar[1] = true;
                StarObjStar[1].GetComponent<Animator>().SetTrigger("StarTrigger");
                bAdd = true;
                star++;
            }
        }
        // スコアバーが100%を達成したら☆をつける
        if (!bstar[2])
        {
            if (fill >= 1.0f)
            {
                bstar[2] = true;
                StarObjStar[2].GetComponent<Animator>().SetTrigger("StarTrigger");
                bAdd = true;
                star++;
            }
        }


        return bAdd;

    }

    public void StarClearAdd()
    {

        for (int i = 0; i < star; i++)
        {
            if (!bstar[i])
            {
                bstar[i] = true;
                StarObjStar[i].GetComponent<Animator>().SetTrigger("StarTrigger");

            }
        }
    }
}

public static class RandomMake
{
    public static string alphabet()
    {
        char[] eigochar = "AAAAAAABBCCCDDDEEEEEEEFFGGGHHHIIIIIIJKKKLLLMMMNNNOOOOPPQRRRSSSTTTUUUUUVWWXYYYZ".ToCharArray();
        int rand = UnityEngine.Random.Range(0, eigochar.Length);
        return eigochar[rand].ToString();
        
    }

}

public static class RandomAd
{
    public static void ShowInterstitial()
    {
        // インステ広告はステージ4以上から
        if (DataBase.playLevel >= 4)
        {
            //AdRateで確率で広告表示
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand <= DataBase.AdRate)
            {
                GameObject.Find("AdMob").GetComponent<AdInterstitial>().ShowInterstitial();
            }
        }

    }

}
