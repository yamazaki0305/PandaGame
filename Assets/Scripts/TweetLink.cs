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
            url = "https://twitter.com/share?url=https://yamazaki1.webnode.jp/nekotan/&text=%e8%8b%b1%e5%8d%98%e8%aa%9e%e3%83%91%e3%82%ba%e3%83%ab%20%23Nekotan%20%e3%81%a7%e9%81%8a%e3%82%93%e3%81%a0%e3%82%88%e2%99%aa";
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
