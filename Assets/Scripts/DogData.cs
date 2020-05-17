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
    UPPER = 3,
    WAIT = 4
}

public class DogData : BlockData
{
    public ArrowType arrowType;

    public bool iMove; //下左右に移動できるか
    public bool iStart; //ゲーム開始時の時true
    public bool bDogJimen; //true:未クリア、false:true
    public bool bRightMove, bLeftMove, bUnderMove;
    public int drop_count; //落ちたマスの数を記録

    // PuzzleMainから情報をSET
    private int BlockSize, rowLength, columnLength, DefaultBlockHeight, BlockGroundHeight;
    public int UnderArrowHeight;

    // 7かける7のX座標
    //public int X=4;

    // 7かける7のY座標
    //public int Y=4;

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
        drop_count = 0;
        blockType = BlockType.Dog;
    }

    void Update()
    {
        if (DataBase.DebugFlg)
        {
            Text text = GameObject.Find("DebugText").GetComponent<Text>();
            /*
            text.text = "Arrow:" + arrowType +" ";text.text += "iMove:" + iMove;
            text.text += " bLeftMove:" + bLeftMove + " "; text.text += "bRightMove:" + bRightMove; text.text += " bUnderMove:" + bUnderMove;
            text.text += "\n" + "X:" + X + " Y:" + Y;

            text.text += " drop:" + drop_count;
            */
        }
        
        if(iMove==false)
        {
            PuzzleMain main = GameObject.Find("GameRoot").GetComponent<PuzzleMain>();
            //Debug.Log("starcheck"+ main.StarDataList.Count);
            starCheck();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "star")
        {
            //other.GetComponent<BlockData>().TouchStar();
            /*
            switch (arrowType)
            {
                case ArrowType.LEFT:
                    arrowType = ArrowType.RIGHT;
                    break;
                case ArrowType.RIGHT:
                    arrowType = ArrowType.LEFT;
                    break;

            }
            */

        }

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

        //Debug.Log("height:" + UnderArrowHeight);

        Vector2 pos = new Vector2((X + 0) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + 0) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

        this.transform.localPosition = pos;
        iStart = false;

    }

    // 位置を取得
    public Vector2 getPos()
    {
        Vector2 pos = new Vector2(X, Y);
        return pos;
    }

    // 移動できるかチェック
    public void moveCheck(GameObject[,] _PuzzleData)
    {
        GameObject main = GameObject.Find("GameRoot");

        //ゲーム開始時以外の時
        if (!iStart)
        {
            // 犬が地面についたらクリア
            if (!bDogJimen && Y == UnderArrowHeight && main.GetComponent<PuzzleMain>().UnderArrowHeight == 0)
            {
                arrowType = ArrowType.WAIT;
                bDogJimen = true;
            }

            //左右下に移動できるか判定
            if (X == 0)
                bLeftMove = false;
            else if (_PuzzleData[X - 1, Y] == null || _PuzzleData[X - 1, Y].GetComponent<BlockData>().animalType == AnimalType.STAR)
                bLeftMove = true;
            else
                bLeftMove = false;

            if (X == columnLength - 1)
            {
                bRightMove = false;
            }
            else if (_PuzzleData[X + 1, Y] == null || _PuzzleData[X + 1, Y].GetComponent<BlockData>().animalType == AnimalType.STAR)
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
            // 犬が上下左右に移動中ではない時
            if (!iMove)
            {

                if (bUnderMove)
                {
                    arrowType = ArrowType.UNDER;

                    Vector2 pos = new Vector2((X + 0) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y - 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    OnStart(pos, 1);
                    
                    drop_count++;

                }
                // 下への移動が終了した時
                else if (!bUnderMove && arrowType == ArrowType.UNDER)
                {
                    Debug.Log("落ちた数;" + drop_count);


                    // 地面の下にブロックがある時の処理
                    int k = 0;
                    while (drop_count > 0)
                    {
                        if (main.GetComponent<PuzzleMain>().UndderArrowCheck())
                        {

                            arrowType = ArrowType.UPPER;

                            //startPosition = new Vector2(X * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + k) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);
                            //Vector2 pos = new Vector2(X * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + k+1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                            // 落ちる時
                            //犬を上に動かす
                            //OnUpper(pos, k);

                            drop_count--;

                        }
                        else
                            break;
                    }

                    _PuzzleData[X, Y] = null;
                    
                    drop_count = 0;

                    arrowType = ArrowType.WAIT;
                    /*
                    {
                        if (bLeftMove)
                            arrowType = ArrowType.LEFT;
                        else if (bRightMove)
                            arrowType = ArrowType.RIGHT;
                    }
                    */

                }

                if (arrowType == ArrowType.UPPER)
                {
                    if (!iMove)
                    {
                        arrowType = ArrowType.LEFT;
                    }
                }

                if (arrowType == ArrowType.RIGHT && bRightMove)
                {
                    Vector2 pos = new Vector2((X + 1) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + 0) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    OnStart(pos, 1);

                }
                else if (arrowType == ArrowType.RIGHT && !bRightMove)
                {
                    arrowType = ArrowType.LEFT;
                }

                if (arrowType == ArrowType.LEFT && bLeftMove)
                {
                    Vector2 pos = new Vector2((X - 1) * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (Y + 0) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

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

    // 犬の上下左右に☆がないかチェック
    public void starCheck()
    {
        PuzzleMain main = GameObject.Find("GameRoot").GetComponent<PuzzleMain>();
        bool bb = false;

        //starのList配列を探索
        int i;
        for(i=0; i<main.StarDataList.Count(); i++)
        {
            // 左をチェック
            if (main.StarDataList[i].GetComponent<BlockData>().X == this.X-1 && main.StarDataList[i].GetComponent<BlockData>().Y == this.Y)
            {
                bb = true;
                main.PuzzleData[this.X - 1, this.Y] = null;
                break;

            }
            // 右をチェック
            else if (main.StarDataList[i].GetComponent<BlockData>().X == this.X+1 && main.StarDataList[i].GetComponent<BlockData>().Y == this.Y)
            {
                bb = true;
                main.PuzzleData[this.X + 1, this.Y] = null;
                break;
            }
            //if　上下左右にstarがあったら、消す
            // 右をチェック
            else if (main.StarDataList[i].GetComponent<BlockData>().X == this.X && main.StarDataList[i].GetComponent<BlockData>().Y == this.Y-1)
            {
                bb = true;
                main.PuzzleData[this.X, this.Y-1] = null;
                break;
            }
        }

        if(bb)
        {
            AudioSource a1;
            AudioClip audio = Resources.Load("SOUND/SE/Button", typeof(AudioClip)) as AudioClip;
            a1 = gameObject.AddComponent<AudioSource>();
            a1.clip = audio;
            a1.Play();

            DataBase.bRescueStarOutputFlg = true;
            DataBase.RescueStarCount++;

            GameObject.Destroy(main.StarDataList[i]);
            main.StarDataList.RemoveAt(i);
            main.CheckBlockSpace();

        }

        // starを消すときは、消したあとの移動も行う

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

        //犬が落下中だけスピードを上げる
        if (arrowType == ArrowType.UNDER)
            duration = duration / 1.5f;

        startPosition = transform.localPosition;
        // この関数を呼び出すとオブジェクトが移動する
        StartCoroutine(MoveTo(startPosition, toPos, duration));
    }

    // ブロックが上がる時の落ちた位置の座標;toPos、何段上がるか:k
    public void OnUpper(Vector3 toPos, int k)
    {
        iMove = true;

        // 距離によってアニメーションの時間を変化
        float duration = 0f;
        float def = 0.4f;
        for (int i = 0; i < k; i++)
        {
            duration += def;
            def = def * 0.4f;
        }

        startPosition = transform.localPosition;
        // この関数を呼び出すとオブジェクトが移動する
        StartCoroutine(MoveTo(startPosition, toPos, duration));
    }

    // fromPosが移動元の座標、toPosが移動先の座標、durationが移動の秒数
    IEnumerator MoveTo(Vector3 fromPos, Vector3 toPos, float duration)
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
                    {
                        Y += -1;
                    }
                }

                iMove = false;
                //if(arrowType == ArrowType.UPPER)


                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
