using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 開発している画面を元に縦横比取得 (縦画面)
        float developAspect = 750.0f / 1334.0f;

        // 実機のサイズを取得して、縦横比取得
        float deviceAspect = (float)Screen.width / (float)Screen.height;
         
        // 実機と開発画面との対比
        float scale = deviceAspect / developAspect;
         
        Camera mainCamera = Camera.main;

        // カメラに設定していたorthographicSizeを実機との対比でスケール
        float deviceSize = mainCamera.orthographicSize;

        // scaleの逆数
        float deviceScale = 1.0f / scale;

        Camera.main.orthographicSize = deviceSize * deviceScale;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
