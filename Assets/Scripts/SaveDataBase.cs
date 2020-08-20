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

public class DataBase
{
    public static bool DebugFlg = false; // デバッグモードにするか

    public static int PLAY = 0;
    public static int CLEAR = 1;
    public static int CLEAR_AFTER = 2;
    public static int MISS = 3;
    public static int MAXSTAGE = 40;

    public static int playLevel = 22;
    public static int LastPlayLevel = 1;
    public static int openLevel = 1;
    public static int RecordWord = 0;
    public static bool NewWordBook = false; //WordBookにNewアイコンをつけるかのフラグ
    public static float AdRate = 0.5f; // インステ広告を表示する
    public static bool AdRewordOK = false; // 動画広告を最後まで見るとONになる
    public  enum AdRewordFlg
    {
        None,
        Moves0,
        Hummer,
        Comic
    }
    public static AdRewordFlg AdFlg = AdRewordFlg.None;

    public static bool AdRealTest = false; // trueのときは本番広告を使用する
    public static bool LangJapanese = true; // 言語が日本語:true 英語:false
    public static int STAR_TOUCH_SCORE = 1000; // スコアにタッチした時
    public static int HammerCount = 3; // ハンマーが打てる回数
    public static int SelectEpNo = 1; // LessonListで選択されたEpNo

    // 200ステージ分用意
    public static int[] level_star = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static int[] ep_movie = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 1:視聴

    //スコアoutputの一時的な保存
    public static bool bScoreOutputFlg = false;
    public static bool bScoreOutputNow = false;
    public static int score_count = 0;
    public static bool bRescueStarOutputFlg = false;
    public static bool bRescueStarOutputNow = false;
    public static int RescueStarCount = 0;
    public static bool bMovesOutputFlg = false;
    public static bool bMovesOutputNow = false;
    public static bool bGameClearFlg = false;
    public static bool bGameClearEnd = false;
    public static bool bGameClearAdEnd = false;
    public static bool bGameAdStop = false; //広告表示中でゲームを止める

    //メニュー画面表示フラグ
    public static bool bGamePause = false;

    //作成済の英単語を格納
    public static List<Word> WordDataList = new List<Word>();
    public static List<int> WordIDList = new List<int>();
    //public static List<string> WordStrList = new List<string>();

    //ゲーム中作成したWordIDを保存
    public static List<int> WordIDAddList = new List<int>();
    public static List<string> WordStrAddList = new List<string>();

    public static int newword_count = 0;

    //ゲーム中のLevelの情報を保存
    public static int BestScore = 0;

    //TransWindowに表示するWordのitem_id
    public static int Word_item_id = 0;
    public static bool Word_new_flg = false;

    //Scene名を格納
    public static string SceneName;

    //ゲームの難易度(Easy:trueの時、英単語予測を表示）
    public static bool GameMode_Easy = true;
}

/*
public class WordData
{
    public int item_id;
    public string word;
    public int count;
    public string savetime;

}
*/

[Serializable]
public class Word
{
    public int item_id;
    public string word;
    public int count;
    public string savetime;
    public bool badge;
 
}

[Serializable]
class CreateWordList
{
    public List<Word> WordList = new List<Word>();
}

[Serializable]
class LevelManage
{
    public int level;
    public int BestScore;
    public bool b_level_clear = false;
    public bool b_level_letter = false;
    public bool b_level_score = false;

}

[Serializable]
class LevelManageList
{
    public List<LevelManage> LevelList = new List<LevelManage>();
}

public class SaveDataBase : MonoBehaviour
{

    // セーブデータの情報など
    const string SAVE_OPEN_LEVEL = "SAVE_OPEN_LEVEL";
    const string SAVE_LASTPLAY_LEVEL = "SAVE_LASTPLAY_LEVEL";
    const string SAVE_STAR_LEVEL = "SAVE_STAR_LEVEL";
    const string SAVE_LEVEL_MANAGE = "SAVE_LEVEL_MANAGE";
    const string SAVE_CREATE_WORD = "CREATE_WORD";
    const string SAVE_NEW_WORDBOOK = "SAVE_NEW_WORDBOOK";
    const string SAVE_GAME_MODE = "SAVE_GAME_MODE"; //Easy 1 Hard 0
    const string SAVE_HAMMER_COUNT = "SAVE_HAMMER_COUNT";
    const string SAVE_EP_MOVIE = "SAVE_EP_MOVIE"; 

    public static void saveData()
    {
        int level;

        // クリアした時のレベル＝DataBase.nowLevel
        // これまでの最高レベル＝DataBase.openLevel
        if (DataBase.playLevel + 1 > DataBase.openLevel)
            level = DataBase.playLevel + 1;
        else
            level = DataBase.openLevel;

        //********** 開始 **********//
        PlayerPrefs.SetInt(SAVE_OPEN_LEVEL, level);
        PlayerPrefs.SetInt(SAVE_LASTPLAY_LEVEL, DataBase.LastPlayLevel);
        PlayerPrefs.Save();

        string str = "";
        for (int i = 0; i < 200; i++)
            str += DataBase.level_star[i].ToString();

        PlayerPrefs.SetString(SAVE_STAR_LEVEL, str);
        PlayerPrefs.Save();

        saveLevel(DataBase.playLevel);
        saveWord();

        SetBool(SAVE_NEW_WORDBOOK, DataBase.NewWordBook); //SaveWordでフラグを立てている

        //********** 終了 **********// 

    }

    public static void saveWord()
    {
        Word word = new Word();
        CreateWordList wordlist;

        if (PlayerPrefs.HasKey(SAVE_CREATE_WORD))
        {
            wordlist = PlayerPrefsUtils.GetObject<CreateWordList>(SAVE_CREATE_WORD);
        }
        else
        {
            wordlist = new CreateWordList();
        }

        for (int k = 0; k < DataBase.WordIDAddList.Count(); k++)
        {
            bool saveflg = true;

            //すでに保存されている単語か調べる
            for (int i = 0; i < wordlist.WordList.Count(); i++)
            {
                //登録済の単語だったら
                if (wordlist.WordList[i].item_id == DataBase.WordIDAddList[k])
                {
                    wordlist.WordList[i].count++;
                    wordlist.WordList[i].savetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                    saveflg = false;
                    break;
                }
            }

            //新しい単語だったら
            if (saveflg)
            {
                Word tempword = new Word();
                tempword.item_id = DataBase.WordIDAddList[k];
                tempword.word = DataBase.WordStrAddList[k];
                tempword.count = 1;
                tempword.savetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                tempword.badge = true;

                wordlist.WordList.Add(tempword);
                DataBase.newword_count++;

                // WordBookにNewアイコンをつける
                DataBase.NewWordBook = true;
            }
        }

        //参考サイト https://qiita.com/tetsu8/items/96b8b889c57eb55125d1
        //wordlist.WordList.Sort((a, b) => a.item_id - b.item_id); //ID昇順
        wordlist.WordList.Sort((a, b) => string.Compare(b.savetime, a.savetime)); //savetime降順
        PlayerPrefsUtils.SetObject(SAVE_CREATE_WORD, wordlist);

        //レコードを更新
        DataBase.RecordWord += DataBase.newword_count;

    }

    // バッジをOFFにして保存
    public static void saveBadgeOff(int _word_id)
    {
        Word word = new Word();
        CreateWordList wordlist;

        if (PlayerPrefs.HasKey(SAVE_CREATE_WORD))
        {
            wordlist = PlayerPrefsUtils.GetObject<CreateWordList>(SAVE_CREATE_WORD);
        }
        else
        {
            return;
        }

        //バッジをOFFにする単語を調べる
        for (int i = 0; i < wordlist.WordList.Count(); i++)
        {
            //登録済の単語だったら
            if (wordlist.WordList[i].item_id == _word_id)
            {
                wordlist.WordList[i].badge = false;
                break;
            }
        }

        PlayerPrefsUtils.SetObject(SAVE_CREATE_WORD, wordlist);

    }


    public static void loadData()
    {
        // Gameの難易度を取得
        if (PlayerPrefs.HasKey(SAVE_GAME_MODE))
        {
            DataBase.GameMode_Easy = GetBool(SAVE_GAME_MODE, true);

        }
        if (PlayerPrefs.HasKey(SAVE_OPEN_LEVEL))
        {
            DataBase.openLevel = PlayerPrefs.GetInt(SAVE_OPEN_LEVEL, 1);

        }
        if (PlayerPrefs.HasKey(SAVE_LASTPLAY_LEVEL))
        {
            DataBase.LastPlayLevel = PlayerPrefs.GetInt(SAVE_LASTPLAY_LEVEL, 1);

        }
        if (PlayerPrefs.HasKey(SAVE_STAR_LEVEL))
        {
            string stTarget = PlayerPrefs.GetString(SAVE_STAR_LEVEL, "");
            // stTarget を Char 型の 1 次元配列に変換する
            char[] chArray1 = stTarget.ToCharArray();
            for (int i = 0; i < 200; i++)
            {
                DataBase.level_star[i] = int.Parse(chArray1[i].ToString());
            }
        }
        if(PlayerPrefs.HasKey(SAVE_NEW_WORDBOOK))
        {
            DataBase.NewWordBook = GetBool(SAVE_NEW_WORDBOOK,false);
        }

        if (PlayerPrefs.HasKey(SAVE_CREATE_WORD))
        {

            CreateWordList wordlist = PlayerPrefsUtils.GetObject<CreateWordList>(SAVE_CREATE_WORD);

            DataBase.WordIDList.Clear();
            DataBase.WordDataList.Clear();

            for (int i = 0; i < wordlist.WordList.Count(); i++)
            {
                DataBase.WordIDList.Add(wordlist.WordList[i].item_id);
                DataBase.WordDataList.Add(wordlist.WordList[i]);
            }
            DataBase.RecordWord = wordlist.WordList.Count();

        }
    }

    public static void saveHammerCount()
    {
        if (PlayerPrefs.HasKey(SAVE_HAMMER_COUNT))
        {
            PlayerPrefs.SetInt(SAVE_HAMMER_COUNT, DataBase.HammerCount);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt(SAVE_HAMMER_COUNT, DataBase.HammerCount);
            PlayerPrefs.Save();
        }
    }

    public static void loadHammerCount()
    {
        if (PlayerPrefs.HasKey(SAVE_HAMMER_COUNT))
        {

            DataBase.HammerCount = PlayerPrefs.GetInt(SAVE_HAMMER_COUNT, 3);

        }
        else
        {
            DataBase.HammerCount = 3;
        }
    }

    public static bool loadGameMode()
    {
        bool b = GetBool(SAVE_GAME_MODE, true);
        return b;
    }

    public static void saveGameMode(bool gamemode)
    {
        SetBool(SAVE_GAME_MODE, gamemode);
    }

    public static void saveLevel(int level)
    {
        LevelManageList level_manage;

        if (PlayerPrefs.HasKey(SAVE_LEVEL_MANAGE))
        {
            level_manage = PlayerPrefsUtils.GetObject<LevelManageList>(SAVE_LEVEL_MANAGE);
        }
        else
        {
            level_manage = new LevelManageList();
        }

        // 今のLevelがまだ保存されていない時
        if (level > level_manage.LevelList.Count())
        {
            LevelManage leveldata = new LevelManage();

            leveldata.level = level;
            leveldata.BestScore = DataBase.BestScore;

            level_manage.LevelList.Add(leveldata);
        }
        else
        {
            level_manage.LevelList[level - 1].level = level;
            level_manage.LevelList[level - 1].b_level_clear = true;
            if (DataBase.BestScore > level_manage.LevelList[level - 1].BestScore)
                level_manage.LevelList[level - 1].BestScore = DataBase.BestScore;
        }

        PlayerPrefsUtils.SetObject(SAVE_LEVEL_MANAGE, level_manage);

    }

    public static void loadLevel(int level)
    {
        if (PlayerPrefs.HasKey(SAVE_LEVEL_MANAGE))
        {
            LevelManageList level_manage = PlayerPrefsUtils.GetObject<LevelManageList>(SAVE_LEVEL_MANAGE);

            if (level > level_manage.LevelList.Count())
            {
                DataBase.BestScore = 0;
            }
            else
            {
                DataBase.BestScore = level_manage.LevelList[level - 1].BestScore;
            }

        }
        else
        {
            DataBase.BestScore = 0;
        }
    }

    public static void saveEp()
    {

        string str = "";
        for (int i = 0; i < 10; i++)
        {
            str += DataBase.ep_movie[i].ToString();
        }

        PlayerPrefs.SetString(SAVE_EP_MOVIE, str);
        PlayerPrefs.Save();

    }

    public static void loadEp()
    {

        if (PlayerPrefs.HasKey(SAVE_EP_MOVIE))
        {
            string stTarget = PlayerPrefs.GetString(SAVE_EP_MOVIE, "");
            // stTarget を Char 型の 1 次元配列に変換する
            char[] chArray1 = stTarget.ToCharArray();
            for (int i = 0; i < 10; i++)
            {
                DataBase.ep_movie[i] = int.Parse(chArray1[i].ToString());
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                DataBase.ep_movie[i] = 0;
            }

        }

    }

    public static void loadAllLevel()
    {
        if (PlayerPrefs.HasKey(SAVE_STAR_LEVEL))
        {
            string stTarget = PlayerPrefs.GetString(SAVE_STAR_LEVEL, "");
            // stTarget を Char 型の 1 次元配列に変換する
            char[] chArray1 = stTarget.ToCharArray();
            for (int i = 0; i < 200; i++)
            {
                DataBase.level_star[i] = int.Parse(chArray1[i].ToString());
            }
        }
    }

    public static bool GetBool(string key, bool defalutValue)
    {
        var value = PlayerPrefs.GetInt(key, defalutValue ? 1 : 0);
        return value == 1;
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}
