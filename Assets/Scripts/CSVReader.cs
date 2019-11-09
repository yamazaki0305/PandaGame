using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum CSVColumn
{
    level = 0,
    neko_sum = 1,
    neko1 = 2,
    neko2 = 3,
    neko3 = 4,
    hand = 5,
    m_letter = 6,
    m_score = 7,
    timeflg = 8,
    time = 9
}

public class CSVReader {

    /*
    TextAsset csvFile; // CSVファイル
    public int height; // CSVの行数
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    */

    /*
    void Start()
    {
        csvFile = Resources.Load("CSV/StageListData") as TextAsset; // Resouces下のCSV読み込み
        //csvFile = Resources.Load("stagedata") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() > -1) // reader.Peaekが0になるまで繰り返す
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
            height++; // 行数加算
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        Debug.Log(csvDatas[0][1]); 

    }
    */

    public static List<string[]> import(string file)
    {
        TextAsset csvFile; // CSVファイル
        int height=0; // CSVの行数
        List<string[]> csvDatas = new List<string[]>();

        csvFile = Resources.Load(file) as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() > -1) // reader.Peaekが0になるまで繰り返す
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
            height++; // 行数加算
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        //Debug.Log(csvDatas[0][1]);

        return csvDatas;
    }

    // 疑問
    // TextAssetはナニモン？
    // StringReaderはナニモン？
    // わざわざリストに入れてるけどTextAssetのままでは使えないの？

}
