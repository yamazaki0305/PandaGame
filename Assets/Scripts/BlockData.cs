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

// ブロックのタイプを定義
public enum BlockType
{
    INVALID = -1,
    ALPHABET = 0,
    CRIMP,
    Animal = 4,
    SPACE
}

// 動物のタイプ
public enum AnimalType
{
    BIRD = 4,
    KAERU = 6,
    RABBIT = 8,
    NEKO1 = 4,
    NEKO2 = 6,
    NEKO3 = 8,
    NEKO4 = 10,
    NEKO5 = 12,
    NEKO6 = 14,
    NEKO7 = 16,
    NEKO8 = 18,
    NEKO9 = 20,
    NEKO10 = 22,
    NEKO11 = 24
}

public class BlockData : MonoBehaviour
{

    // 余白のサイズ
    private int margin = 5;

    //public Sprite[]BlockSprites;
    public Sprite[] Sprites;

    // 動物のTYPE
    public AnimalType animalType;

    // 並んでいるブロックのタイプを定義する。
    public BlockType blockType = BlockType.INVALID;

    // ブロックのアルファベットを格納する。
    public string Alphabet;

    // 選択されているか？
    public bool Selected;

    //英単語か？
    public bool EigoFlg;

    // 7かける7のX座標
    public int X;

    // 7かける7のY座標
    public int Y;

    // アルファ度
    public float alpha = 1f;
    public bool death = false;

    // 瞬き
    bool eyechange = false;
    DateTime datetimeStr;

    public void TapBlock()
    {
        if (this.Selected == false)
        {
            this.Selected = true;
            if (this.blockType == BlockType.ALPHABET)
                this.GetComponent<SpriteRenderer>().sprite = Sprites[1];
        }
        //this.GetComponentInChildren<TextMesh>().text = this.Alphabet;

    }
    public void ChangeBlock(bool selected,bool eigoflg )
    {
        
        if (selected == true && eigoflg == true)
        {
            this.Selected = selected;
            this.EigoFlg = eigoflg;
            this.GetComponent<SpriteRenderer>().sprite = Sprites[2];
        }
        else if (selected == true && eigoflg == false)
        {
            this.Selected = selected;
            this.EigoFlg = eigoflg;
            this.GetComponent<SpriteRenderer>().sprite = Sprites[1];
        }
        else if (selected == false && eigoflg == false)
        {
            this.Selected = selected;
            this.EigoFlg = eigoflg;
            this.GetComponent<SpriteRenderer>().sprite = Sprites[0];
        }

    }

    public void setup(BlockType type, string alphabet, bool selected, int x, int y)
    {
        this.blockType = type;
        this.Alphabet = alphabet;
        this.Selected = selected;
        this.EigoFlg = false;
        this.X = x;
        this.Y = y;

        if(this.blockType==BlockType.ALPHABET)
        {
            this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)BlockType.ALPHABET];
            this.GetComponentInChildren<TextMesh>().text = this.Alphabet;
        }
        else if(this.blockType==BlockType.Animal)
        {
            this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)AnimalType.BIRD];
            this.GetComponentInChildren<TextMesh>().text = "";
        }


    }

    public void setAnimalInt(int _NekoID)
    {
        AnimalType animal = (AnimalType)_NekoID;
        this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)animal];
        this.animalType = animal;

    }

    public void setAnimal(AnimalType type)
    {
        this.animalType = type;

        if ( type == AnimalType.BIRD)
            this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)AnimalType.BIRD];
        else if(type == AnimalType.KAERU)
            this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)AnimalType.KAERU];
        else if (type == AnimalType.RABBIT)
            this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)AnimalType.RABBIT];

    }

    void Update()
    {
        if (this.blockType == BlockType.Animal )
        {

            int rand = UnityEngine.Random.Range(0, 150);

            if (!eyechange)
            {
                if (rand == 0)
                {
                    eyechange = true;
                    this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)animalType + 1];
                    datetimeStr = System.DateTime.Now.AddMilliseconds(400);
                }

            }

            if (eyechange && System.DateTime.Now > datetimeStr)
            {
                this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)animalType];
                eyechange = false;
            }



            /*
            // 取得する値: 秒
            int datetimeStr = System.DateTime.Now.Second;
            int sec = datetimeStr;

            while (10 < sec)
            {
                sec = sec-10;
            }
            
            if (sec == 1 || sec == 2 || sec == 4 || sec == 5 || sec == 7 || sec == 8)
                this.GetComponent<SpriteRenderer>().sprite  = Sprites[(int)animalType];
            else if( sec ==3 || sec == 6 || sec == 9)
                this.GetComponent<SpriteRenderer>().sprite = Sprites[(int)animalType+1];
            */

        }

        //rend = GetComponent<SpriteRenderer>();
        //rend.sprite = block[1];
        //this.transform.position = new Vector3(0, 0,0);

    }


    /*
    public PuzzleBlock Generate()
    {
        //var blockType = Random.Range(0, prefabs.Count);

        //GameObject instance = GameObject.Instantiate(block[0]);
        //instance.transform.SetParent(transform);
        //instance.SetChainLine(chainLineGroup.Get());
        //instance.SetBlockType(blockType);
        //instance.transform.position = new Vector3(UnityEngine.Random.Range(-300, 300), 600);
       //gidbodyList.Add(instance.GetComponent<Rigidbody2D>());
        //return instance;
    }
    */
}
