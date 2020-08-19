// Copyright (C) 2015-2017 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using System.Collections;

namespace Ricimi
{
    // This class is responsible for creating and opening a popup of the given prefab and add
    // it to the UI canvas of the current scene.
    public class ScorePopup : MonoBehaviour
    {
        public GameObject ScorePrefab;
        public GameObject GameClearPrefab;

        protected Canvas m_canvas;
        protected Canvas c_canvas;
        public bool bWindow = false;
        Vector3 startPosition;

        GameObject popup;


        protected void Start()
        {
            m_canvas = GameObject.Find("ScoreCanvas").GetComponent<Canvas>();
            c_canvas = GameObject.Find("TopContentCanvas").GetComponent<Canvas>();
        }

        public virtual void GameClearOutput()
        {
            var popup = Instantiate(GameClearPrefab) as GameObject;
            popup.SetActive(true);

            var rect = popup.GetComponent<RectTransform>();
            //rect.SetAnchor(AnchorPresets.BottonCenter);
            rect.SetAnchor(AnchorPresets.MiddleCenter);
            startPosition = new Vector3(0, 560, 1);

            //startPosition = new Vector3(0, 1700, 1);

            popup.transform.localPosition = startPosition;
            popup.transform.SetParent(m_canvas.transform, false);

            popup.GetComponent<ScoreLiner>().GameClear();

        }

        public virtual void MovesOutput(int count)
        {
            var popup = Instantiate(ScorePrefab) as GameObject;
            popup.SetActive(true);

            var rect = popup.GetComponent<RectTransform>();
            //rect.SetAnchor(AnchorPresets.BottonCenter);

            rect.SetAnchor(AnchorPresets.MiddleCenter);
            startPosition = new Vector3(0, 260, 1);
            //startPosition = new Vector3(0, 1400, 1);

            popup.transform.localPosition = startPosition;
            popup.transform.SetParent(m_canvas.transform, false);

            popup.GetComponent<ScoreLiner>().MovesScore(count);


        }

        public virtual void ScoreOutput(int length)
        {
            var popup = Instantiate(ScorePrefab) as GameObject;
            popup.SetActive(true);

            var rect = popup.GetComponent<RectTransform>();
            //rect.SetAnchor(AnchorPresets.BottonCenter);

            rect.SetAnchor(AnchorPresets.MiddleCenter);
            startPosition = new Vector3(0, 260, 1);

            //startPosition = new Vector3(0, 1400, 1);

            popup.transform.localPosition = startPosition;
            popup.transform.SetParent(m_canvas.transform, false);

            popup.GetComponent<ScoreLiner>().UpperScore(length);

        }

        public virtual void RescueOutput(int count)
        {
            var popup = Instantiate(ScorePrefab) as GameObject;
            popup.SetActive(true);

            var rect = popup.GetComponent<RectTransform>();
            //rect.SetAnchor(AnchorPresets.BottonCenter);

            //startPosition = new Vector3(0, 1400, 1);

            rect.SetAnchor(AnchorPresets.MiddleCenter);
            startPosition = new Vector3(0, 260, 1);

            popup.transform.localPosition = startPosition;
            popup.transform.SetParent(m_canvas.transform, false);

            popup.GetComponent<ScoreLiner>().UpperRescue(count);

        }

    }
}