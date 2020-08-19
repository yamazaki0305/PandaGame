using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweetLink : MonoBehaviour {

    public bool twitter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LinkBitton()
    {

        //https://greenspace.info/twitter/blogparts.html#tweet_button
        string url;
        if (twitter)
            url = "https://twitter.com/share?url=https://yamazaki1.webnode.jp/pandapuzzle/&text=%E3%83%91%E3%83%B3%E3%83%80%E3%81%A8%E7%8A%AC%E3%81%AE%E8%8B%B1%E5%8D%98%E8%AA%9E%E3%83%91%E3%82%BA%E3%83%AB%E3%81%A7%E3%80%81%E8%8B%B1%E8%AA%9E%E5%8B%89%E5%BC%B7%E4%B8%AD%E2%99%AA%0D%0A%23%E3%83%91%E3%83%B3%E3%83%80%E3%81%A8%E7%8A%AC%0D%0A%23PandaPuzzle";
        else
            url = "https://yamazaki1.webnode.jp/gameprivacypolicy/";

#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
    Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
    Application.OpenURL(url);
#endif
    }
}
