﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using System; //DateTimeを使用する為追加。

public class PauseManager : MonoBehaviour
{
    // pause Prefab
    public GameObject popupPausePrefab;

    void OnApplicationPause(bool status)
    {
        if (status)
        {

            if (GameObject.Find("ResumePortrait(Clone)"))
            {
                //ResumeResumePortraitが存在する時は、追加しない
            }
            else
            {
                // 動画リワードやインステ広告でpauseのとき＝bGameAdStop:true のときはResumeにしない
                if (!DataBase.AdRectangleFlg && !DataBase.bGameAdStop)
                {
                    //動画広告終了から5秒経過しないと、ResumePopupは表示しない
                    if (DataBase.AdResumeFlg)
                    {
                        Debug.Log("resumeひらくと");
                        GameObject.Find("AdMob").GetComponent<AdRectangle>().AdBannerShow();

                        Canvas m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

                        var popup = Instantiate(popupPausePrefab) as GameObject;
                        popup.SetActive(true);
                        popup.transform.localScale = Vector3.zero;
                        popup.transform.SetParent(m_canvas.transform, false);
                    }
                }

                //popup.GetComponent<Popup>().Open();
            }
        }
        else
        {
            Debug.Log("resume!");


        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
