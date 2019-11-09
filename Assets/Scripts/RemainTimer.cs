using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プログラムの参考URL
//https://qiita.com/nonkapibara/items/edb497f60e991bcbfe98

[RequireComponent(typeof(Text))]
public class RemainTimer : MonoBehaviour
{
    [SerializeField] float gameTime = 20.0f;        // ゲーム制限時間 [s]
    Text uiText;                                    // UIText コンポーネント
    float currentTime;                              // 残り時間タイマー

    void Start()
    {
        // Textコンポーネント取得
        uiText = GetComponent<Text>();
        // 残り時間を設定
        currentTime = gameTime;
    }

    void Update()
    {
        // 残り時間を計算する
        currentTime -= Time.deltaTime;

        // ゼロ秒以下にならないようにする
        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
        }
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        int mseconds = Mathf.FloorToInt((currentTime - minutes * 60 - seconds) * 1000);
        uiText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, mseconds);
    }
}