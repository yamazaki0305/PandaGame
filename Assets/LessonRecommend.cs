using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ricimi
{
    public class LessonRecommend : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void lessonRecommend()
        {
            DataBase.playLevel = 1;

            for (int i = 0; i < DataBase.MAXSTAGE; i++) {
                if(DataBase.level_star[i] < 3)
                {
                    DataBase.playLevel = i + 1;
                    break;
                }
            }

            DataBase.bGamePause = false;
            Color color = new Color(92, 156, 255);
            Transition.LoadLevel("GameScenePortrait", 1f, color);

        }
    }
}
