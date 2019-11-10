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

public class WordBookController : MonoBehaviour {

    public GameObject ItemWordPrefab;

    // 単語数を表示
    public Text WordCountText;

    // 未読チェックBOX
    public Toggle UnreadToggle;

    //ItemWordをContents下に表示する
    public Transform contentsTransform;


    public GameObject[] ItemWord;

    // 未読チェックを押した時
    public void UnreadToggleChange()
    {
        if (UnreadToggle.isOn)
        {
            UnreadToggleOn();
            //Debug.Log("チェック!!!");
        }
        else if (!UnreadToggle.isOn)
        {
            UnreadToggleOff();
            //Debug.Log("外す!!");
        }
    }

    void UnreadToggleOn()
    {

        for (int i = 0; i < ItemWord.Count(); i++)
        { 
            Destroy(ItemWord[i]);
        }
       
        //GameObject.Find("GameRoot").GetComponent<PuzzleMain>().InitScoreOutput();
        SaveDataBase.loadData();
        ItemWord = new GameObject[DataBase.WordDataList.Count];
        WordCountText.text = DataBase.WordDataList.Count.ToString();

        // WordBookがNewアイコンの時
        if (DataBase.NewWordBook)
        {
            DataBase.NewWordBook = false;
            GameObject.Find("NewIcon").SetActive(false);
            SaveDataBase.SetBool("SAVE_NEW_WORDBOOK", DataBase.NewWordBook);
        }

        int k = 0;
        for (int i = 0; i < DataBase.WordDataList.Count; i++ )
        {
            Vector2 pos = new Vector2(0, 0);

            // 未読の時のみインスタンスを作成
            if (DataBase.WordDataList[i].badge)
            {

                // スクリプトからインスタンス（動的にゲームオブジェクトを指定数だけ作る
                ItemWord[k] = Instantiate(ItemWordPrefab, pos, Quaternion.identity);
                // アルファベットブロックにする
                //PuzzleData[i, j].GetComponent<BlockData>().setup(BlockType.ALPHABET, RandomMake.alphabet(), false, i, j);
                ItemWord[k].name = "ItemWord"; // GameObjectの名前を決めている
                ItemWord[k].transform.SetParent(contentsTransform);
                ItemWord[k].transform.localPosition = pos;
                ItemWord[k].transform.localScale = ItemWordPrefab.transform.localScale;

                ItemWord[k].transform.Find("EngWordText").gameObject.GetComponent<Text>().text = DataBase.WordDataList[i].word;
                ItemWord[k].transform.Find("DateText").gameObject.GetComponent<Text>().text = "Date:" + DataBase.WordDataList[i].savetime + " Count:" + DataBase.WordDataList[i].count;

                // Cellにword_idを格納
                ItemWord[k].GetComponent<CellItem>().word_id = DataBase.WordDataList[i].item_id;
                // Cellに配列Noを格納
                ItemWord[k].GetComponent<CellItem>().array_no = i;
                k++;
            }

        }
                
    }

    void UnreadToggleOff()
    {

        for (int i = 0; i < ItemWord.Count(); i++)
        {
            Destroy(ItemWord[i]);
        }
        Start();
    }

    // Use this for initialization
    void Start () {

        //GameObject.Fin").GetComponent<PuzzleMain>().InitScoreOutput();
        SaveDataBase.loadData();
        ItemWord = new GameObject[DataBase.WordDataList.Count];
        WordCountText.text = DataBase.WordDataList.Count.ToString();

        // WordBookがNewアイコンの時
        if (DataBase.NewWordBook)
        {
            DataBase.NewWordBook = false;
            GameObject.Find("NewIcon").SetActive(false);
            SaveDataBase.SetBool("SAVE_NEW_WORDBOOK", DataBase.NewWordBook);
        }

        for (int i=0;i< DataBase.WordDataList.Count; i++)
        {
            Vector2 pos = new Vector2(0, 0);

            // スクリプトからインスタンス（動的にゲームオブジェクトを指定数だけ作る
            ItemWord[i] = Instantiate(ItemWordPrefab, pos, Quaternion.identity);
            // アルファベットブロックにする
            //PuzzleData[i, j].GetComponent<BlockData>().setup(BlockType.ALPHABET, RandomMake.alphabet(), false, i, j);
            ItemWord[i].name = "ItemWord"; // GameObjectの名前を決めている
            ItemWord[i].transform.SetParent(contentsTransform);
            ItemWord[i].transform.localPosition = pos;
            ItemWord[i].transform.localScale = ItemWordPrefab.transform.localScale;

            ItemWord[i].transform.Find("EngWordText").gameObject.GetComponent<Text>().text = DataBase.WordDataList[i].word;
            ItemWord[i].transform.Find("DateText").gameObject.GetComponent<Text>().text = "Date:"+DataBase.WordDataList[i].savetime + " Count:"+DataBase.WordDataList[i].count;

            if (!DataBase.WordDataList[i].badge)
            {
                //ItemWord[i].transform.Find("Badge").gameObject.GetComponent<SpriteRenderer>().enabled = false;
                ItemWord[i].transform.Find("Badge").gameObject.SetActive(false);
            }

            // Cellにword_idを格納
            ItemWord[i].GetComponent<CellItem>().word_id = DataBase.WordDataList[i].item_id;
            // Cellに配列Noを格納
            ItemWord[i].GetComponent<CellItem>().array_no = i;
        }


    }

    // Update is called once per frame
    void Update () {
		
	}
}
