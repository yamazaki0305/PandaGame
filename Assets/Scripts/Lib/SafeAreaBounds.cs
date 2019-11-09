using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class SafeAreaBounds : MonoBehaviour

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

            if (Screen.width < Screen.height)

            {

                positionOffset = new Vector2(0f, area.size.y * 34f / 812f);

                sizeOffset = positionOffset + new Vector2(0f, area.size.y * 44f / 812f);

            }

            //横持ち

            else

            {

                positionOffset = new Vector2(area.size.x * 44f / 812f, area.size.y * 21f / 375f);

                sizeOffset = positionOffset + new Vector2(area.size.x * 44f / 812f, 0f);

            }

            area.position = area.position + positionOffset;

            area.size = area.size - positionOffset - sizeOffset;

        }

#endif



        var anchorMin = area.position;

        var anchorMax = area.position + area.size;

        anchorMin.x /= Screen.width;

        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;

        anchorMax.y /= Screen.height;

        target.anchorMin = new Vector2(Mathf.Clamp(target.anchorMin.x, anchorMin.x, anchorMax.x), Mathf.Clamp(target.anchorMin.y, anchorMin.y, anchorMax.y));

        target.anchorMax = new Vector2(Mathf.Clamp(target.anchorMax.x, anchorMin.x, anchorMax.x), Mathf.Clamp(target.anchorMax.y, anchorMin.y, anchorMax.y));

    }

}
