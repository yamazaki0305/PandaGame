using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameModeManager : MonoBehaviour
{

    private Toggle toggle1;
    private ToggleGroup toggleGroup1;
    public Toggle toggleEasy, toggleHard;
    public Text assist_help_text;

    void Start()
    {

            assist_help_text.text = "※Hardにすると英単語\r\nアシスト機能がOFF";


        toggle1 = GetComponent<Toggle>();
        toggleGroup1 = GetComponent<ToggleGroup>();
        DataBase.GameMode_Easy = SaveDataBase.loadGameMode();

        if (DataBase.GameMode_Easy)
        {
            toggleEasy.isOn = true;
            toggleHard.isOn = false;
        }
        else
        {
            toggleEasy.isOn = false;
            toggleHard.isOn = true;
        }
    }

    void Update()
    {
        //Debug.Log(toggleGroup1.AnyTogglesOn());  // いずれかのトグルがオンになっているか
    }

    // このメソッドをOn Click()に指定するとボタンを押したときにメソッドを呼び出す
    public void OnClick()
    {
        //Debug.Log(toggle1.isOn);  // トグルの状態

        if (toggleGroup1.AnyTogglesOn())
        {
            toggle1 = toggleGroup1.ActiveToggles().FirstOrDefault();  // チェックが付いているトグルを取得
            if (toggle1.name == "Easy")
            {
                DataBase.GameMode_Easy = true;
                SaveDataBase.saveGameMode(true);
                Debug.Log("Easy");
            }
            else if (toggle1.name == "Hard")
            {
                DataBase.GameMode_Easy = false;
                SaveDataBase.saveGameMode(false);
                Debug.Log("Hard");
            }
        }
    }
}