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
using System.Globalization;
using Ricimi;


// Macで書く
public class PuzzleMain : MonoBehaviour
{

    //英語ブロックのサイズを指定
    private int BlockSize = 85;
    private int MaskSize = 90;

    //英語ブロックの1段目の高さ
    private int BlockGroundHeight = -200;

    //草ラインの高さ
    private int GlassLineHeight = -240;
    private GameObject GlassLine;

    //株草画像の高さ
    private int ImgGroundHeight = -520;
    private GameObject ImgGround;


    //デフォルトの高さのマス
    private int DefaultBlockHeight = 7;

    //地面に到着して猫を消すマスの高さ
    public int DeathBlockHeight = 0;

    //プレイヤーがタップ出来るPuzzleDataのマスの高さ
    private int ActiveBlockHeight = 0;

    //UnderArrowの残りブロック数
    private int UnderArrowHeight = 0;

    // PuzzleDataをPuzzleObjectGroup下に表示する
    public Transform puzzleTransform;

    // Prefabをアタッチ
    public GameObject puzzlePrefab;
    public GameObject MaskPrefab;

    // UpdateScoreのGameObject
    public GameObject UpdateScoreObj;

    // 7列のパズルデータを作成。このパズルのデータでゲームを制御
    public GameObject[,] PuzzleData;

    // 7列のパズルデータのエリア内を作成（Mask）
    public GameObject[,] MaskData;

    // PuzzleDataやMaskの余白の高さ
    public int margin_height = 20;

    public string[,] stageData;

    public string[] textMessage; //テキストの加工前の一行を入れる変数
    public string[,] textWords; //テキストの複数列を入れる2次元は配列
    private int rowLength; //テキスト内の行数を取得する変数
    private int columnLength; //テキスト内の列数を取得する変数

    private int[] can_alphabet = new int[26]; //A-Zまでパズルエリアに何文字あるか格納する
    private GameObject CanWordText; // 何単語作れるかのテキストを表示

    //////////////////////////////////////////////////////////// 

    // SEを所得
    private AudioClip soundTap;
    private AudioClip soundCancel;
    private AudioSource audioSource;

    public GameObject EigoButton;

    //ヘッダーStatusのアタッチ
    public GameObject StatusCat;
    public GameObject StatusHand;
    public GameObject StatusScore;

    // ボタンに表示する英語（全て大文字）
    private string EigoText;
    // TransWindowに表示する英単語
    private string TransEigoText;
    // TransWindowに表示する英単語の翻訳
    private string TransJapText;
    // 消す英単語のSQL3のitem_id

    // EigoTextの状態を保持(NORMAL/PRESSED/EIGO)
    public ButtonFlg btnFlg;

    // ステージデータを格納
    public StageStatus StatusData;

    // ステージの残り時間を格納
    //https://qiita.com/nonkapibara/items/edb497f60e991bcbfe98
    public Text RemainTimerText; // UIText コンポーネント

    //ゲーム開始前の処理
    private GameObject StartWindow;

    //DBの定義
    SqliteDatabase sqlDB;

    //無視英単語リスト
    string[] ignore_word;

    // リストを作っている
    private List<GameObject> PuzzleDataList = new List<GameObject>();
    public List<GameObject> StarDataList = new List<GameObject>();

    // ゲームループフラグを管理
    GameLoopFlg GameFlg = GameLoopFlg.StartInfo;

    bool isRunning = true;

    // 画面に表示されないブロックの縦数を表示するGameObject
    private GameObject UnderArrow;

    private GameObject BackPicture;

    // GameOver/GameOver popup Prefab
    public GameObject popupWinPrefab;
    public GameObject popupLosePrefab;

    // CSVデータ
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    // TransWindowの表示有無
    public bool bTransWindowFlg = false;

    // Movesが0になった時一度だけAd動画視聴でMoveを増やせる
    public bool Moves0AdFlg = true;

    // DogObjectのゲームオブジェクト
    GameObject DogObject;

    void Start()
    {
        DogObject = GameObject.Find("Dog");
        DogObject.transform.SetParent(puzzleTransform);

        // 言語を判定する
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            DataBase.LangJapanese = true;
        }
        else
        {
            DataBase.LangJapanese = false;
        }
        

        //Scene名を格納
        DataBase.SceneName = SceneManager.GetActiveScene().name;

        // スコアの表示をリセット
        InitScoreOutput();

        // セーブデータをロードする
        SaveDataBase.loadData();

        // CSVReader
        csvDatas = CSVReader.import("CSV/StageListData");

        /// <summary>
        /// StageFileの読み込み
        string filename = "CSV/stage" + DataBase.playLevel.ToString();

        // ステージのクリア条件
        StatusData = new StageStatus(
            0,
            0,
            0,
            0,
            int.Parse(csvDatas[DataBase.playLevel][(int)CSVColumn.hand]),
            int.Parse(csvDatas[DataBase.playLevel][(int)CSVColumn.m_letter]),
            int.Parse(csvDatas[DataBase.playLevel][(int)CSVColumn.m_score]),
            int.Parse(csvDatas[DataBase.playLevel][(int)CSVColumn.timeflg]),
            int.Parse(csvDatas[DataBase.playLevel][(int)CSVColumn.time])
            );

        stageMaker(filename); // Status Updateより先にしないとバグる
        StatusData.StatusUpdate();

        ActiveBlockHeight = rowLength - DefaultBlockHeight;
        UnderArrowHeight = ActiveBlockHeight;
        DogObject.GetComponent<DogData>().UnderArrowHeight = UnderArrowHeight;

        // can_alphabetにパズルエリアのアルフェベットを格納
        CheckPotentialPuzzle();
        CanWordText = GameObject.Find("CanWordText");

        /////////////////

        // ステージLevelを表示
        GameObject.Find("LevelText").GetComponent<Text>().text = "レベル " + DataBase.playLevel;

        // ゲームモードがハードの時
        if (!DataBase.GameMode_Easy)
            GameObject.Find("CanWordText").SetActive(false);

        // ステージによってBackPicture・ImgGround・GlassLineを変更する


            // 画面に表示されない縦数を見つける
        UnderArrow = GameObject.Find("UnderArrow");

        // 猫救出ラインのGlassLineを見つける
        GlassLine = GameObject.Find("GlassLine");
        if (DataBase.playLevel < 15)
            GlassLine.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/green_glass");
        else if (15 <= DataBase.playLevel && DataBase.playLevel < 20)
            GlassLine.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/glass");
        else if (20 <= DataBase.playLevel && DataBase.playLevel < 29) 
            GlassLine.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/snow_glass");
        else if (29 <= DataBase.playLevel)
            GlassLine.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/cloud_glass");
        Vector3 pos = new Vector3(0, GlassLineHeight + UnderArrowHeight * -BlockSize, -1);
        GlassLine.transform.localPosition = pos;

        // 下部草画像を見つける
        ImgGround = GameObject.Find("ImgGround");
        
        if (DataBase.playLevel < 15)
            ImgGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/green_jimen");
        else if (15 <= DataBase.playLevel && DataBase.playLevel < 20)
            ImgGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/park_jimen4");
        else if (20 <= DataBase.playLevel && DataBase.playLevel < 29)
            ImgGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/snow_jimen");
        else if (29 <= DataBase.playLevel)
            ImgGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/cloud_jimen");

        pos = new Vector3(0, rowLength * BlockSize + ImgGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize - DefaultBlockHeight*BlockSize,-2);
        ImgGround.transform.localPosition = pos;

        BackPicture = GameObject.Find("BackPicture");
        if (DataBase.playLevel < 15)
            BackPicture.GetComponent<SpriteRenderer>().sprite =  Resources.Load<Sprite>("StageIMG/green_stage");
        else if (15 <= DataBase.playLevel && DataBase.playLevel < 20)
            BackPicture.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/park_sunset3");
        else if ( 20 <= DataBase.playLevel && DataBase.playLevel < 29)
            BackPicture.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/snow_stage");
        else if ( 29 <= DataBase.playLevel)
            BackPicture.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StageIMG/cloud_stage02");
        pos = new Vector3(0, GlassLineHeight + UnderArrowHeight * -BlockSize + 400, 100);
        BackPicture.transform.localPosition = pos;

        //無視英単語リストを設定する
        //　テキストファイルから読み込んだデータ
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("ignore_word", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        //ignore_word = TextLines.Split('\n'); //
        ignore_word = TextLines.Split(new char[] { '\r', '\n' });

        //DBの設定
        sqlDB = new SqliteDatabase("ejdict.sqlite3");

        //GameObjectを探して格納
        StartWindow = GameObject.Find("StartWindow");
        StartWindow.SetActive(true);
        Text StageText = GameObject.Find("StageText").GetComponent<Text>();

        if (DataBase.LangJapanese)
        {
            StageText.text = "Level " + DataBase.playLevel.ToString() + "\r\nパンダの犬を助けよう！";
            if (StatusData.timeflg)
                StageText.text += "\r\n制限時間 " + (StatusData.time - 1) + "秒";

            if (DataBase.playLevel == 1)
                StageText.text += "\r\n画面タップでスタート!";
        }
        else
        {
            StageText.text = "Level " + DataBase.playLevel.ToString() + "\r\nLet's rescue " + StatusData.AnimalSum.ToString() + " cats.";
            if (StatusData.timeflg)
                StageText.text += "\r\nTime limit " + (StatusData.time - 1) + " seconds";

            if (DataBase.playLevel == 1)
                StageText.text += "\r\nStart with screen tap!";
        }

        btnFlg = ButtonFlg.NORMAL;

        EigoText = "";
        TransJapText = "";
        DataBase.Word_item_id = 0;

        // 音声ファイルを設定
        soundTap = Resources.Load("SOUND/SE/cursor1", typeof(AudioClip)) as AudioClip;

        // 時間制のゲームの場合
        if (StatusData.timeflg)
        {
            // 残り時間を設定
            //currentTime = StatusData.time;
            RemainTimerFunction();
        }
        else
        {
            RemainTimerText.text = "";
        }

        // 犬の初期値をセット
        DogObject.GetComponent<DogData>();
        //DogMove();
    }



    // Update is called once per frame
    void Update()
    {
        
        // 広告中はゲームを止める
        if (DataBase.bGameAdStop)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        // 時間制のゲームの場合
        if (StatusData.timeflg)
        {
            // Timerをカウントダウンする
            if (GameFlg == GameLoopFlg.PlayNow)
                RemainTimerFunction();
        }

        // スコアポップアップの処理
        ScoreFunction();

        if (UnderArrowHeight == 0)
        {
            UnderArrow.SetActive(false);
        }
        else
        {
            // 画面に表示されない縦数を表示する
            UnderArrow.GetComponentInChildren<Text>().text = UnderArrowHeight.ToString();
        }

        // ゲーム開始前処理
        if (GameFlg == GameLoopFlg.StartInfo)
        {


            // スマホのタッチと、PCのクリック判定
            if (Input.GetMouseButtonUp(0))
            {

                StartWindow.SetActive(false);

                // プレイ中処理に移行
                GameFlg = GameLoopFlg.PlayNow;

                // level:1の時のみHowToPlayを表示
                if (DataBase.playLevel == 1)
                    GameObject.Find("HowToPlayButton").GetComponent<PopupOpener>().OpenPopup();
            }
            return;
        }
        // 和訳表示中処理
        else if (GameFlg == GameLoopFlg.Translate)
        {


            var button = EigoButton.GetComponent<Button>();


            StartCoroutine(BreakBlockCoroutine());

            if (isRunning == false)
            {

                //英語ボタンの文字を消す
                EigoText = "";
                EigoButton.GetComponentInChildren<Text>().text = EigoText;

                SelectEigoDestroy();

                btnFlg = ButtonFlg.NORMAL;

                // ブロック移動中処理に移行
                GameFlg = GameLoopFlg.BlockMove;
                isRunning = true;

                ButtonColorChange(button);

                //ブロックデータリストをクリア
                PuzzleDataList.Clear();

                return;
            }

        }
        // ブロック移動中処理
        else if (GameFlg == GameLoopFlg.BlockMove)
        {
            // 救出済ねこがいない時、移動中のブロックがない時
            if (CheckBlockMove() == false)
            {

                if (DeathCat() == false)
                {
                    //SelectEigoDestroy();
                    if (CheckBlockSpace() == false)
                        GameFlg = GameLoopFlg.UndderArrow;
                }

            }
        }
        // 地面の下にブロックがある時の処理
        else if (GameFlg == GameLoopFlg.UndderArrow)
        {
            /*
            while (UndderArrowCheck())
            {

            }
            */

            GameFlg = GameLoopFlg.PlayBefore;

        }
        // ゲーム中処理に戻る前の処理
        else if (GameFlg == GameLoopFlg.PlayBefore)
        {
            // can_alphabetにパズルエリアのアルフェベットを格納
            CheckPotentialPuzzle();

            GameFlg = GameLoopFlg.PlayNow;

            // UpdateScoreを非表示
            //UpdateScoreObj.SetActive(false);
        }
        else if (GameFlg == GameLoopFlg.PlayEnd)
        {
            
        }
        // ゲーム中処理
        else if (GameFlg == GameLoopFlg.PlayNow)
        {

            //ゲームクリア判定
            if (DogObject.GetComponent<DogData>().bDogJimen)
            {
                DataBase.bGameClearFlg = true;

                //GameOverObj.GetComponent<Text>().text = "GameClear!!\n次のステージへ";
                //GameOverObj.SetActive(true);
                //WinPopup();
                GameFlg = GameLoopFlg.PlayEnd;

            }
            /*
            if (StatusData.AnimalSum == 0)
            {
                DataBase.bGameClearFlg = true;

                //GameOverObj.GetComponent<Text>().text = "GameClear!!\n次のステージへ";
                //GameOverObj.SetActive(true);
                //WinPopup();
                GameFlg = GameLoopFlg.PlayEnd;

            }
            */
            //ゲームオーバー判定
            else if (StatusData.Hand == 0 || StatusData.currentTime <= 0.0f)
            {

                if (Moves0AdFlg)
                {
                    Moves0AdFlg = false;
                    DataBase.bGamePause = true;
                    this.GetComponent<PopupOpener2>().OpenPopup();
                }
                else if (!Moves0AdFlg && !DataBase.bGamePause)
                {
                    // インステ広告を確率で表示
                    //RandomAd.ShowInterstitial();

                    LosePopup();
                    GameFlg = GameLoopFlg.PlayEnd;
                }

            }
            // スマホのタッチと、PCのクリック判定
            if (Input.GetMouseButtonDown(0) && !bTransWindowFlg && !DataBase.bGamePause )
            {

                Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D collition2d = Physics2D.OverlapPoint(point);

                BlockData blockData;

                // ここでRayが当たったGameObjectを取得できる
                if (collition2d)
                {
                    if (collition2d.tag == "Block" )
                    {
                        blockData = collition2d.GetComponent<BlockData>();

                        if (blockData.blockType == BlockType.ALPHABET)
                        {
                            if (!blockData.Selected)
                            {
                                // プレイヤーがタップできるPuzzleDataのマスの高さ
                                if (blockData.Y >= ActiveBlockHeight)
                                {
                                    //audioSource = this.GetComponent<AudioSource>();
                                    //audioSource.clip = soundTap;
                                    //audioSource.Play();
                                    blockData.GetComponent<AudioSource>().Play();

                                    PuzzleDataList.Add(collition2d.gameObject);

                                    blockData.TapBlock();

                                    // ここでRayが当たったGameObjectを取得できる
                                    EigoText += blockData.Alphabet;
                                    EigoButton.GetComponentInChildren<Text>().text = EigoText;

                                    //英単語になったかの判定
                                    bool judge = EigoJudgement();

                                    //英単語になった時
                                    if (judge)
                                    {
                                        btnFlg = ButtonFlg.EIGO;
                                        SelectEigoChange();


                                    }
                                    //英単語ではない
                                    else
                                    {
                                        btnFlg = ButtonFlg.PRESSED;
                                    }
                                    var button = EigoButton.GetComponent<Button>();
                                    ButtonColorChange(button);
                                }

                            }
                            else
                            {
                                int x = PuzzleDataList[PuzzleDataList.Count - 1].GetComponent<BlockData>().X;
                                int y = PuzzleDataList[PuzzleDataList.Count - 1].GetComponent<BlockData>().Y;

                                // 最後にタップしたブロックの選択を解除
                                if (blockData.X == x && blockData.Y == y)
                                {
                                    blockData.GetComponent<AudioSource>().Play();

                                    PuzzleDataList[PuzzleDataList.Count - 1].GetComponent<BlockData>().ChangeBlock(false, false);
                                    PuzzleDataList.RemoveRange(PuzzleDataList.Count - 1, 1);

                                    //末尾から1文字削除する
                                    EigoText = EigoText.Remove(EigoText.Length - 1, 1);
                                    EigoButton.GetComponentInChildren<Text>().text = EigoText;
                                    EigoJudgement();



                                }
                            }
                        }
                    }
                }
            }

        }

        // 犬の移動処理（Updateで実行）
        DogObject.GetComponent<DogData>().moveCheck(PuzzleData);

    }

    // Timerを動かす
    void RemainTimerFunction()
    {
        // 残り時間を計算する
        StatusData.currentTime -= Time.deltaTime;

        // ゼロ秒以下にならないようにする
        if (StatusData.currentTime <= 0.0f)
        {
            StatusData.currentTime = 0.0f;
        }
        int minutes = Mathf.FloorToInt(StatusData.currentTime / 60F);
        int seconds = Mathf.FloorToInt(StatusData.currentTime - minutes * 60);
        int mseconds = Mathf.FloorToInt((StatusData.currentTime - minutes * 60 - seconds) * 1000);
        //RemainTimerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, mseconds);
        RemainTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ScoreFunction()
    {
        if (DataBase.bScoreOutputFlg)
        {
            this.GetComponent<ScorePopup>().ScoreOutput(DataBase.score_count);

            int s = 50;

            for (int i = 0; i < DataBase.score_count; i++)
            {
                s = s + 50 * i;
            }
            StatusData.ScoreUpdate(s);
            StatusData.HandUpdate();
            StatusData.StarCheck(EigoText);

            DataBase.score_count = 0;
            DataBase.bScoreOutputFlg = false;

            //スコアやスターリワードをチェック
        }
        // スターに接触した時のフラグ
        if (DataBase.bRescueStarOutputFlg)
        {
            this.GetComponent<ScorePopup>().RescueOutput(DataBase.RescueStarCount);

            StatusData.ScoreUpdate(DataBase.STAR_TOUCH_SCORE * DataBase.RescueStarCount);
            StatusData.StarCheck(EigoText);

            DataBase.RescueStarCount = 0;
            DataBase.bRescueStarOutputFlg = false;

            AudioSource a1;
            AudioClip audio = Resources.Load("SOUND/SE/Button", typeof(AudioClip)) as AudioClip;
            a1 = gameObject.AddComponent<AudioSource>();
            a1.clip = audio;
            a1.Play();

        }
        if (DataBase.bGameClearFlg && !DataBase.bScoreOutputNow && !DataBase.bRescueStarOutputNow )
        {
            this.GetComponent<ScorePopup>().GameClearOutput();
            StatusData.StarClearAdd();

            DataBase.bGameClearFlg = false;
            AudioSource a1;
            AudioClip audio = Resources.Load("SOUND/SE/Magical", typeof(AudioClip)) as AudioClip;
            a1 = gameObject.AddComponent<AudioSource>();
            a1.clip = audio;
            a1.Play();

        }
        if(DataBase.bMovesOutputFlg)
        {
            if (StatusData.Hand > 0)
            {
                this.GetComponent<ScorePopup>().MovesOutput(StatusData.Hand);

                StatusData.ScoreUpdate(100 * StatusData.Hand);
                StatusData.StarCheck("");

                DataBase.bMovesOutputFlg = false;
                DataBase.bMovesOutputNow = true;

                AudioSource a1;
                AudioClip audio = Resources.Load("SOUND/SE/Button", typeof(AudioClip)) as AudioClip;
                a1 = gameObject.AddComponent<AudioSource>();
                a1.clip = audio;
                a1.Play();
            }
            else
            {
                DataBase.bMovesOutputFlg = false;
                DataBase.bMovesOutputNow = true;
            }

        }
        // ゲームクリア時のスコア表示の処理
        if(DataBase.bGameClearEnd)
        {

            //StatusをDataBaseをコピーする
            if (DataBase.BestScore < StatusData.Score)
                DataBase.BestScore = StatusData.Score;

            // 獲得した☆を保存
            if (DataBase.level_star[DataBase.playLevel - 1] < StatusData.star)
                DataBase.level_star[DataBase.playLevel - 1] = StatusData.star;

            SaveDataBase.saveData();
            DataBase.bGameClearEnd = false;

            /*
            RandomAd.ShowInterstitial();

            // 広告表示以外のみ遷移する
            if(DataBase.bGameAdStop == false)
                WinPopup();
            else
                DataBase.bGameClearAdEnd = true;
            */
            WinPopup();

        }
        // ゲームクリア時のAdを閉じた時
        if (DataBase.bGameClearAdEnd && !DataBase.bGameAdStop)
        {

            DataBase.bGameClearAdEnd = false;
            WinPopup();

        }

    }

    public void InitScoreOutput()
    {
        DataBase.bScoreOutputFlg = false;
        DataBase.bScoreOutputNow = false;
        DataBase.score_count = 0;
        DataBase.bRescueStarOutputFlg = false;
        DataBase.bRescueStarOutputNow = false;
        DataBase.RescueStarCount = 0;
        DataBase.bMovesOutputFlg = false;
        DataBase.bMovesOutputNow = false;
        DataBase.bGameClearFlg = false;
        DataBase.bGameClearEnd = false;
        DataBase.bGameClearAdEnd = false;
        DataBase.bGamePause = false;
        DataBase.bGameAdStop = false;
        DataBase.WordIDList.Clear();
        DataBase.WordDataList.Clear();
        DataBase.WordIDAddList.Clear();
        DataBase.WordStrAddList.Clear();
        DataBase.newword_count = 0;
        DataBase.BestScore = 0;
        DataBase.Word_item_id = 0;
        DataBase.Word_new_flg = false;

}

    IEnumerator BreakBlockCoroutine()
    {

        for (int i = 0; i < PuzzleDataList.Count(); i++)
        {

            PuzzleDataList[i].GetComponent<SpriteRenderer>().sprite = null;
            PuzzleDataList[i].GetComponentInChildren<TextMesh>().GetComponent<EigoWordController>().breakFlg = true;
            yield return new WaitForSeconds(0.2f);

        }

        for (int i = 0; i < PuzzleDataList.Count(); i++)
        {
            PuzzleDataList[i].SetActive(false);
            //PuzzleDataList[i].GetComponentInChildren<Text>().fontSize = 80;
            //yield return new WaitForSeconds(0.25f);
        }
        isRunning = false;
    }


    bool EigoJudgement()
    {
        // 英単語が作れるかチェック
        //CheckPotentialWords(EigoText);

        //英単語になったかの判定(2文字以上の時)
        bool judge = false;

        //英単語になったときの単語
        string eigoword = "temp";

        if (EigoText.Length >= 2)
        {

            //すべて小文字の時
            if (judge == false)
            {
                string eigo = EigoText.ToLowerInvariant();

                string query = "select item_id,word,mean from items where word ='" + eigo + "'";
                DataTable dataTable = sqlDB.ExecuteQuery(query);

                TransJapText = "";
                DataBase.Word_item_id = 0;

                foreach (DataRow dr in dataTable.Rows)
                {
                    judge = true;

                    string id = dr["item_id"].ToString();
                    string word = (string)dr["word"];
                    string str = (string)dr["mean"];

                    eigoword = word;

                    TransJapText += str;
                    DataBase.Word_item_id = Int32.Parse(id);
                }
            }

            //1文字目を大文字で検索
            if (judge == false)
            {
                //全て小文字にする
                string inStr = EigoText.ToLowerInvariant();

                //1文字目を大文字で検索
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                string outStr = ti.ToTitleCase(inStr);

                string  query = "select item_id,word,mean from items where word ='" + outStr + "'";
                DataTable dataTable = sqlDB.ExecuteQuery(query);
                TransJapText = "";
                DataBase.Word_item_id = 0;

                foreach (DataRow dr in dataTable.Rows)
                {
                    judge = true;
                    string id = dr["item_id"].ToString();
                    string word = (string)dr["word"];
                    string str = (string)dr["mean"];

                    eigoword = word;

                    TransJapText += str;
                    DataBase.Word_item_id = Int32.Parse(id);

                }
            }
            //全て大文字にする
            /*
            if (judge == false)
            {
                //全て大文字にする
                string inStr = EigoText.ToUpperInvariant();
                
                string query = "select word,mean from items where word ='" + inStr + "'";
                DataTable  dataTable = sqlDB.ExecuteQuery(query);
                TransJapText = "";
                foreach (DataRow dr in dataTable.Rows)
                {
                    judge = true;
                    string word = (string)dr["word"];
                    string str = (string)dr["mean"];

                    eigoword = word;

                    TransJapText += str;

                }
            }
            */

            if(judge)
            {
                //除外英単語かどうか調べる
                //すべて小文字の時
                string tempStr = EigoText.ToLowerInvariant();
                for (int i = 0; i < ignore_word.Length; i++)
                {
                    //Debug.Log("英単語:" + eigoword + "無視:" + ignore_word[i]);
                    if (ignore_word[i] == tempStr)
                    {
                        judge = false;
                    }
                }
                //1文字目が大文字の時
                //TextInfo tinfo = CultureInfo.CurrentCulture.TextInfo;
                //tempStr = tinfo.ToTitleCase(EigoText);
                tempStr = EigoText.ToLowerInvariant();
                tempStr = char.ToUpper(EigoText[0]) + tempStr.Substring(1);

                for (int i = 0; i < ignore_word.Length; i++)
                {

                    if (ignore_word[i] == tempStr)
                    {
                        judge = false;
                    }
                }
                //すべて大文字の時
                tempStr = EigoText.ToUpperInvariant();
                for (int i = 0; i < ignore_word.Length; i++)
                {
                    //Debug.Log("英単語:" + eigoword + "無視:" + ignore_word[i]);
                    if (ignore_word[i] == tempStr)
                    {
                        judge = false;
                    }
                }
            }
        }


        //英単語になった時にボタンの色を変える
        if (judge)
        {
            btnFlg = ButtonFlg.EIGO;
            for (int i = 0; i < PuzzleDataList.Count; i++)
            {
                PuzzleDataList[i].GetComponent<BlockData>().ChangeBlock(true, true);
            }
            SelectEigoChange();
            TransEigoText = eigoword;

            for (int i = 0; i < PuzzleDataList.Count(); i++)
            {
                PuzzleDataList[i].GetComponentInChildren<TextMesh>().GetComponent<EigoWordController>().scalingFlg = true;
            }
        }
        else if (EigoText.Length == 0)
        {

            btnFlg = ButtonFlg.NORMAL;

        }
        //英単語ではない
        else
        {
            TransJapText = "";
            DataBase.Word_item_id = 0;

            btnFlg = ButtonFlg.PRESSED;
            for (int i = 0; i < PuzzleDataList.Count; i++)
            {
                PuzzleDataList[i].GetComponent<BlockData>().ChangeBlock(true, false);
            }

            for (int i = 0; i < PuzzleDataList.Count(); i++)
            {
                PuzzleDataList[i].GetComponentInChildren<TextMesh>().GetComponent<EigoWordController>().scalingFlg = false;
            }
        }
        var button = EigoButton.GetComponent<Button>();
        ButtonColorChange(button);


        return judge;
    }

    public void ReloadButton()
    {
        
        // 現在読み込んでいるシーンのインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 取得したシーンインデックスで再読込み
        SceneManager.LoadScene(currentSceneIndex);
        //SceneManager.LoadScene("PuzzleGame");
    }

    public void NextButton()
    {
        DataBase.playLevel++;
        if (DataBase.playLevel > 11)
            DataBase.playLevel = 1;

        // 現在読み込んでいるシーンのインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 取得したシーンインデックスで再読込み
        SceneManager.LoadScene(currentSceneIndex);
        //SceneManager.LoadScene("PuzzleGame");
    }
    public void PressEigoButton()
    {
        var button = EigoButton.GetComponent<Button>();
        //CanWordText.GetComponent<Text>().text = "";

        if (btnFlg == ButtonFlg.PRESSED)
        {
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = soundCancel;
            audioSource.Play();

            EigoText = "";
            EigoButton.GetComponentInChildren<Text>().text = EigoText;
            SelectAllCanceled();
            btnFlg = ButtonFlg.NORMAL;

            ButtonColorChange(button);

            //ブロックデータリストをクリア
            PuzzleDataList.Clear();

        }
        else if (btnFlg == ButtonFlg.EIGO)
        {
            
            AudioSource a1;
            AudioClip audio = Resources.Load("SOUND/SE/Harp", typeof(AudioClip)) as AudioClip;
            a1 = gameObject.AddComponent<AudioSource>();
            a1.clip = audio;
            a1.Play();

            // 和訳表示処理に移行
            GameFlg = GameLoopFlg.Translate;

            this.GetComponent<PopupOpener>().OpenWindow();
            bTransWindowFlg = true;

            // 作成済英単語か調べる
            bool newword = true;
            for(int i=0;i<DataBase.WordIDList.Count();i++)
            {
                if (DataBase.WordIDList[i] == DataBase.Word_item_id)
                    newword = false;
            }
            for (int i = 0; i < DataBase.WordIDAddList.Count(); i++)
            {
                if (DataBase.WordIDAddList[i] == DataBase.Word_item_id)
                    newword = false;
            }
            if (newword)
                DataBase.Word_new_flg = true;
            else
                DataBase.Word_new_flg = false;

            // 作成した英単語IDを追加
            DataBase.WordIDAddList.Add(DataBase.Word_item_id);
            DataBase.WordStrAddList.Add(TransEigoText);

            /*
            if (SaveDataBase.saveWord(Word_item_id, TransEigoText))
            {
                GameObject.Find("NewIcon").SetActive(true);
            }
            else
                GameObject.Find("NewIcon").SetActive(false);
            */

            // UpdateScoreを表示
            DataBase.bScoreOutputFlg = true;
            DataBase.bScoreOutputNow = true;

            DataBase.score_count = EigoText.Length;
            //UpdateScoreObj.SetActive(true);
        }

    }

    // 手数を消費してアルファベットブロックを落とす
    public void DropBlockButton()
    {
        bool search = true;

        // 手数が0以上の時のみ実行可能
        if ( StatusData.Hand > 0)
        {
            // Maskと英語ブロックを参照して空きスペースを探す
            for (int j = ActiveBlockHeight - UnderArrowHeight; j < rowLength; j++)
            {
                // Maskと英語ブロックを参照して空きスペースを探す
                for (int i = 0; i < columnLength; i++)
                {
                    //PuzzleDataが空白の時
                    if (PuzzleData[i, j] == null && MaskData[i, j] != null)
                    {

                        search = false;

                        // パズルを落とす位置をセット
                        //Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, 1500);
                        Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, rowLength * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                        // スクリプトからインスタンス（動的にゲームオブジェクトを指定数だけ作る
                        PuzzleData[i, j] = Instantiate(puzzlePrefab, pos, Quaternion.identity);

                        // アルファベットブロックにする
                        PuzzleData[i, j].GetComponent<BlockData>().setup(BlockType.ALPHABET, RandomMake.alphabet(), false, i, j);
                        PuzzleData[i, j].name = "Block"; // GameObjectの名前を決めている
                        PuzzleData[i, j].transform.SetParent(puzzleTransform);
                        PuzzleData[i, j].transform.localPosition = pos;

                        //アルフェベットブロックの位置をセット
                        pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, j * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);
                        PuzzleData[i, j].GetComponent<Liner>().OnUpper(pos, rowLength - j + 1);
                        //PuzzleData[i, j].GetComponent<Liner>().OnStart(pos, k);
                        //PuzzleData[i, j].transform.position = pos;
                        //PuzzleData[i, j].transform.localScale = puzzlePrefab.transform.localScale;

                        //アルファベットブロックを落としたので手数を１つ減らす
                        StatusData.Hand--;
                        StatusData.StatusUpdate();

                        //パズルエリアの英語ブロック数を更新
                        CheckPotentialPuzzle();

                    }

                    if (!search)
                        break;
                }
                if (!search)
                    break;
            }
        }


        // ActiveBlockHeightから上DefaultBlockHeight(7段)までPuzzleData[,]を参照して空き配列を見る
        // 空き配列にMaskData[,]を確認しMaskが存在する
        //アルファベットブロックを落とす

        //空きスペースがない returnする
        return;
        //

    }

    void ButtonColorChange(Button button)
    {
        if (btnFlg == ButtonFlg.PRESSED)
        {
            var colors = button.colors;
            colors.normalColor = new Color(255f / 255f, 51f / 255f, 153f / 255f, 255f / 255f);
            colors.highlightedColor = new Color(255f / 255f, 51f / 255f, 153f / 255f, 255f / 255f);
            colors.pressedColor = new Color(204f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
            colors.disabledColor = new Color(204f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);

            EigoButton.GetComponent<Button>().colors = colors;

            EigoButton.GetComponent<EigoButtonController>().scaling = false;
        }
        else if (btnFlg == ButtonFlg.NORMAL)
        {
            var colors = button.colors;
            colors.normalColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            colors.highlightedColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            colors.pressedColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 255f / 255f);
            colors.disabledColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 255f / 255f);

            EigoButton.GetComponent<Button>().colors = colors;

            EigoButton.GetComponent<EigoButtonController>().scaling = false;
        }
        else if (btnFlg == ButtonFlg.EIGO)
        {
            var colors = button.colors;
            colors.normalColor = new Color(255f / 255f, 255f / 255f, 153f / 255f, 255f / 255f);
            colors.highlightedColor = new Color(255f / 255f, 255f / 255f, 153f / 255f, 255f / 255f);
            colors.pressedColor = new Color(255f / 255f, 255f / 255f, 153f / 255f, 255f / 255f);
            colors.disabledColor = new Color(255f / 255f, 255f / 255f, 153f / 255f, 255f / 255f);

            EigoButton.GetComponent<Button>().colors = colors;

            EigoButton.GetComponent<EigoButtonController>().scaling = true;
            isRunning = true;

        }
    }

    /// <summary>
    /// PuzzleObjectGroupからコピー
    /// </summary>

    public bool DeathCat()
    {
        bool b = false;
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                //空白の時
                if (PuzzleData[i, j])
                {
                    if (PuzzleData[i, j].GetComponent<BlockData>().blockType == BlockType.Animal)
                    {
                        if (PuzzleData[i, j].GetComponent<BlockData>().death && PuzzleData[i, j].GetComponent<Liner>().iMove == false)
                        {
                            //消せる猫がいる時はreturn trueにする
                            b = true;

                            // 救出処理開始時
                            if (PuzzleData[i, j].GetComponent<BlockData>().alpha == 1.0f)
                            {
                                // スコア表示
                                DataBase.bRescueStarOutputFlg = true;
                                DataBase.bRescueStarOutputNow = true;
                                DataBase.RescueStarCount++;
                            }

                            PuzzleData[i, j].GetComponent<BlockData>().alpha -= Time.deltaTime;
                            var color = PuzzleData[i, j].GetComponent<SpriteRenderer>().color;
                            color.a = PuzzleData[i, j].GetComponent<BlockData>().alpha;
                            PuzzleData[i, j].GetComponent<SpriteRenderer>().color = color;

                            if (PuzzleData[i, j].GetComponent<BlockData>().alpha < 0)
                            {
                                GameObject obj = GameObject.Find("GameRoot");

                                obj.GetComponent<PuzzleMain>().StatusData.AnimalUpdate(PuzzleData[i, j].GetComponent<BlockData>().animalType);

                                // 残り時間が無くなったら自分自身を消滅
                                GameObject.Destroy(PuzzleData[i, j]);

                            }
                        }

                    }
                }

            }
        }
        return b;
    }


    //現在選択中のブロックを全てキャンセル
    public void SelectAllCanceled()
    {
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                if (PuzzleData[i, j] != null)
                {
                    if (PuzzleData[i, j].GetComponent<BlockData>().Selected)
                    {
                        PuzzleData[i, j].GetComponent<BlockData>().ChangeBlock(false, false);
                        Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, j * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);
                        PuzzleData[i, j].transform.SetParent(puzzleTransform);
                        //PuzzleData[i, j].transform.position = pos;
                    }

                }

            }
        }
    }

    //現在選択中のブロックを英単語にする
    public void SelectEigoChange()
    {
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                if (PuzzleData[i, j] != null)
                {
                    if (PuzzleData[i, j].GetComponent<BlockData>().Selected)
                    {

                        PuzzleData[i, j].GetComponent<BlockData>().ChangeBlock(true, true);
                        Vector2 pos = new Vector2(i * BlockSize - 320 + 45, j * BlockSize - 270);
                        PuzzleData[i, j].transform.SetParent(puzzleTransform);
                        //PuzzleData[i, j].transform.position = pos;
                    }

                }

            }
        }
    }

    //PuzzleDataの空白を探す
    public bool CheckBlockSpace()
    {

        bool b=false;

        //PuzzleDataの空白を探す
        for (int i = 0; i<columnLength; i++)
        {
            for (int j = ActiveBlockHeight - UnderArrowHeight; j<rowLength+margin_height; j++)
            {
                //PuzzleDataが空白の時
                if (PuzzleData[i, j] == null && MaskData[i, j] != null)
                {

                    //空白PuzzleDataのブロックの上にブロックがないかrowLengthまで調べる
                    for (int k = 1; j + k<rowLength+margin_height; k++)
                    {

                        /*
                        // 空白PuzzleDataの上に障害ブロックが見つかった時
                        if (PuzzleData[i, j + k].GetComponent<BlockData>().blockType == BlockType.CRIMP)
                        {
                            //break;
                        }
                        */
                        //もしNULL以外のPuzzleDataのブロックが見つかった時
                        if (PuzzleData[i, j + k] != null)
                        {
                            // 空白PuzzleDataの上に障害ブロックが見つかった時
                            if (PuzzleData[i, j + k].GetComponent<BlockData>().blockType == BlockType.CRIMP)
                            {
                                break;
                            }

                            //PuzzleDataのX,Yのマスの位置を現在→新しい位置に更新
                            PuzzleData[i, j + k].GetComponent<BlockData>().X = i;
                            PuzzleData[i, j + k].GetComponent<BlockData>().Y = j;

                            //空白PuzzleDataの空白に見つかったPuzzleDataのブロックを代入
                            PuzzleData[i, j] = PuzzleData[i, j + k];

                            //空白のPuzzleDataに代入したので代入元のデータをnullにする
                            PuzzleData[i, j + k] = null;

                            //PuzzleDataのブロックの表示座標を更新する
                            //Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, j * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize + (ActiveBlockHeight - UnderArrowHeight) * BlockSize);
                            Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, j * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);
                            PuzzleData[i, j].transform.SetParent(puzzleTransform);

                            //PuzzleData[i, j].GetComponent<Liner>().OnMove(pos, k);
                            PuzzleData[i, j].GetComponent<Liner>().OnStart(pos, k);
                            //PuzzleData[i, j].transform.position = pos;
                            PuzzleData[i, j].transform.localScale = puzzlePrefab.transform.localScale;

                            // 空白PuzzleDataのブロックの上にブロックがないかrowLengthまで調べるのを終了
                            k = 100;

                            b = true;

                        }
                    }
                }
            }
        }

        //地面に到着した猫を探す
        /*
        for (int i = 0; i<columnLength; i++)
        {
            if (PuzzleData[i, DeathBlockHeight] != null)
            {
                if (PuzzleData[i, DeathBlockHeight].GetComponent<BlockData>().blockType == BlockType.Animal)
                {
                    PuzzleData[i, DeathBlockHeight].GetComponent<BlockData>().death = true;

                }
            }
        }
        */

        return b;
    }

    //現在選択中の英語ブロックを消す
    public void SelectEigoDestroy()
    {
        List<BlockData> blockDataList = new List<BlockData>();


        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                if (PuzzleData[i, j] != null)
                {
                    if (PuzzleData[i, j].GetComponent<BlockData>().EigoFlg)
                    {
                        blockDataList.Add(PuzzleData[i, j].GetComponent<BlockData>());
                        Destroy(PuzzleData[i, j]);
                        PuzzleData[i, j] = null;
                        //yield return new WaitForSeconds(0.2f);
                    }

                }

            }
        }

        CheckBlockSpace();
    }

    // 移動中のブロックがないかチェック true:移動中、false:移動中なし
    public bool CheckBlockMove()
    {
        //PuzzleDataが移動中か調べる
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength+margin_height; j++)
            {
                //空白のPuzzleData以外の時
                if (PuzzleData[i, j] != null)
                {
                    //PuzzleDataが移動中の時
                    if (PuzzleData[i, j].GetComponent<Liner>().iMove == true)
                    {
                        return true;
                    }
                }

            }
        }
        return false;

    }

    // ステージのブロックを作成
    public void stageMaker(string filename)
    {

        //　テキストファイルからデータを読み込む
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load(filename, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        textMessage = TextLines.Split('\n'); //

        //行数と列数を取得
        string[] columstr = textMessage[0].Split(',');
        columnLength = columstr.Length - 1;
        rowLength = textMessage.Length;

        // 画面に出すブロックの縦数は最大7にする
        if (rowLength > 7)
            DefaultBlockHeight = 7;
        else
            DefaultBlockHeight = rowLength;

        // ステージ用のテキストファイルを２次元配列データに格納する用の２次元配列を作成
        stageData = new string[columnLength, rowLength];

        // stageDataから空き枠以外をMaskDataに格納する用の２次元配列を作成
        MaskData = new GameObject[columnLength, rowLength+margin_height];

        // stageDataから英語ブロック、猫などを格納する用の２次元配列を作成 
        PuzzleData = new GameObject[columnLength, rowLength+margin_height];


        //2次配列を定義
        textWords = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++)
        {

            string[] tempWords = textMessage[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++)
            {
                textWords[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }

        //char[] eigochar = "AAAAAABBCCCDDDEEEEEEFFGGGHHHIIIIIJKKKLLLMMMNNNOOOOPPQRRRSSSTTTUUUUVWWXYYYZ".ToCharArray();

        int k = rowLength - 1;
        for (int i = 0; i < rowLength; i++)
        {
            for (int n = 0; n < columnLength; n++)
            {
                string str = textWords[i, n];

                //bird * kaeru # rabbit $
                if (str == "*")
                {
                    stageData[n, k] = "star";
                }
                //maskなし
                else if (str == "-")
                {
                    stageData[n, k] = "";
                }
                //アルファベットなしmaskあり
                else if (str == "+")
                {
                    stageData[n, k] = "+";
                }
                // 犬の初期位置の時
                else if (str == "!")
                {

                    stageData[n, k] = "+";
                    //犬の初期値をセットする
                    DogData dogd = DogObject.GetComponent<DogData>();
                    dogd.setPos(n, k, BlockSize, rowLength, columnLength, DefaultBlockHeight, BlockGroundHeight);

                }
                // 固体ブロック（消せない）の時
                else if(str =="@")
                {
                    stageData[n, k] = "crimp";
                }
                //アルファベットランダム
                else if (str == "?")
                { 
                    stageData[n, k] = RandomMake.alphabet();
                }
                //アルファベットの時
                else
                {
                    stageData[n, k] = str;
                }

            }
            k--;
        }

        // stageDataからMaskDataを作成する
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {

                //空白の時
                if (stageData[i, j] != "")
                {

                    // アルファベットなしmaskありの時
                    if (stageData[i, j] == "+")
                        stageData[i, j] = "";

                    Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, j * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    // スクリプトからインスタンス（動的にゲームオブジェクトを指定数だけ作る
                    MaskData[i, j] = Instantiate(MaskPrefab, pos, Quaternion.identity);

                    MaskData[i, j].name = "Mask";
                    MaskData[i, j].transform.SetParent(puzzleTransform);
                    MaskData[i, j].transform.localPosition = pos;
                    MaskData[i, j].transform.localScale = MaskPrefab.transform.localScale;

                }

            }
        }

        // neko1～3のspriteをランダムで決める 
        // PuzzleData[i, j].GetComponent<BlockData>().setAnimal(ねこID); ※ねこIDは4,6,8,10,12,14,16,18,20,22,24の11匹
        int[] NekoIDRand = new int[29] { 4, 6, 8, 8, 8, 10, 12, 12, 12, 14, 16, 18, 18, 20, 20, 20, 22, 22, 24, 26, 28, 28, 30, 32, 34, 34, 36, 38, 40 };
        int[] NekoIDRand1 = new int[14] { 4, 6, 8, 8, 8, 10, 12, 12, 12, 14, 16, 18, 18, 40 };
        int[] NekoIDRand2 = new int[15] { 20, 20, 22, 22, 24, 26, 28, 28, 30, 32, 34, 34, 36, 38, 38 };
        int[] NekoIDRandBest = new int[8] { 4, 8, 12, 18, 20, 22, 26, 34 };
        //int[] NekoIDRand = new int[3] { 20, 22, 24 };

        // starは4

        int NekoID1=4, NekoID2=4, NekoID3=4;
        
        StatusData.AnimalTypeOK1 = (AnimalType)NekoID1;
        StatusData.AnimalTypeOK2 = (AnimalType)NekoID2;
        StatusData.AnimalTypeOK3 = (AnimalType)NekoID3;
            


        // stageDataからPuzzleDataを作成する
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {

                //空白の時
                if (stageData[i, j] != "")
                {

                    Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, j * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                    //Vector2 pos = new Vector2(i * BlockSize - 320 + 45 + margin, j * BlockSize - 270);

                    // スクリプトからインスタンス（動的にゲームオブジェクトを指定数だけ作る
                    PuzzleData[i, j] = Instantiate(puzzlePrefab, pos, Quaternion.identity);
                    if (stageData[i, j] == "star")
                    {
                        PuzzleData[i, j].GetComponent<BlockData>().setup(BlockType.Animal, stageData[i, j], false, i, j);
                        PuzzleData[i, j].GetComponent<BlockData>().setAnimalInt(NekoID1);
                        PuzzleData[i, j].name = "star"; // GameObjectの名前を決めている
                        StatusData.Animal1++;
                        StatusData.AnimalSum++;

                        // STARのObjectを追加
                        StarDataList.Add(PuzzleData[i, j]);

                    }
                    // 障害ブロックの時
                    else if(stageData[i, j] == "crimp")
                    {
                        PuzzleData[i, j].GetComponent<BlockData>().setup(BlockType.CRIMP, stageData[i, j], false, i, j);
                        PuzzleData[i, j].name = "Crimp"; // GameObjectの名前を決めている

                    }
                    else
                    {
                        PuzzleData[i, j].GetComponent<BlockData>().setup(BlockType.ALPHABET, stageData[i, j], false, i, j);
                        PuzzleData[i, j].name = "Block"; // GameObjectの名前を決めている
                    }

                    // 生成したGameObjectをヒエラルキーに表示
                    PuzzleData[i, j].transform.SetParent(puzzleTransform);
                    PuzzleData[i, j].transform.localPosition = pos;
                    PuzzleData[i, j].transform.localScale = puzzlePrefab.transform.localScale;

                }
            }
        }


    }

    /// <returns></returns>

    public bool UndderArrowCheck()
    {
        // 地面の下にブロックがある時
        if (UnderArrowHeight > 0)
        {

            /*
            for (int i = 0; i < columnLength; i++)
            {
                // 最上部行が全て空いているか確認（Maskのないエリアの場合も空いているカウントする）
                if (PuzzleData[i, rowLength - 1] != null)
                {
                    BlockData blockData = PuzzleData[i, rowLength - 1].GetComponent<BlockData>();

                    //詰まっている列があれば何もしない
                    if (blockData.blockType == BlockType.ALPHABET || blockData.blockType == BlockType.Animal)
                    {
                        return false;
                    }
                }
            }
            */
            // 犬をパズルデータにコピー
            PuzzleData[DogObject.GetComponent<DogData>().X, DogObject.GetComponent<DogData>().Y] = DogObject;
            if (DogObject.GetComponent<DogData>().drop_count > 0)
                DogObject.GetComponent<DogData>().iMove = true;

            //上に移動できる分全ての列を一番高い列が詰まるまで移動
            for (int i = 0; i < columnLength; i++)
            {
                for (int j = rowLength - 1+margin_height; j >= 0; j--)
                {
                    //もしNULL以外のPuzzleDataのブロックが見つかった時
                    if (PuzzleData[i, j] != null)
                    {
                    //PuzzleData[i, j].GetComponent<BlockData>().X = i;
                    PuzzleData[i, j].GetComponent<BlockData>().Y = j + 1;

                        PuzzleData[i, j + 1] = PuzzleData[i, j];
                        PuzzleData[i, j] = null;

                        //PuzzleDataのブロックの表示座標を更新する
                        Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (j + 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                        PuzzleData[i, j + 1].transform.SetParent(puzzleTransform);

                        PuzzleData[i, j + 1].GetComponent<Liner>().OnUpper(pos, 1);

                        PuzzleData[i, j + 1].transform.localScale = puzzlePrefab.transform.localScale;
                    }
                    // 犬を上に動かす
                    //DogObject.GetComponent<DogData>().OnUpper(pos, 1);

                }
            }

            //Vector2 pos2 = new Vector2(DogObject.GetComponent<DogData>().X * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (DogObject.GetComponent<DogData>().Y + 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

            //犬を上に動かす
            //DogObject.GetComponent<DogData>().OnUpper(pos2, 1);


            // Maskを上に移動する Maskが二重になる問題発生
            for (int i = 0; i < columnLength; i++)
            {
                for (int j = rowLength+margin_height - 1; j >= 0; j--)
                {
                    //もしNULL以外のPuzzleDataのブロックが見つかった時
                    if (MaskData[i, j] != null)
                    {
                        //PuzzleData[i, j].GetComponent<BlockData>().X = i;
                        //PuzzleData[i, j].GetComponent<BlockData>().Y = j + 1;

                        MaskData[i, j + 1] = MaskData[i, j];
                        MaskData[i, j] = null;

                        //PuzzleDataのブロックの表示座標を更新する
                        Vector2 pos = new Vector2(i * BlockSize - (BlockSize * columnLength) / 2 + BlockSize / 2, (j + 1) * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize);

                        MaskData[i, j + 1].transform.localScale = MaskPrefab.transform.localScale;
                        MaskData[i, j + 1].transform.SetParent(puzzleTransform);
                        MaskData[i, j + 1].GetComponent<Liner>().OnUpper(pos, 1);
                    }

                }
            }


            // 残り上の行に移動できる回数をへらす
            UnderArrowHeight--;

            // ブックを消すPuzzleDataの座標を1つ上げる
            DeathBlockHeight++;

            // GlassLineを移動する
            Vector3 glass_pos = new Vector3(0, GlassLineHeight + UnderArrowHeight * -BlockSize, -1);
            GlassLine.GetComponent<Liner>().OnUpper(glass_pos, 1);

            // 下部草画像を移動する
            //ector3 glassimg_pos = new Vector3(0, ActiveBlockHeight * BlockSize + BlockGroundHeight - (rowLength - DefaultBlockHeight) * BlockSize,-1);
            //ImgGround.GetComponent<Liner>().OnUpper(glassimg_pos, 1);

            Vector3 back_pos = new Vector3(0, GlassLineHeight + UnderArrowHeight * -BlockSize + 300, 100);
            BackPicture.GetComponent<Liner>().OnUpper(back_pos, 1);

            return true;
        }
        // 上げるブロックがない時に犬が下に移動した時
        else
        {
                        
            DogObject.GetComponent<DogData>().iMove = false;
            return false;
        }

    }

    // can_alphabetにパズルエリアのアルフェベットを格納
    public void CheckPotentialPuzzle()
    {
        char[] eigochar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        // パズルエリアのアルファベットA-Zまで0を格納する
        for (int i = 0; i < 26; i++)
        {
            can_alphabet[i] = 0;
        }

        // パズルエリアのアルファベットの数を格納する
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = ActiveBlockHeight; j < rowLength; j++)
            {

                if (PuzzleData[i, j] != null)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        char eigo = PuzzleData[i, j].GetComponent<BlockData>().Alphabet[0];
                        if (eigochar[k] == eigo)
                        {
                            can_alphabet[k]++;
                            k = 100;
                        }
                    }

                }
            }
        }

        // 現在のアルファベットブロック数をDebugLogに表示
        /*
        for (int i = 0; i < 26; i++)
        {
            Debug.Log(eigochar[i] + ":" + can_alphabet[i]);
        }
        */

        GameObject.Find("CanWordText").GetComponent<CanWordController>().CopyCanAlphabet(can_alphabet);

    }
    void WinPopup()
    {
        Canvas m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        var popup = Instantiate(popupWinPrefab) as GameObject;
        popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.SetParent(m_canvas.transform, false);
        popup.GetComponent<WinLosePopup>().Open();

    }

    void LosePopup()
    {
        Canvas m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        var popup = Instantiate(popupLosePrefab) as GameObject;
        popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.SetParent(m_canvas.transform, false);
        popup.GetComponent<WinLosePopup>().Open();

    }


}