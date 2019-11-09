// Copyright (C) 2015-2017 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // This class is responsible for popup management. Popups follow the traditional behavior of
    // automatically blocking the input on elements behind it and adding a background texture.
    public class Moves0Popup : Popup
    {
        public Text RecommendText;

        void Start()
        {
            if(DataBase.LangJapanese)
                RecommendText.text = "動画を視聴して、残り回数や\r\n時間を回復しますか？";            else                RecommendText.text = "Do you watch videos and\r\nrecover time with Moves?";
        }

        private void Update()
        {
            if (DataBase.AdRewordOK)
            {
                DataBase.AdRewordOK = false;
                GameObject.Find("GameRoot").GetComponent<PuzzleMain>().StatusData.AdRewordUpdate();
                Close();
            }
        }

        public void AdButton()
        {

            // 動画広告を表示
            GameObject.Find("AdMob").GetComponent<AdReward>().UserOptToWatchAd();
            GameObject.Find("YesButton").SetActive(false);
            //GameObject obj = GameObject.Find("YesButton");
            //obj.GetComponent<Renderer>().enabled = false;

            // 動画広告を見た時のりワード処理
            //GameObject.Find("GameRoot").GetComponent<PuzzleMain>().StatusData.AdRewordUpdate();

            var animator = GetComponent<Animator>();
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            //    animator.Play("Close");

            //DataBase.bGamePause = false;

            //RemoveBackground();
            //StartCoroutine(RunPopupDestroy());
        }

        public new void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            DataBase.bGamePause = false;

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());
        }
       
    }
}
