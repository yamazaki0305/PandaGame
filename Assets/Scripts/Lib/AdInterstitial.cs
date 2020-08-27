using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdInterstitial : MonoBehaviour {

    bool IsClosed = false;
    private InterstitialAd interstitial;
    // 広告読み込むフラグ
    float AdFlgTime = 10f;

    float ResumeOpenTime = 5.0f;

    private void Start()
    {
        RequestInterstitial();
    }

    private void Update()
    {
        AdFlgTime -= Time.deltaTime;

        if (AdFlgTime < 0)
        {
            if (!this.interstitial.IsLoaded())
            {
                RequestInterstitial();
            }
            AdFlgTime = 10f;
        }

        if (IsClosed)
        {
            IsClosed = false;

            //ここで報酬を与える処理を実行

            // 広告を終了するとBGMをON
            DataBase.bGameAdStop = false;

            // 動画を終了したらDestroyする
            interstitial.Destroy();

            // 動画広告終了5秒間はResumeしない
            DataBase.AdResumeFlg = false;
            ResumeOpenTime = 5.0f;
        }

        // 動画広告終了直後はResumeしない処理
        if (DataBase.AdResumeFlg == false)
        {
            // 残り時間を計算する
            ResumeOpenTime -= Time.deltaTime;
            // ゼロ秒以下にならないようにする
            if (ResumeOpenTime <= 0.0f)
            {
                DataBase.AdResumeFlg = true;
            }
        }
    }

    private void RequestInterstitial()
    {
        string adUnitId;
#if UNITY_ANDROID
        if (DataBase.AdRealTest)
            adUnitId = "ca-app-pub-4228179100830730/2667851820"; //正しい
        else
            adUnitId = "ca-app-pub-3940256099942544/1033173712"; //サンプル
#elif UNITY_IPHONE
        if (DataBase.AdRealTest)
            adUnitId = "ca-app-pub-4228179100830730/1225927587"; //正しい
        else
            adUnitId = "ca-app-pub-3940256099942544/4411468910"; //サンプル
#else
            adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build(); //リリース時のAd設定
        //AdRequest request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build(); //テスト端末

        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        IsClosed = true;
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            // 広告中はBGMをOFF
            DataBase.bGameAdStop = true;

#if UNITY_EDITOR
            IsClosed = true;
            this.interstitial.Show();
            // 広告を終了するとBGMをON
            DataBase.bGameAdStop = false;
# else
            this.interstitial.Show();
#endif

        }
        // 広告の読み込み失敗時
        else
        {
            RequestInterstitial();
            MonoBehaviour.print("Interstitial is not ready yet");
            IsClosed = true;

            // 広告を終了するとBGMをON
            DataBase.bGameAdStop = false;
        }
    }
}
