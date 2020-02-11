using System;
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

public class ScoreLiner : MonoBehaviour
{
    static int SCORE = 0;
    static int RESCUE = 1;
    static int GAMECLEAR = 2;
    static int MOVES = 3;

    private Vector2 startPosition;
    Text ScoreText;
    public bool iMove = false;
    int score_kind;

    // スコアの移動
    public void UpperScore(int score)
    {
        iMove = true;
        score_kind = SCORE;

        // 距離によってアニメーションの時間を変化
        float duration = 2f;
        ScoreText = this.GetComponent<Text>();

        //pointを計算
        int point = 50;
        for (int i = 0; i < score; i++)
        {
            point = point + 50 * i;
        }

        ScoreText.text = "Letter " + score + " +" + point;
        startPosition = transform.localPosition;
        Vector3 toPos = new Vector3(startPosition.x + 0, startPosition.y + 400, 1);

        // この関数を呼び出すとオブジェクトが移動する
        StartCoroutine(MoveTo(startPosition, toPos, duration, false));

        // 拡大縮小
        //StartCoroutine(Scaling(120, 3));
    }

    // レスキュースコアの移動
    public void UpperRescue(int count)
    {
        iMove = true;
        score_kind = RESCUE;

        // 距離によってアニメーションの時間を変化
        float duration = 2f;
        ScoreText = this.GetComponent<Text>();

        int point = count * DataBase.STAR_TOUCH_SCORE; 

        ScoreText.text = "Star " + count + " +" + point;
        startPosition = transform.localPosition;
        Vector3 toPos = new Vector3(startPosition.x + 0, startPosition.y + 400, 1);

        // この関数を呼び出すとオブジェクトが移動する
        StartCoroutine(MoveTo(startPosition, toPos, duration, false));
    }

    public void GameClear()
    {
        score_kind = GAMECLEAR;
        ScoreText = this.GetComponent<Text>();
        StartCoroutine(TransparentHalfOut(4));
    }

    public void MovesScore(int count)
    {
        score_kind = MOVES;
        ScoreText = this.GetComponent<Text>();
        int point = count * 100;

        ScoreText.text = "Moves " + count + " +" + point;
        startPosition = transform.localPosition;

        StartCoroutine(TransparentHalfOut(2));
    }

    // 半分の時間アルファ100%で半分の時間を過ぎるとだんだん薄くなる
    IEnumerator TransparentHalfOut(float duration)
    {
        float time = 0;
        float maxtime = 0.95f;

        while (true)
        {
            time += (Time.deltaTime / duration);

            if (time > 1)
            {
                time = 1;

            }

            if (score_kind == GAMECLEAR && time > 0.5f)
            {
                if (!DataBase.bMovesOutputFlg && !DataBase.bMovesOutputNow)
                {
                    DataBase.bMovesOutputFlg = true;
                    DataBase.bMovesOutputNow = false;
                }
            }

            var color = ScoreText.color;
            // 現在のサイズ
             if (time < maxtime)
            {
                ScoreText.color = new Color(color.r, color.g, color.b, 1f);
            }
            // フェードアウト
            else
            {
                float alpha = (1 - time) / (1 - maxtime);
                color.a = alpha;
                ScoreText.color = new Color(color.r, color.g, color.b, alpha);

            }

            if (time == 1)
            {
                iMove = false;
                Destroy(this.gameObject);

                if (score_kind == GAMECLEAR)
                {
                    DataBase.bGameClearEnd = true;
                }
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Scaling(int maxSize, float duration)
    {
        float time = 0;
        float maxtime = 0.1f;

        while (true)
        {
            time += (Time.deltaTime / duration);

            if (time > 1)
            {
                time = 1;

            }

            var color = ScoreText.color;
            // 拡大
            if (time < maxtime)
            {
                float alpha = time;
                color.a = alpha;
                ScoreText.fontSize = (int)(maxSize * (time/maxtime) );
                ScoreText.color = new Color(color.r, color.g, color.b, alpha);

            }
            else if (maxtime <= time && time < 1-maxtime)
            {
                ScoreText.fontSize = maxSize;
                ScoreText.color = new Color(color.r, color.g, color.b, 1f);
            }
            // フェードアウト
            else
            {
                float alpha = (1 - time) / (1 - maxtime);
                color.a = alpha;
                //ScoreText.fontSize = (int)(maxSize * ((1-time)/maxtime ) );
                //ScoreText.fontSize = (int)(maxSize * (1+time -maxtime+((1-maxtime)-maxtime )) );
                ScoreText.color = new Color(color.r, color.g, color.b, alpha);
            }

            if (time == 1)
            {
                iMove = false;
                Destroy(this.gameObject);

                if(score_kind == GAMECLEAR)
                {
                    DataBase.bGameClearEnd = true;
                }
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // fromPosが移動元の座標、toPosが移動先の座標、durationが移動の秒数
    IEnumerator MoveTo(Vector3 fromPos, Vector3 toPos, float duration, bool drop)
    {
        float time = 0;

        while (true)
        {
            time += (Time.deltaTime / duration);

            if (time > 1)
            {
                time = 1;

            }

            //float easingValue = EasingLerps.EasingLerp(EasingLerps.EasingLerpsType.Bounce, EasingLerps.EasingInOutType.EaseOut, time, 0, 1);
            Vector3 lerpValue = Vector3.Lerp(fromPos, toPos, time);
            this.transform.localPosition = lerpValue;

            var color = ScoreText.color;
            // フェードイン
            if (time < 0.1f)
            {
                float alpha = time/0.1f;
                color.a = alpha;
                ScoreText.color = new Color(color.r, color.g, color.b, alpha);

            }
            else if( 0.1f<= time && time < 0.5f)
            {
                ScoreText.color = new Color(color.r, color.g, color.b, 1f);
            }
            // フェードアウト
            else
            {
                float alpha = (1 - time)*(1/0.5f);
                color.a = alpha;
                ScoreText.color = new Color(color.r, color.g, color.b, alpha);
            }

            if (time == 1)
            {
                iMove = false;
                Destroy(this.gameObject);

                if (score_kind == SCORE)
                    DataBase.bScoreOutputNow = false;
                else if (score_kind == RESCUE)
                    DataBase.bRescueStarOutputNow = false;

                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    

}