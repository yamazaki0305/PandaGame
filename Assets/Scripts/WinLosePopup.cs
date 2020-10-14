// Copyright (C) 2015-2017 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//山崎作成

namespace Ricimi
{
    // This class is responsible for popup management. Popups follow the traditional behavior of
    // automatically blocking the input on elements behind it and adding a background texture.
    public class WinLosePopup : Popup
    {

        public string scene = "<Insert scene name>";
        public float duration = 1.0f;
        public Color color = Color.black;

        float currentTime = 2f;

        private void Start()
        {

            
             // 結果画面にレクタングル広告を出す
             if (!DataBase.AdRectangleFlg)
                GameObject.Find("AdMob").GetComponent<AdRectangle>().AdBannerShow();
             

        }

        // Update is called once per frame
        void Update()
        {


            // 残り時間を計算する
            currentTime -= Time.deltaTime;

            // ゼロ秒以下にならないようにする

            GameObject mask1 = GameObject.Find("whitemask1");
            GameObject mask2 = GameObject.Find("whitemask2");
            GameObject mask3 = GameObject.Find("whitemask3");

            if (mask1)
            {

                if (currentTime <= 0.0f)
                {
                    mask1.SetActive(false);
                    mask2.SetActive(false);
                    mask3.SetActive(false);

                }
            }

        }

        public void LessonList()
        {

                DataBase.AdRectangleFlg = false;
                GameObject.Find("AdMob").GetComponent<AdRectangle>().AdBannerHide();

            Close();

            // インステ広告を表示
            RandomAd.ShowInterstitial();

            // 取得したシーンへ移動
            //SceneManager.LoadScene("LessonScenePortrait");
            Transition.LoadLevel(scene, duration, color);
        }

        public void Reload()
        {

                DataBase.AdRectangleFlg = false;
                GameObject.Find("AdMob").GetComponent<AdRectangle>().AdBannerHide();

            // インステ広告を表示
            RandomAd.ShowInterstitial();

            // 現在読み込んでいるシーンのインデックスを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 取得したシーンインデックスで再読込み
            SceneManager.LoadScene(currentSceneIndex);

            Close();
        }

        public void Reload_Lose()
        {

            DataBase.AdRectangleFlg = false;
            GameObject.Find("AdMob").GetComponent<AdRectangle>().AdBannerHide();
            
            // 現在読み込んでいるシーンのインデックスを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 取得したシーンインデックスで再読込み
            SceneManager.LoadScene(currentSceneIndex);

            Close();
        }

        public void Next()
        {

                DataBase.AdRectangleFlg = false;
                GameObject.Find("AdMob").GetComponent<AdRectangle>().AdBannerHide();

            // インステ広告を表示
            RandomAd.ShowInterstitial();

            if (DataBase.playLevel == 3 || DataBase.playLevel == 6 || DataBase.playLevel == 12 || DataBase.playLevel == 18 ||
                DataBase.playLevel == 27 || DataBase.playLevel == 36 || DataBase.playLevel == 45 || DataBase.playLevel == 54 || DataBase.playLevel == 63)
            {
                Close();

                Transition.LoadLevel(scene, duration, color);
            }
            else
            {

                DataBase.playLevel++;
                if (DataBase.playLevel > DataBase.MAXSTAGE)
                    DataBase.playLevel = 1;

                // 現在読み込んでいるシーンのインデックスを取得
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                // 取得したシーンインデックスで再読込み
                SceneManager.LoadScene(currentSceneIndex);

                Close();
            }
        }


        // We destroy the popup automatically 0.5 seconds after closing it.
        // The destruction is performed asynchronously via a coroutine. If you
        // want to destroy the popup at the exact time its closing animation is
        // finished, you can use an animation event instead.
        private IEnumerator RunPopupDestroy()
        {
            yield return new WaitForSeconds(destroyTime);
            Destroy(m_background);
            Destroy(gameObject);
        }

        private void AddBackground()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();

            m_background = new GameObject("PopupBackground");
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);
            image.CrossFadeAlpha(1.0f, 0.4f, false);

            var canvas = GameObject.Find("Canvas");
            m_background.transform.localScale = new Vector3(1, 1, 1);
            m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            m_background.transform.SetParent(canvas.transform, false);
            m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        private void RemoveBackground()
        {
            var image = m_background.GetComponent<Image>();
            if (image != null)
                image.CrossFadeAlpha(0.0f, 0.2f, false);
        }
    }
}
