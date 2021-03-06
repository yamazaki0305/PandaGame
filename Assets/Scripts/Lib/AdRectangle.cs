﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdRectangle : MonoBehaviour
{

    public BannerView bannerView = null;

    public void AdBannerShow()
    {

#if UNITY_ANDROID
        string appId = "ca-app-pub-4228179100830730~6015219838"; //PandaAndroid Admob AppID
#elif UNITY_IPHONE
        string appId = "ca-app-pub-4228179100830730~7244541025";//PandaiPhone Admob AppID
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
    }

    public void RequestBanner()
    {
        DataBase.AdRectangleFlg = true;

        string adUnitId;
#if UNITY_ANDROID
        if (DataBase.AdRealTest)
            adUnitId = "ca-app-pub-4228179100830730/2954081841";  //正しい
        else
            adUnitId = "ca-app-pub-3940256099942544/6300978111";  //サンプル
#elif UNITY_IPHONE
        if (DataBase.AdRealTest)
            adUnitId = "ca-app-pub-4228179100830730/8944775121";  //正しい
        else
            adUnitId = "ca-app-pub-3940256099942544/2934735716";  //サンプル
#else
        adUnitId = "unexpected_platform";
#endif

        bannerView = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.Center);

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
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
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void AdBannerHide()
    {
        Debug.Log("banner_close");
        //bannerView.Hide();

#if UNITY_EDITOR

# else
        bannerView.Destroy();
#endif

        DataBase.AdRectangleFlg = false;

        //GameObject.Find("AdMob").GetComponent<AdBanner>().RequestBanner();
    }
}