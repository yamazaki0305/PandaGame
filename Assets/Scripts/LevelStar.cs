using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStar : MonoBehaviour {

    private int level;
    public GameObject Star1Obj;
    public GameObject Star2Obj;
    public GameObject Star3Obj;
    public GameObject LessonObj;

    // Use this for initialization
    void Start () {

        level = LessonObj.GetComponent<LevelController>().Level;

        SaveDataBase.loadAllLevel();

        switch (DataBase.level_star[level - 1])
        {
            case 0:
                Star1Obj.SetActive(false);
                Star2Obj.SetActive(false);
                Star3Obj.SetActive(false);
                break;
            case 1:
                Star1Obj.SetActive(true);
                Star2Obj.SetActive(false);
                Star3Obj.SetActive(false);
                break;
            case 2:
                Star1Obj.SetActive(true);
                Star2Obj.SetActive(true);
                Star3Obj.SetActive(false);
                break;
            case 3:
                Star1Obj.SetActive(true);
                Star2Obj.SetActive(true);
                Star3Obj.SetActive(true);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
