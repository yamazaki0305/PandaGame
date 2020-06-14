// Copyright (C) 2015-2017 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ricimi
{
    // This class is responsible for popup management. Popups follow the traditional behavior of
    // automatically blocking the input on elements behind it and adding a background texture.


    public class PauseController : Popup
    {
        public string scene = "<Insert scene name>";
        public float duration = 1.0f;
        public Color color = Color.black;

        void Start()
        {
            DataBase.bGamePause = true;

        }

        public new void Open()
        {
            AddBackground();
        }

        public new void Close()
        {

            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());

            DataBase.bGamePause = false;

        }

        public void LessonList()
        {

            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());

            DataBase.bGamePause = false;

            // インステ広告を表示
            RandomAd.ShowInterstitial();

            // 取得したシーンへ移動
            //SceneManager.LoadScene("LessonScenePortrait");
            Transition.LoadLevel(scene, duration, color);

        }
        public void ReloadButton()
        {
            // インステ広告を表示
            RandomAd.ShowInterstitial();

            DataBase.bGamePause = false;

            // 現在読み込んでいるシーンのインデックスを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 取得したシーンインデックスで再読込み
            SceneManager.LoadScene(currentSceneIndex);

        }
    }
}
