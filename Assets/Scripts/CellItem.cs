using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Globalization;
using Ricimi;


namespace Ricimi
{
    public class CellItem : MonoBehaviour
    {
        // Cellのword_idを格納
        public int word_id;
        // Cellの配列Noを格納
        public int array_no;

        // Popupをセット
        public GameObject popupPrefab;
        protected Canvas m_canvas;
        /*
        [SerializeField]
        private Text header = null;

        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Text duration = null;
        */

        public virtual void OpenPopup()
        {
            DataBase.Word_item_id = word_id;

            var popup = Instantiate(popupPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.SetParent(m_canvas.transform, false);
            popup.GetComponent<Popup>().Open2();

            if (DataBase.WordDataList[array_no].badge)
            {
                DataBase.WordDataList[array_no].badge = false;
                this.transform.Find("Badge").gameObject.SetActive(false);
                SaveDataBase.saveBadgeOff(word_id);
            }
        }

        void Start()
        {
            //Scene名を格納
            DataBase.SceneName = SceneManager.GetActiveScene().name;

            m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        /// <summary>Mono Method</summary>
        void Update()
        {

        }

        /*
        public void SetInformation(string header, string title, string duration)
        {
            this.header.text = header;
            this.title.text = title;
            this.duration.text = duration;
        }
        */
    }
}