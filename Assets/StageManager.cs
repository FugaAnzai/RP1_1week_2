using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    enum TILE_TYPE
    {
        FLOOR,
        PLAYER,
        WALL,
        POWER,
        BATTERY,
        GOAL,
        GOALWALL
    }

    TILE_TYPE[,] tileTable;
    int[,] playerTable;
    GameObject[,] field;
    GameObject[,] playerField;
    GameObject[] elecricEffect;

    public GameObject floorPrafab_;
    public GameObject playerPrafab_;
    public GameObject wallPrafab_;
    public GameObject powerPrefab_;
    public GameObject batteryPrefab_;
    public GameObject goalPrefab_;
    public GameObject goalwallPrefab_;
    public GameObject elecEffect;
    public TextAsset stageFile;
    public TextAsset stagePlayerFile;

    private bool isChangePower_ = false;
    private int powerNumber_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        elecricEffect = new GameObject[8];

        isChangePower_ = false;
        powerNumber_ = 0;
        LoadTileData();
        CreateStage();
    }

    // Update is called once per frame
    void Update()
    {
        // 電源を切り替えたフレームか判定の初期化
        isChangePower_ = false;
        for (int y1 = 0; y1 < field.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < field.GetLength(1); x1++)
            {
                if (field[y1, x1] != null && field[y1, x1].tag == "Power")
                {
                    field[y1, x1].GetComponent<PowerSpriteChange>().isChangeFrame_ = false;
                    field[y1, x1].GetComponent<PowerSpriteChange>().isChangeClear_ = false;
                }
            }
        }

        ElectricEffect();

        PlayerMove();

        // ゴール判定
        CheckBattery();

        // ESC || ゴールしたら
        Vector2Int playerIndex = GetPlayerIndex();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainManagerScript.stageSelect = 0;
            SceneManager.LoadScene("MainScene");
        }

        if (field[playerIndex.y, playerIndex.x].tag == "Goal")
        {
            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                SceneManager.LoadScene("Stage2");
            }

            if (SceneManager.GetActiveScene().name == "Stage2")
            {
                SceneManager.LoadScene("Stage3");
            }

            if (SceneManager.GetActiveScene().name == "Stage3")
            {
                SceneManager.LoadScene("Stage4");
            }

            if (SceneManager.GetActiveScene().name == "Stage4")
            {
                SceneManager.LoadScene("Stage5");
            }

            if (SceneManager.GetActiveScene().name == "Stage5")
            {
                SceneManager.LoadScene("Stage6");
            }

            if (SceneManager.GetActiveScene().name == "Stage6")
            {
                SceneManager.LoadScene("Stage7");
            }

            if (SceneManager.GetActiveScene().name == "Stage7")
            {
                SceneManager.LoadScene("Stage8");
            }

            if (SceneManager.GetActiveScene().name == "Stage8")
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

    }

    public void LoadTileData()
    {
        string[] lines = stageFile.text.Split('\n'); 
        int rows = lines.Length;
        int columns = lines[0].Split(new char[] { ',' }).Length;
        tileTable = new TILE_TYPE[rows, columns];

        for(int y = 0; y < rows; y++)
        {
            string[] values = lines[y].Split(new char[] { ',' });

            for (int x = 0; x < columns; x++)
            {
                tileTable[y,x] = (TILE_TYPE)int.Parse(values[x]);
            }
        }

        string[] lines2 = stagePlayerFile.text.Split('\n');
        int rows2 = lines2.Length;
        int columns2 = lines2[0].Split(new char[] { ',' }).Length;
        playerTable = new int[rows2, columns2];

        for (int y = 0; y < rows2; y++)
        {
            string[] values = lines2[y].Split(new char[] { ',' });

            for (int x = 0; x < columns2; x++)
            {
                playerTable[y, x] = int.Parse(values[x]);
            }
        }

    }

    public void CreateStage()
    {
        field = new GameObject[tileTable.GetLength(0), tileTable.GetLength(1)];
        playerField = new GameObject[playerTable.GetLength(0), playerTable.GetLength(1)];

        for (int y = 0; y < tileTable.GetLength(0); y++)
        {
            for(int x = 0; x < tileTable.GetLength(1); x++)
            {
                if (tileTable[y, x] == TILE_TYPE.FLOOR)
                {
                    field[y, x] = Instantiate(floorPrafab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }

                if (tileTable[y,x] == TILE_TYPE.PLAYER)
                {
                    field[y,x] = Instantiate(playerPrafab_, new Vector3(x, tileTable.GetLength(0) - y, 0),Quaternion.identity);
                }

                if (tileTable[y, x] == TILE_TYPE.WALL)
                {
                    field[y, x] = Instantiate(wallPrafab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }

                if (tileTable[y,x] == TILE_TYPE.POWER)
                {
                    field[y, x] = Instantiate(powerPrefab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }

                if (tileTable[y, x] == TILE_TYPE.BATTERY)
                {
                    field[y, x] = Instantiate(batteryPrefab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }

                if (tileTable[y, x] == TILE_TYPE.GOAL)
                {
                    field[y, x] = Instantiate(goalPrefab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }

                if (tileTable[y, x] == TILE_TYPE.GOALWALL)
                {
                    field[y, x] = Instantiate(goalwallPrefab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }
            }
        }

        for (int y = 0; y < playerTable.GetLength(0); y++)
        {
            for (int x = 0; x < playerTable.GetLength(1); x++)
            {
                if (playerTable[y, x] == 1)
                {
                    playerField[y, x] = Instantiate(playerPrafab_, new Vector3(x, tileTable.GetLength(0) - y, 0), Quaternion.identity);
                }
            }
        }
    }

    public Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < playerField.GetLength(0); y++)
        {
            for (int x = 0; x < playerField.GetLength(1); x++)
            {
                if (playerField[y, x] != null && playerField[y, x].tag == "Player")
                {
                    Vector2Int resultTrue = new(x, y);
                    Debug.Log(resultTrue.ToString());
                    return resultTrue;
                }
            }
        }

        Vector2Int resultFalse = new(-1, -1);

        return resultFalse;
    }

    bool MoveObject(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if ((field[moveTo.y, moveTo.x] != null && (field[moveTo.y, moveTo.x].tag == "Wall" || field[moveTo.y, moveTo.x].tag == "Power")) ||
            // 通電床が移動先にあったら移動不可
            (field[moveTo.y, moveTo.x] != null && (field[moveTo.y, moveTo.x].tag == "Floor" && field[moveTo.y, moveTo.x].GetComponent<FloorSpriteChange>().GetIsChange())) ||
            // 電池が移動先にあったら移動不可
            (field[moveTo.y, moveTo.x] != null && (field[moveTo.y, moveTo.x].tag == "Battery")) ||
            // ゴール周り壁があったら移動不可
            (field[moveTo.y, moveTo.x] != null && (field[moveTo.y, moveTo.x].tag == "GoalWall" && field[moveTo.y, moveTo.x].GetComponent<GoalWallSpriteChange>().GetIsChange())))
        {
            Debug.Log("Failed");
            return false;
        }

        //オブジェクトの座標を変更
        playerField[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, playerField.GetLength(0) - moveTo.y, 0);
        //現在のプレイヤーオブジェクトを移動先に代入
        playerField[moveTo.y, moveTo.x] = playerField[moveFrom.y, moveFrom.x];
        //移動前のオブジェクトにnullを代入
        playerField[moveFrom.y, moveFrom.x] = null;
        return true;

    }

    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {

            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int move = new(1, 0);

            for (int y = 0; y < playerField.GetLength(0); y++)
            {
                for (int x = 0; x < playerField.GetLength(1); x++)
                {
                    if (playerField[y, x] != null && playerField[y, x].tag == "Player")
                    {
                        playerField[y, x].GetComponent<SpriteRenderer>().flipX = false;
                    }
                }
            }

            MoveObject("Player", playerIndex, playerIndex + move);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            Vector2Int move = new(1, 0);

            for (int y = 0; y < playerField.GetLength(0); y++)
            {
                for (int x = 0; x < playerField.GetLength(1); x++)
                {
                    if (playerField[y, x] != null && playerField[y, x].tag == "Player")
                    {
                        playerField[y,x].GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
            }

            MoveObject("Player", playerIndex, playerIndex - move);

        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {

            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int move = new(0, 1);

            MoveObject("Player", playerIndex, playerIndex - move);

        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            Vector2Int move = new(0, 1);

            MoveObject("Player", playerIndex, playerIndex + move);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Power();
            FloorElectrification();
        }

    }

    void Power()
    {
        Vector2Int playerIndex = GetPlayerIndex();
        Vector2Int upIndex = playerIndex + new Vector2Int(0, -1);
        Vector2Int downIndex = playerIndex + new Vector2Int(0, 1);
        Vector2Int rightIndex = playerIndex + new Vector2Int(1, 0);
        Vector2Int leftIndex = playerIndex + new Vector2Int(-1, 0);
        Vector2Int upRightIndex = playerIndex + new Vector2Int(1, -1);
        Vector2Int upLeftIndex = playerIndex + new Vector2Int(-1, -1);
        Vector2Int downRightIndex = playerIndex + new Vector2Int(1, 1);
        Vector2Int downLeftIndex = playerIndex + new Vector2Int(-1, 1);


        if (field[upIndex.y, upIndex.x] != null && field[upIndex.y,upIndex.x].tag == "Power")
        {
            PowerChange(upIndex);
        }

        if (field[downIndex.y, downIndex.x] != null && field[downIndex.y, downIndex.x].tag == "Power")
        {
            PowerChange(downIndex);
        }

        if (field[rightIndex.y, rightIndex.x] != null && field[rightIndex.y, rightIndex.x].tag == "Power")
        {
            PowerChange(rightIndex);
        }

        if (field[leftIndex.y, leftIndex.x] != null && field[leftIndex.y, leftIndex.x].tag == "Power")
        {
            PowerChange(leftIndex);
        }

        if (field[upRightIndex.y, upRightIndex.x] != null && field[upRightIndex.y, upRightIndex.x].tag == "Power")
        {
            PowerChange(upRightIndex);
        }

        if (field[upLeftIndex.y, upLeftIndex.x] != null && field[upLeftIndex.y, upLeftIndex.x].tag == "Power")
        {
            PowerChange(upLeftIndex);
        }

        if (field[downRightIndex.y, downRightIndex.x] != null && field[downRightIndex.y, downRightIndex.x].tag == "Power")
        {
            PowerChange(downRightIndex);
        }

        if (field[downLeftIndex.y, downLeftIndex.x] != null && field[downLeftIndex.y, downLeftIndex.x].tag == "Power")
        {
            PowerChange(downLeftIndex);
        }

    }

    void ElectricEffect()
    {

        Vector2Int playerIndex = GetPlayerIndex();
        Vector2Int upIndex = playerIndex + new Vector2Int(0, -1);
        Vector2Int downIndex = playerIndex + new Vector2Int(0, 1);
        Vector2Int rightIndex = playerIndex + new Vector2Int(1, 0);
        Vector2Int leftIndex = playerIndex + new Vector2Int(-1, 0);
        Vector2Int upRightIndex = playerIndex + new Vector2Int(1, -1);
        Vector2Int upLeftIndex = playerIndex + new Vector2Int(-1, -1);
        Vector2Int downRightIndex = playerIndex + new Vector2Int(1, 1);
        Vector2Int downLeftIndex = playerIndex + new Vector2Int(-1, 1);

        if (field[upIndex.y, upIndex.x] != null && field[upIndex.y, upIndex.x].tag == "Power" && field[upIndex.y, upIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[0] == null)
            {
                elecricEffect[0] = Instantiate(elecEffect, new Vector3(upIndex.x, tileTable.GetLength(0) - upIndex.y, 0), Quaternion.identity);
                elecricEffect[0].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[0]);
        }

        if (field[downIndex.y, downIndex.x] != null && field[downIndex.y, downIndex.x].tag == "Power" && field[downIndex.y, downIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[1] == null)
            {
                elecricEffect[1] = Instantiate(elecEffect, new Vector3(downIndex.x, tileTable.GetLength(0) - downIndex.y, 0), Quaternion.identity);
                elecricEffect[1].GetComponent<ParticleSystem>().Play();
            }
        }
        else {
            Destroy(elecricEffect[1]);
        }

        if (field[rightIndex.y, rightIndex.x] != null && field[rightIndex.y, rightIndex.x].tag == "Power" && field[rightIndex.y, rightIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[2] == null)
            {
                elecricEffect[2] = Instantiate(elecEffect, new Vector3(rightIndex.x, tileTable.GetLength(0) - rightIndex.y, 0), Quaternion.identity);
                elecricEffect[2].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[2]);
        }

        if (field[leftIndex.y, leftIndex.x] != null && field[leftIndex.y, leftIndex.x].tag == "Power" && field[leftIndex.y, leftIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[3] == null)
            {
                elecricEffect[3] = Instantiate(elecEffect, new Vector3(leftIndex.x, tileTable.GetLength(0) - leftIndex.y, 0), Quaternion.identity);
                elecricEffect[3].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[3]);
        }

        if (field[upRightIndex.y, upRightIndex.x] != null && field[upRightIndex.y, upRightIndex.x].tag == "Power" && field[upRightIndex.y, upRightIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[4] == null)
            {
                elecricEffect[4] = Instantiate(elecEffect, new Vector3(upRightIndex.x, tileTable.GetLength(0) - upRightIndex.y, 0), Quaternion.identity);
                elecricEffect[4].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[4]);
        }

        if (field[upLeftIndex.y, upLeftIndex.x] != null && field[upLeftIndex.y, upLeftIndex.x].tag == "Power" && field[upLeftIndex.y, upLeftIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[5] == null)
            {
                elecricEffect[5] = Instantiate(elecEffect, new Vector3(upLeftIndex.x, tileTable.GetLength(0) - upLeftIndex.y, 0), Quaternion.identity);
                elecricEffect[5].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[5]);
        }

        if (field[downRightIndex.y, downRightIndex.x] != null && field[downRightIndex.y, downRightIndex.x].tag == "Power" && field[downRightIndex.y, downRightIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[6] == null)
            {
                elecricEffect[6] = Instantiate(elecEffect, new Vector3(downRightIndex.x, tileTable.GetLength(0) - downRightIndex.y, 0), Quaternion.identity);
                elecricEffect[6].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[6]);
        }

        if (field[downLeftIndex.y, downLeftIndex.x] != null && field[downLeftIndex.y, downLeftIndex.x].tag == "Power" && field[downLeftIndex.y, downLeftIndex.x].GetComponent<PowerSpriteChange>().GetIsChange() == false)
        {
            if (elecricEffect[7] == null)
            {
                elecricEffect[7] = Instantiate(elecEffect, new Vector3(downLeftIndex.x, tileTable.GetLength(0) - downLeftIndex.y, 0), Quaternion.identity);
                elecricEffect[7].GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Destroy(elecricEffect[7]);
        }


    }

    void PowerChange(Vector2Int vector2)
    {
        field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().GetIsChange());
        field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().isChangeFrame_ = field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().GetIsChange();
        field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().isChangeClear_ = true;
        isChangePower_ = true;

        // 番号をつける
        if (field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().isChangeFrame_)
        {
            powerNumber_ += 1;
            field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().number_ = powerNumber_;
        }

    }

    // 床の通電処理
    void FloorElectrification()
    {
        for (int y1 = 0; y1 < field.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < field.GetLength(1); x1++)
            {
                if (field[y1, x1] != null && (field[y1, x1].tag == "Floor" || field[y1, x1].tag == "Battery"))
                {
                    // 通電したかフラグ
                    bool isElectrification = false;

                    // 電源の入ったスイッチが同じX軸、Y軸にあるなら通電する
                    for (int y2 = 0; y2 < field.GetLength(0); y2++)
                    {
                        // 同じX軸にあり、電源が入っているなら通電する
                        if (field[y2, x1] != null && field[y2, x1].tag == "Power" && field[y2, x1].GetComponent<PowerSpriteChange>().isChangeFrame_)
                        {
                            // 電源と対象の床の間に（既に通電している床 || 電源 || プレイヤー）があるなら通電しない
                            // 向きの取得
                            int tmpVertical = y1 - y2;

                            // 間に通電済みがあったかフラグ
                            bool isExisting_ = false;

                            // 電源の上方向にある場合で、間に通電済みがあるか判定
                            if (!isExisting_ && tmpVertical < 0 && y2 > 0)
                            {
                                for (int y3 = (y2 - 1); y3 >= y1; y3--)
                                {
                                    if (CheckBetween(x1, y3))
                                    {
                                        isExisting_ = true;
                                        break;
                                    }
                                }
                            }
                            // 電源の下方向にある場合で、間に通電済みがあるか判定
                            if (!isExisting_ && tmpVertical > 0 && y2 < field.GetLength(0))
                            {
                                for (int y3 = (y2 + 1); y3 <= y1; y3++)
                                {
                                    if (CheckBetween(x1, y3))
                                    {
                                        isExisting_ = true;
                                        break;
                                    }
                                }
                            }

                            // 間に通電済みがあったらループを抜ける
                            if (isExisting_)
                            {
                                break;
                            }

                            if (field[y1, x1].tag == "Floor")
                            {
                                field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(true);
                                field[y1, x1].GetComponent<FloorSpriteChange>().number_ = field[y2, x1].GetComponent<PowerSpriteChange>().number_;
                            }
                            if (field[y1, x1].tag == "Battery")
                            {
                                field[y1, x1].GetComponent<BatterySpriteChange>().SetIsChangeReady(true);
                                field[y1, x1].GetComponent<BatterySpriteChange>().number_ = field[y2, x1].GetComponent<PowerSpriteChange>().number_;
                            }
                            isElectrification = true;
                            break;
                        }

                        // 通電していないならただの床にする
                        else if (!isElectrification && ((field[y1, x1].tag == "Floor" && field[y1, x1].GetComponent<FloorSpriteChange>().GetIsChange()) || (field[y1, x1].tag == "Battery" && field[y1, x1].GetComponent<BatterySpriteChange>().GetIsChange())) && field[y2, x1] != null && field[y2, x1].tag == "Power" && !field[y2, x1].GetComponent<PowerSpriteChange>().isChangeFrame_ && field[y2, x1].GetComponent<PowerSpriteChange>().isChangeClear_)
                        {
                            // 消した電源と同じ番号を持つ床を消す
                            if (field[y1, x1].tag == "Floor" && field[y1, x1].GetComponent<FloorSpriteChange>().number_ == field[y2, x1].GetComponent<PowerSpriteChange>().number_)
                            {
                                field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(false);
                            }
                            if (field[y1, x1].tag == "Battery" && field[y1, x1].GetComponent<BatterySpriteChange>().number_ == field[y2, x1].GetComponent<PowerSpriteChange>().number_)
                            {
                                field[y1, x1].GetComponent<BatterySpriteChange>().SetIsChangeReady(false);
                            }
                        }
                    }
                    for (int x2 = 0; x2 < field.GetLength(1); x2++)
                    {
                        // すでに通電してたらループしない
                        if (isElectrification)
                        {
                            break;
                        }

                        // 同じY軸にあり、電源が入っているなら通電する
                        if (field[y1, x2] != null && field[y1, x2].tag == "Power" && field[y1, x2].GetComponent<PowerSpriteChange>().isChangeFrame_)
                        {
                            // 電源と対象の床の間に（既に通電している床 || 電源 || プレイヤー）があるなら通電しない
                            // 向きの取得
                            int tmpHorizontal = x1 - x2;

                            // 間に通電済みがあったかフラグ
                            bool isExisting_ = false;

                            // 電源の左方向にある場合で、間に通電済みがあるか判定
                            if (!isExisting_ && tmpHorizontal < 0 && x2 > 0)
                            {
                                for (int x3 = (x2 - 1); x3 >= x1; x3--)
                                {
                                    if (CheckBetween(x3, y1))
                                    {
                                        isExisting_ = true;
                                        break;
                                    }
                                }
                            }
                            // 電源の右方向にある場合で、間に通電済みがあるか判定
                            if (!isExisting_ && tmpHorizontal > 0 && x2 < field.GetLength(1))
                            {
                                for (int x3 = (x2 + 1); x3 <= x1; x3++)
                                {
                                    if (CheckBetween(x3, y1))
                                    {
                                        isExisting_ = true;
                                        break;
                                    }
                                }
                            }

                            // 間に通電済みがあったらループを抜ける
                            if (isExisting_)
                            {
                                break;
                            }

                            if (field[y1, x1].tag == "Floor")
                            {
                                field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(true);
                                field[y1, x1].GetComponent<FloorSpriteChange>().number_ = field[y1, x2].GetComponent<PowerSpriteChange>().number_;
                            }
                            if (field[y1, x1].tag == "Battery")
                            {
                                field[y1, x1].GetComponent<BatterySpriteChange>().SetIsChangeReady(true);
                                field[y1, x1].GetComponent<BatterySpriteChange>().number_ = field[y1, x2].GetComponent<PowerSpriteChange>().number_;
                            }
                            isElectrification = true;
                            break;
                        }

                        // 通電していないならただの床にする
                        else if (!isElectrification && ((field[y1, x1].tag == "Floor" && field[y1, x1].GetComponent<FloorSpriteChange>().GetIsChange()) || (field[y1, x1].tag == "Battery" && field[y1, x1].GetComponent<BatterySpriteChange>().GetIsChange())) && field[y1, x2] != null && field[y1, x2].tag == "Power" && !field[y1, x2].GetComponent<PowerSpriteChange>().isChangeFrame_ && field[y1, x2].GetComponent<PowerSpriteChange>().isChangeClear_)
                        {
                            // 消した電源と同じ番号を持つ床を消す
                            if (field[y1, x1].tag == "Floor" && field[y1, x1].GetComponent<FloorSpriteChange>().number_ == field[y1, x2].GetComponent<PowerSpriteChange>().number_)
                            {
                                field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(false);
                            }
                            if (field[y1, x1].tag == "Battery" && field[y1, x1].GetComponent<BatterySpriteChange>().number_ == field[y1, x2].GetComponent<PowerSpriteChange>().number_)
                            {
                                field[y1, x1].GetComponent<BatterySpriteChange>().SetIsChangeReady(false);
                            }
                        }
                    }
                }
            }
        }

        // 一気に通電処理をする（電源を切り替えたフレームのみ）
        if (isChangePower_)
        {
            for (int y1 = 0; y1 < field.GetLength(0); y1++)
            {
                for (int x1 = 0; x1 < field.GetLength(1); x1++)
                {
                    if (field[y1, x1] != null && field[y1, x1].tag == "Floor")
                    {
                        field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChange();
                    }

                    if (field[y1, x1] != null && field[y1, x1].tag == "Battery")
                    {
                        field[y1, x1].GetComponent<BatterySpriteChange>().SetIsChange();
                    }

                    if (field[y1, x1] != null && field[y1, x1].tag == "Power" && field[y1, x1].GetComponent<PowerSpriteChange>().isChangeClear_ && !field[y1, x1].GetComponent<PowerSpriteChange>().GetIsChange())
                    {
                        field[y1, x1].GetComponent<PowerSpriteChange>().number_ = 0;
                    }
                }
            }
        }
    }

    private bool CheckBetween(int x, int y)
    {
        if (field[y, x] != null && field[y, x].tag == "Wall")
        {
            return true;
        }
        if (field[y, x] != null && field[y, x].tag == "GoalWall")
        {
            return true;
        }
        if (field[y, x] != null && field[y, x].tag == "Floor" && field[y, x].GetComponent<FloorSpriteChange>().GetIsChange())
        {
            return true;
        }
        if (field[y, x] != null && field[y, x].tag == "Battery" && field[y, x].GetComponent<BatterySpriteChange>().GetIsChange())
        {
            return true;
        }
        if (playerField[y, x] != null && playerField[y, x].tag == "Player")
        {
            return true;
        }
        if (field[y, x] != null && field[y, x].tag == "Power")
        {
            return true;
        }

        return false;
    }

    public void CheckBattery()
    {
        bool isClear_ = true;

        for (int y1 = 0; y1 < field.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < field.GetLength(1); x1++)
            {
                if (field[y1, x1] != null && field[y1, x1].tag == "Battery" && !field[y1, x1].GetComponent<BatterySpriteChange>().GetIsChange())
                {
                    isClear_ = false;
                    break;
                }
            }
        }

        for (int y1 = 0; y1 < field.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < field.GetLength(1); x1++)
            {
                if (field[y1, x1] != null && field[y1, x1].tag == "GoalWall")
                {
                    if (isClear_)
                    {
                        field[y1, x1].GetComponent<GoalWallSpriteChange>().SetIsChange(false);
                    }
                    else
                    {
                        field[y1, x1].GetComponent<GoalWallSpriteChange>().SetIsChange(true);
                    }
                }
            }
        }
    }

}
