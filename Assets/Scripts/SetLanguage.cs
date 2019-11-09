using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguage : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // 言語を判定する
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            Debug.Log("Jap");
            DataBase.LangJapanese = true;
        }
        else
        {
            Debug.Log("Eng");
            DataBase.LangJapanese = false;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
