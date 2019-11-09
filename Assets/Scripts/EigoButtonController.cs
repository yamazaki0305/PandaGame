﻿using System;
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


public class EigoButtonController : MonoBehaviour
{

    //最小サイズ
    private float minimum = 1.0f;
    //拡大縮小スピード
    private float magSpeed = 5.0f;
    //拡大率
    private float magnification = 0.015f;

    //拡大縮小
    public bool scaling;

    // Use this for initialization
    void Start()
    {
        scaling = false;
    }

    public void OnClick()
    {
        Debug.Log("Click");

    }

    // Update is called once per frame
    void Update()
    {
        
        if (scaling)
        {
            float d = Mathf.Sin(Time.time * this.magSpeed);
            if (d < 0) d = -d;
            //ボタンを拡大縮小
            this.transform.localScale = new Vector2(this.minimum +d * this.magnification, this.minimum + d * this.magnification);
            //this.GetComponentInChildren<Text>();
        }
        else
        {
            this.transform.localScale = new Vector2(1, 1);
        }
        
    }
}