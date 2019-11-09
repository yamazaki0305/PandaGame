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

//参考URL
//http://baba-s.hatenablog.com/entry/2016/01/01/100000

[Serializable]
class Character
{
    public int Id;
    public string Name;
    public int[] SkillIdList;
}

public class PlayerPrefsTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        string stTarget = PlayerPrefs.GetString("hoge");

        var key = "hoge";
        var chara = new Character
        {
            Id = 25,
            Name = "ライチュウ",
            SkillIdList = new[] { 1, 2, 3, 4 },
        };

        PlayerPrefsUtils.SetObject(key, chara);

        // PlayerPrefsを展開
        var result = PlayerPrefsUtils.GetObject<Character>(key);

        Debug.Log("Name: " + result.Name);

        for(int i=0;i< result.SkillIdList.Length;i++)
        Debug.Log("SkillIdList: " + result.SkillIdList[i]);


        stTarget = PlayerPrefs.GetString("hoge");
        Debug.Log("JSON読み込み:"+stTarget);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
