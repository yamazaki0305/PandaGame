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

public class nekoanime : MonoBehaviour
{

    public Sprite[] Sprites;

    // 瞬き
    bool eyechange = false;
    DateTime datetimeStr;

    public bool randomX = false;

    // Use this for initialization
    void Start()
    {

        if (randomX)
        {
            float randX = UnityEngine.Random.Range(-2.5f, 2.5f);
            Vector2 pos = this.transform.position;
            pos.x = randX;
            int randY = UnityEngine.Random.Range(20, 50);
            pos.y = randY;
            this.transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {

        int rand = UnityEngine.Random.Range(0, 150);

        if (!eyechange)
        {
            if (rand == 0)
            {
                eyechange = true;
                this.GetComponent<SpriteRenderer>().sprite = Sprites[1];
                datetimeStr = System.DateTime.Now.AddMilliseconds(400);
            }

        }

        if (eyechange && System.DateTime.Now > datetimeStr)
        {
            this.GetComponent<SpriteRenderer>().sprite = Sprites[0];
            eyechange = false;
        }

    }

    // 音声ファイルを設定
    //soundTap = Resources.Load("SOUND/SE/cursor1", typeof(AudioClip)) as AudioClip;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (randomX)
        {
            AudioSource a1;
            AudioClip audio = Resources.Load("SOUND/SE/Pretty_Kitten_Meow_1B_Synthesized_1648", typeof(AudioClip)) as AudioClip;
            a1 = gameObject.AddComponent<AudioSource>();
            a1.clip = audio;
            a1.Play();
            randomX = false;
        }
    }

}
