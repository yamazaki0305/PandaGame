using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneController : MonoBehaviour
{
    int change_no = 0;
    public Text fukidashi_text;
    int[] panda_no = { 19, 2, 5, 1, 30, 29, 12, 7 };
    string [] panda_text = {"Dog cute！",
                            "いぬかわいいYo！",
                            "Learn English with puzzle games！",
                            "パズルゲームで\nえいごがまねべるYo！",
                            "It's a game that makes you sleepy,",
                            "Recommended for sleepless nights！",
                            "だんだんねむくなる\nゲームだから",
                            "ねむれないよるに\nオススメだYo！",
    };


    Image dogimg;
    float change_time;

    // Start is called before the first frame update
    void Start()
    {
        dogimg = GameObject.Find("PandaDogImg").GetComponent<Image>();
        dogimg.sprite = Resources.Load<Sprite>("LessonIMG/" + panda_no[0]);

        change_time = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        change_time -= Time.deltaTime;

        if(change_time < 0)
        {
            change_time = 3.5f;

            change_no++;
            if (change_no >= 8)
                change_no = 0;

            fukidashi_text.text = panda_text[change_no];
            dogimg.sprite = Resources.Load<Sprite>("LessonIMG/" + panda_no[change_no]);
        }
    }
}
