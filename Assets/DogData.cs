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

// 犬の進行方向を定義
public enum ArrowType
{
    RIGHT = 0,
    LEFT = 1,
    UNDER = 2,
    WAIT = 3
}

public class DogData : MonoBehaviour
{
    public ArrowType arrowType;

    public bool iMove;
    public bool bRightMove, bLeftMove, bUnderMove;

    // 7かける7のX座標
    public int X=4;

    // 7かける7のY座標
    public int Y=4;

    Vector2 pos_now;
    Vector2 pos_future;

    Vector3 direction = new Vector3(100f, 500f, 0f);
    float speed = 5.0f;
    Vector3 startPosition;

    // Use this for initialization
    void Start () {
        arrowType = ArrowType.LEFT;

        bRightMove = false;
        bLeftMove = false;
        bUnderMove = false;
        iMove = false;

        
    }

    // ブロックが落ちる時の落ちた位置の座標;toPos、何段落ちるか:k
    public void OnStart(Vector3 toPos, int k)
    {
        iMove = true;

        // 距離によってアニメーションの時間を変化
        float duration = 0f;
        float def = 1.0f;
        for (int i = 0; i < k; i++)
        {
            duration += def;
            def = def * 0.4f;
        }

        startPosition = transform.localPosition;
        // この関数を呼び出すとオブジェクトが移動する
        StartCoroutine(MoveTo(startPosition, toPos, duration, true));
    }

    /*
    // Update is called once per frame
    void Update () {
        float step = speed * Time.deltaTime;
        //DogObject.transform.localPosition = Vector3.MoveTowards(DogObject.transform.localPosition, direction, step);

        if(iMove==false)
        {

            if(GameObject.Find("GameRoot").GetComponent<PuzzleMain>().PuzzleData[X+1, Y]==null)
            {
                Vector3 toPos;
                toPos.x = DogObject.transform.localPosition.x + 90;
                toPos.y = DogObject.transform.localPosition.y;
                toPos.z = DogObject.transform.localPosition.z;

                iMove = true;
                // この関数を呼び出すとオブジェクトが移動する
                StartCoroutine(MoveTo(DogObject.transform.localPosition, toPos, 3f,false));

            }

        }
        //Transform transform = GameObject.Find("PuzzleObjectGroup").transform;
        //DogObject.transform.SetParent(transform.parent);
        //DogObject.transform.localPosition = new Vector3(0, 0, 0);

        
    }
    */

    // fromPosが移動元の座標、toPosが移動先の座標、durationが移動の秒数
    IEnumerator MoveTo(Vector3 fromPos, Vector3 toPos, float duration,bool drop)
    {
        float time = 0;

        while (true)
        {
            time += (Time.deltaTime / duration);

            if (time > 1)
            {
                time = 1;
            }

            Vector3 lerpValue = Vector3.Lerp(fromPos, toPos, time);
            this.transform.localPosition = lerpValue;

            if (time == 1)
            {
                if (iMove)
                {
                    if (arrowType == ArrowType.LEFT)
                        X += -1;
                    else if (arrowType == ArrowType.RIGHT)
                        X += 1;
                    else if (arrowType == ArrowType.UNDER)
                        Y += -1;
                }

                iMove = false;

                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
