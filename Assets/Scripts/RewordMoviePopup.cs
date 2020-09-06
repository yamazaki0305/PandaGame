using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{

    public class RewordMoviePopup : Popup
    {

        void Start()
        {

        }

        private void Update()
        {
            if (DataBase.AdRewordOK && DataBase.AdFlg == DataBase.AdRewordFlg.Comic)
            {

                DataBase.AdFlg = DataBase.AdRewordFlg.None;

                DataBase.AdRewordOK = false;

                Debug.Log("EPP" + DataBase.SelectEpNo);
                DataBase.ep_movie[DataBase.SelectEpNo] = 1; // エピソード開放
                SaveDataBase.saveEp();

                // 取得したシーンへ移動
                //SceneManager.LoadScene("RewordScenePortrait");
                Transition.LoadLevel("RewordScenePortrait", 1.0f, Color.black);
            }

            /*
            if (DataBase.AdRewordOK && DataBase.AdFlg == DataBase.AdRewordFlg.Moves0)
            {
                Debug.Log("bbbbbbbbbbbb");

                DataBase.AdRewordOK = false;
                GameObject.Find("GameRoot").GetComponent<PuzzleMain>().StatusData.AdRewordUpdate();
                Close();
            }
            */
        }

        public void AdButton()
        {

            DataBase.AdFlg = DataBase.AdRewordFlg.Comic;

            // 動画広告を表示
            GameObject.Find("AdMob").GetComponent<AdReward>().UserOptToWatchAd();
            GameObject.Find("YesButton").SetActive(false);

            var animator = GetComponent<Animator>();

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