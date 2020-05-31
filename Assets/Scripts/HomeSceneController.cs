using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneController : MonoBehaviour
{
    int change_no = 0;
    public Text fukidashi_text;
    string [] panda_text = {"パズルゲームで\nえいごをおぼえるYo！",
                            "Learn English with puzzle games！",
                            "いぬかわいいYo！",
                            "Dog cute！",
                            "だんだんねむくなる\nゲームだから",
                            "ねむれないよるに\nオススメだYo！",
                            "It's a game that makes you sleepy,",
                            "Recommended for sleepless nights！",
    };

    float change_time;

    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
}
