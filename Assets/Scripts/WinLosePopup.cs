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
        //public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

        //public float destroyTime = 0.5f;

        //private GameObject m_background;

        /*
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
        }
        */
        public string scene = "<Insert scene name>";
        public float duration = 1.0f;
        public Color color = Color.black;

        public void LessonList()
        {
            Close();

            // インステ広告を表示
            RandomAd.ShowInterstitial();

            // 取得したシーンへ移動
            //SceneManager.LoadScene("LessonScenePortrait");
            Transition.LoadLevel(scene, duration, color);
        }

        public void Reload()
        {
            // インステ広告を表示
            RandomAd.ShowInterstitial();

            // 現在読み込んでいるシーンのインデックスを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 取得したシーンインデックスで再読込み
            SceneManager.LoadScene(currentSceneIndex);

            Close();
        }

        public void Next()
        {

            // インステ広告を表示
            RandomAd.ShowInterstitial();

            DataBase.playLevel++;
            if (DataBase.playLevel > DataBase.MAXSTAGE)
                DataBase.playLevel = 1;

            // 現在読み込んでいるシーンのインデックスを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 取得したシーンインデックスで再読込み
            SceneManager.LoadScene(currentSceneIndex);

            Close();
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
