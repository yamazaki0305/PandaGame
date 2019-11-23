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

    public bool iMove; //下左右に移動できるか
    public bool iStart; //ゲーム開始時の時true
    public bool bDogJimen; //true:未クリア、false:true
    public bool bRightMove, bLeftMove, bUnderMove;

    // PuzzleMainから情報をSET
    private int BlockSize, rowLength, columnLength, DefaultBlockHeight, BlockGroundHeight;

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
        iStart = true;
        bDogJimen = false;
    }

    // 初期位置をセット
    public void setPos(int _x, int _y, int _BlockSize, int _rowLength, int _columnLength, int _DefaultBlockHeight, int _BlockGroundHeight)
    {
        X = _x;
        Y = _y;
        BlockSize = _BlockSize;
        rowLength = _rowLength;
        columnLength = _columnLength;
        DefaultBlockHeight = _DefaultBlockHeight;
        BlockGroundHeight = _BlockGroundHeight;

        Vector2 pos = new Vector2((X + 0) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + 0) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

        this.transform.localPosition = pos;
        iStart = false;

    }

    // 移動できるかチェック
    public void moveCheck(GameObject[,] _PuzzleData)
    {
        //ゲーム開始時以外の時
        if (!iStart)
        {
            // 犬が地面についたらクリア
            if (!bDogJimen && Y == 0)
            {
                Debug.Log("犬地面についた");
                arrowType = ArrowType.WAIT;
                bDogJimen = true;
            }

            //左右下に移動できるか判定
            if (X == 0)
                bLeftMove = false;
            else if (_PuzzleData[X - 1, Y] == null)
                bLeftMove = true;
            else
                bLeftMove = false;

            if (X == columnLength - 1)
            {
                bRightMove = false;
            }
            else if (_PuzzleData[X + 1, Y] == null)
                bRightMove = true;
            else
                bRightMove = false;

            if (Y == 0)
                bUnderMove = false;
            else if (_PuzzleData[X, Y - 1] == null)
                bUnderMove = true;
            else
                bUnderMove = false;
        }

        // 犬のアニメーションを変更
        if (arrowType == ArrowType.UNDER)
            GetComponent<Animator>().SetTrigger("fallTrigger");
        else if (arrowType == ArrowType.LEFT)
        {
            //this.transform.localScale = new Vector3(-84, 84, 1);
            Debug.Log("xx:"+transform.localScale.x);
            this.transform.localScale = new Vector3(-84, transform.localScale.y, transform.localScale.z);
            GetComponent<Animator>().SetTrigger("walkTrigger");
        }
        else if (arrowType == ArrowType.RIGHT)
        {
            this.transform.localScale = new Vector3(84, transform.localScale.y, transform.localScale.z);
            GetComponent<Animator>().SetTrigger("walkTrigger");
        }
        else
            GetComponent<Animator>().SetTrigger("waitTrigger");


        // 犬が地面についていない時
        if (!bDogJimen)
        {
            // 犬が下左右に移動できる時の処理
            if (!iMove)
            {
                if (bUnderMove)
                {
                    arrowType = ArrowType.UNDER;

                    Vector2 pos = new Vector2((X + 0) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y - 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    //    Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (j + 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    OnStart(pos, 1);

                }
                else if (!bUnderMove && arrowType == ArrowType.UNDER)
                {
                    if (bLeftMove)
                        arrowType = ArrowType.LEFT;
                    else if (bRightMove)
                        arrowType = ArrowType.RIGHT;
 
                }


                if (arrowType == ArrowType.RIGHT && bRightMove)
                {
                    Vector2 pos = new Vector2((X + 1) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + 0) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    //    Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (j + 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    OnStart(pos, 1);

                }
                else if (arrowType == ArrowType.RIGHT && !bRightMove)
                {
                    arrowType = ArrowType.LEFT;
                }

                if (arrowType == ArrowType.LEFT && bLeftMove)
                {
                    Vector2 pos = new Vector2((X - 1) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + 0) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    //    Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (j + 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    OnStart(pos, 1);

                }
                else if (arrowType == ArrowType.LEFT && bRightMove)
                {
                    arrowType = ArrowType.RIGHT;
                }

                // 全て移動不可の時
                if(!bRightMove && !bLeftMove && !bUnderMove)
                {
                    arrowType = ArrowType.WAIT;
                }

                // ArrowType.Waitの時
                if(arrowType == ArrowType.WAIT)
                {
                    if (bUnderMove)
                        arrowType = ArrowType.UNDER;
                    else if (bLeftMove)
                        arrowType = ArrowType.LEFT;
                    else if (bRightMove)
                        arrowType = ArrowType.RIGHT;
                }

            }
        }


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
