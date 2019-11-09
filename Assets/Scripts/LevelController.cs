using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public int Level;

    public GameObject NowObj;
    public GameObject LockObj;
    public GameObject AnimalObj;
    public GameObject OoenObj;


    // Use this for initialization
    void Start () {

        SaveDataBase.loadData();

        Debug.Log("complete;" + DataBase.openLevel);

        // OpenLevelとこのLevelが同じ時
        if( DataBase.openLevel == Level )
        {
            NowObj.SetActive(true);
            OoenObj.SetActive(false);
            LockObj.SetActive(false);
            AnimalObj.SetActive(false);
        }
        // クリア済のLevelの時
        else if (DataBase.openLevel > Level)
        {
            NowObj.SetActive(false);
            OoenObj.SetActive(true);
            LockObj.SetActive(false);
            AnimalObj.SetActive(false);
        }
        // 開放していないLevelの時
        else if (DataBase.openLevel < Level)
        {
            NowObj.SetActive(false);
            OoenObj.SetActive(false);
            LockObj.SetActive(true);
            AnimalObj.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
