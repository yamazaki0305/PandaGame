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


public class CanWordController : MonoBehaviour
{

    private string beforeEigoText;
    public Text nowEigoTextObj;

    private int[] can_alphabet = new int[26]; //A-Zまでパズルエリアに何文字あるか格納する
                                              //DBの定義
    SqliteDatabase sqlDB;

    //無視英単語リスト
    string[] ignore_word;

    // Use this for initializati
    void Start()
    {

        // 言語判定をする
        if (DataBase.LangJapanese)
            GameObject.Find("CanWordText").GetComponent<Text>().text = "英単語アシスト機能ON";
        else
            GameObject.Find("CanWordText").GetComponent<Text>().text = "English word assist ON";

        beforeEigoText = nowEigoTextObj.text;

        //　テキストファイルから読み込んだデータ
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("ignore_word", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        //ignore_word = TextLines.Split('\n'); 
        ignore_word = TextLines.Split(new char[] { '\r', '\n' });

        //DBの設定
        sqlDB = new SqliteDatabase("ejdict.sqlite3");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeEigoText != nowEigoTextObj.text)
        {

            beforeEigoText = nowEigoTextObj.text;
            CheckPotentialWords(nowEigoTextObj.text);
        }
    }

    public void CopyCanAlphabet(int[] str)
    {
        for(int i=0;i<26;i++)
            can_alphabet[i] = str[i];
    }


    public void CheckPotentialWords(string eigostr)
    {
        
        char[] eigochar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        Text CanWordText = GameObject.Find("CanWordText").GetComponent<Text>();

        // 英単語が1文字以下の時はCanWordTextを表示しない
        if (eigostr.Length < 1)
        {
            CanWordText.GetComponent<Text>().text = "";
            return;
        }

        //英単語としてカウントするか
        bool judge = false;

        // SQLite3のSQL文を作成　現在の選択中の英単語+%で英単語の可能性のある単語をSelectする
        string query = "select word,mean from items where word like '" + eigostr + "%'";
        DataTable dataTable = sqlDB.ExecuteQuery(query);

        int moutch_count = 0; //何単語作れるか

        string lastword = null; // マッチした英語の前回の単語を可能

        // SQL文で見つかった英単語を1つ1つ調べて、judge=trueなら作れる英単語を足す
        foreach (DataRow dr in dataTable.Rows)
        {
            judge = true;

            string word = (string)dr["word"];
            string str = (string)dr["mean"];

            if (word.Length < 2)
                judge = false;

            //単語が全て大文字か調べる
            int omoji = 0;
            foreach (char c in word)
            {
                if (Char.IsUpper(c))
                {
                    omoji++;
                }
                if(omoji>=2)
                {
                    judge = false;
                    break;
                }

            }

            if (judge)
            {

                // 前回と同じ英単語は除く（英語辞書データに同じ英単語がある場合があるための処理）
                if (lastword == word.ToUpperInvariant())
                {
                    judge = false;
                }
                else
                {
                    //除外リストの単語かチェック
                    for (int i = 0; i < ignore_word.Length; i++)
                    {
                        //もし除外リストの単語だったら
                        if (ignore_word[i].ToString() == word)
                        {
                            judge = false;
                            break;
                        }
                    }
                }
            }

            if (judge)
            {
                string tempword = word.ToUpperInvariant();


                // パズル画面中のアルファベット格納数can_alphabetをtemp_alphabetにコピー
                int[] temp_alphabet = new int[26];
                for (int i = 0; i < 26; i++)
                    temp_alphabet[i] = can_alphabet[i];

                // 英単語を1文字ずつアルファベットを調べてパズル画面のアルファベット格納数の配列から1つ減らす処理をする
                for (int i = 0; i < tempword.Length; i++)
                {
                    for (int j = 0; j < 26; j++)
                    {

                        if (tempword[i] == eigochar[j])
                        {
                            temp_alphabet[j]--;
                            //もしアルファベットの数がマイナスになったらパズル画面中のアルファベットが足りなくなるので、作れる可能性のある英単語ではないと判定
                            if (temp_alphabet[j] < 0)
                                judge = false;

                            break;
                        }
                        // 英語辞書データにスペースなど2単語の和訳があるのでそういうのは除外（作れる可能性のある英単語ではないと判定
                        else if (tempword[i] == ' ' || tempword[i] == '.' || tempword[i] == '/' || tempword[i] == '[' || tempword[i] == ']' || tempword[i] == '-' || tempword[i] == '\'' ||
                                 tempword[i] == '0' || tempword[i] == '1' || tempword[i] == '2' || tempword[i] == '3' || tempword[i] == '4' || tempword[i] == '5' || tempword[i] == '6' || tempword[i] == '7' || tempword[i] == '8' || tempword[i] == '9')
                        {
                            judge = false;
                            break;
                        }
                    }
                    if (judge == false)
                        break;
                }
            }

            // 上記処理を通過した英単語は作れる可能性のある英単語と判定する
            if (judge)
            {
                Debug.Log("英単語:" + word);
                lastword = word.ToUpperInvariant();
                moutch_count++;
            }

            // 作れる可能性のある英単語が多すぎる場合、処理が重くなるので最大10単語で打ち止めする（foreachからbreak）
            if (moutch_count > 10)
                break;
        }

        //Debug.Log("マッチ数:" + moutch_count);

        if (DataBase.GameMode_Easy)
        {
            // 言語判定をする
            if (DataBase.LangJapanese)
            {

                // 作れる可能性のある英単語が多すぎる場合、処理が重くなるので最大10単語で打ち止めする
                if (moutch_count > 10)
                    this.GetComponent<Text>().text = "10単語以上作れます";
                else if (moutch_count > 1)
                    this.GetComponent<Text>().text = moutch_count + "単語作れます";
                else if (moutch_count == 1)
                    this.GetComponent<Text>().text = moutch_count + "単語作れます";
                else if (moutch_count == 0)
                    this.GetComponent<Text>().text = "作成不可";
            }
            else
            {            // 作れる可能性のある英単語が多すぎる場合、処理が重くなるので最大10単語で打ち止めする
                if (moutch_count > 10)
                    this.GetComponent<Text>().text = "10+ words hit!";
                else if (moutch_count > 1)
                    this.GetComponent<Text>().text = moutch_count + " words hit!";
                else if (moutch_count == 1)
                    this.GetComponent<Text>().text = moutch_count + " word hit!";
                else if (moutch_count == 0)
                    this.GetComponent<Text>().text = "Unable to create.";

            }
        }
        /*
        if (moutch_count > 20)
            this.GetComponent<Text>().text = "20+ WORDS HIT";
        else if (moutch_count > 1)
            this.GetComponent<Text>().text = moutch_count + " WORDS HIT!";
        else if (moutch_count == 1)
            this.GetComponent<Text>().text = moutch_count + " WORD HIT!";
        else if (moutch_count == 0)
            this.GetComponent<Text>().text = "NO HIT";
        */
    }
 
}
