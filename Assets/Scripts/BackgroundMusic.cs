// Copyright (C) 2015-2017 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ricimi
{
    // This class manages the audio source used to play the looping background song
    // in the demo. The player can choose to mute the music, and this preference is
    // persisted via Unity's PlayerPrefs.
    public class BackgroundMusic : MonoBehaviour
    {
        public static BackgroundMusic Instance;

        private AudioSource m_audioSource;

        private string last_scene_name;
        private float bgm_vol = 1.0f;

        private void Awake()
        {
            last_scene_name = SceneManager.GetActiveScene().name;
            Debug.Log("scene:" + last_scene_name);

            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
                m_audioSource = GetComponent<AudioSource>();
                m_audioSource.ignoreListenerVolume = true;
                m_audioSource.volume = PlayerPrefs.GetInt("music_on",1);
                AudioListener.volume = PlayerPrefs.GetInt("sound_on",1);
            }
        }
        private void Update()
        {

            // BGMÄ¶’†‚ÅL’†‚ÍBGM‚ÌÄ¶‚ðŽ~‚ß‚é 
            if(m_audioSource.isPlaying == true && DataBase.bGameAdStop == true)
                m_audioSource.Stop();
            // BGM–¢Ä¶‚ÅL”ñ•\Ž¦‚ÍBGM‚ðÄ¶‚·‚é
            else if(m_audioSource.isPlaying == false && DataBase.bGameAdStop == false)
                m_audioSource.Play();

            string now_scene_name = SceneManager.GetActiveScene().name;

            if (now_scene_name == last_scene_name && m_audioSource.volume == 0)
            {
                switch (now_scene_name)
                {
                    case "HomeScenePortrait":
                        break;
                    case "LessonScenePortrait":
                        m_audioSource.loop = true;
                        //m_audioSource.clip = Resources.Load("Sound/happy-steps-sc1", typeof(AudioClip)) as AudioClip;
                        m_audioSource.clip = Resources.Load("Sound/BGM/wave_14", typeof(AudioClip)) as AudioClip;
                        m_audioSource.Play();
                        FadeInVolume(1.0f);
                        break;
                    case "GameScenePortrait":
                        m_audioSource.clip = Resources.Load("Sound/BGM/water_bubble_01", typeof(AudioClip)) as AudioClip;
                        m_audioSource.loop = false;
                        /*
                        int random = Random.Range(0, 3);
                        if(random==0)
                            m_audioSource.clip = Resources.Load("Sound/african-huapangazo-sc1", typeof(AudioClip)) as AudioClip;
                        else if(random==1)
                            m_audioSource.clip = Resources.Load("Sound/sunny-day", typeof(AudioClip)) as AudioClip;
                        else if (random == 2)
                            m_audioSource.clip = Resources.Load("Sound/Nightlight-sc1", typeof(AudioClip)) as AudioClip;
                        */

                        // L’†‚ÍÄ¶‚µ‚È‚¢
                        if (!DataBase.bGameAdStop)
                        {
                            m_audioSource.Play();
                            FadeInVolume(0.5f);
                        }
                        break;

                    default:
                        break;
                }

            }
            else if (now_scene_name != last_scene_name)
            {
                switch (now_scene_name)
                {
                    case "HomeScenePortrait":
                        last_scene_name = now_scene_name;
                        break;
                    case "LessonScenePortrait":
                        if (last_scene_name == "GameScenePortrait")
                        {
                            FadeOut();
                        }
                        last_scene_name = now_scene_name;
                        break;
                    case "GameScenePortrait":
                        last_scene_name = now_scene_name;
                        Debug.Log("test");
                        FadeOut();
                        break;

                    default:
                        break;
                }
            }
            else if(now_scene_name == "GameScenePortrait")
            { // ‚»‚êˆÈŠO‚ÌƒV[ƒ“‚Å‚â‚è‚½‚¢ˆ—
                if(!m_audioSource.isPlaying)
                //if((m_audioSource.time + Time.deltaTime) > m_audioSource.clip.length &&m_audioSource.isPlaying )
                {
                    m_audioSource.Stop();
                    m_audioSource.clip = Resources.Load("Sound/BGM/water_bubble_01", typeof(AudioClip)) as AudioClip;
                    /*
                    int random = Random.Range(0, 3);
                    if (random == 0)
                        m_audioSource.clip = Resources.Load("Sound/african-huapangazo-sc1", typeof(AudioClip)) as AudioClip;
                    else if (random == 1)
                        m_audioSource.clip = Resources.Load("Sound/sunny-day", typeof(AudioClip)) as AudioClip;
                    else 
                        m_audioSource.clip = Resources.Load("Sound/Nightlight-sc1", typeof(AudioClip)) as AudioClip;
                    */

                    m_audioSource.Play();
                    FadeInVolume(0.5f);
                }
            }

        }

        public void FadeIn()
        {
            if (PlayerPrefs.GetInt("music_on") == 1)
            {
                StartCoroutine(FadeAudio(1.0f, Fade.In));
            }
        }

        public void FadeInVolume(float vol)
        {

            //bgm_vol = vol;
            if (PlayerPrefs.GetInt("music_on") == 1)
            {
                StartCoroutine(FadeAudioVolume(1.0f, Fade.In, vol));
            }
        }

        public void FadeOut()
        {
            if (PlayerPrefs.GetInt("music_on") == 1)
            {
                StartCoroutine(FadeAudio(1.0f, Fade.Out));
            }
        }
        public void FadeOutVolume()
        {
            if (PlayerPrefs.GetInt("music_on") == 1)
            {
                StartCoroutine(FadeAudioVolume(1.0f, Fade.Out,bgm_vol));
            }
        }

        private enum Fade
        {
            In,
            Out
        }

        private IEnumerator FadeAudio(float time, Fade fadeType)
        {
            var start = fadeType == Fade.In ? 0.0f : 1.0f;
            var end = fadeType == Fade.In ? 1.0f : 0.0f;
            var i = 0.0f;
            var step = 1.0f / time;

            while (i <= 1.0f)
            {
                i += step * Time.deltaTime;
                m_audioSource.volume = Mathf.Lerp(start, end, i);
                yield return new WaitForSeconds(step * Time.deltaTime);
            }
        }

        private IEnumerator FadeAudioVolume(float time, Fade fadeType,float vol)
        {
            var start = fadeType == Fade.In ? 0.0f : 1.0f;
            var end = fadeType == Fade.In ? 1.0f : 0.0f;
            var i = 0.0f;
            var step = 1.0f / time;

            while (i <= 1.0f)
            {
                i += step * Time.deltaTime;
                m_audioSource.volume = Mathf.Lerp(start, end, i) * vol;
                yield return new WaitForSeconds(step * Time.deltaTime);
            }

        }
    }
}