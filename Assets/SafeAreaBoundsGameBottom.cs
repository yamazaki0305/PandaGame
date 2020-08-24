using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaBoundsGameBottom : MonoBehaviour

{

    private RectTransform target;



#if UNITY_EDITOR

    public bool emulateIPhoneX = false;

#endif



    void Awake()

    {

        target = GetComponent<RectTransform>();

        ApplySafeArea();

    }



    void ApplySafeArea()

    {

        var area = Screen.safeArea;



#if UNITY_EDITOR

        if (Screen.width == 1125 && Screen.height == 2436 || Screen.width == 2436 && Screen.height == 1125)

        {

            emulateIPhoneX = true;

        }



        if (emulateIPhoneX)

        {

            Vector2 positionOffset;

            Vector2 sizeOffset;

            //縦持ち

            if (Screen.width * 2 <= Screen.height)
                this.transform.localPosition = new Vector2(0, -1060);

            //横持ち


        }

#endif


        if (Screen.width * 2 <= Screen.height)
            this.transform.localPosition = new Vector2(0,-1060);

            /*
            var anchorMin = area.position;

            var anchorMax = area.position + area.size;

            anchorMin.x /= Screen.width;

            anchorMin.y /= Screen.height;

            anchorMax.x /= Screen.width;

            anchorMax.y /= Screen.height;

            target.anchorMin = new Vector2(Mathf.Clamp(target.anchorMin.x, anchorMin.x, anchorMax.x), Mathf.Clamp(target.anchorMin.y, anchorMin.y, anchorMax.y));

            target.anchorMax = new Vector2(Mathf.Clamp(target.anchorMax.x, anchorMin.x, anchorMax.x), Mathf.Clamp(target.anchorMax.y, anchorMin.y, anchorMax.y));
            */
    }

}

