using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    enum TILE_TYPE
    {
        FLOOR,
        PLAYER,
        WALL,
        POWER
    }

    TILE_TYPE[,] tileTable;
    int[,] playerTable;
    GameObject[,] field;
    GameObject[,] playerField;

    public GameObject floorPrafab_;
    public GameObject playerPrafab_;
    public GameObject wallPrafab_;
    public GameObject powerPrefab_;
    public TextAsset stageFile;
    public TextAsset stagePlayerFile;

    private bool isChangePower_ = false;
    private int powerPriority_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        isChangePower_ = false;
        powerPriority_ = 0;
        LoadTileData();
        CreateStage();
    }

    // Update is called once per frame
    void Update()
    {
        // �d����؂�ւ����t���[��������̏�����
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

        PlayerMove();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainScene");
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
        if (field[moveTo.y, moveTo.x] != null && (field[moveTo.y, moveTo.x].tag == "Wall" || field[moveTo.y, moveTo.x].tag == "Power"))
        {
            Debug.Log("Failed");
            return false;
        }

        //�I�u�W�F�N�g�̍��W��ύX
        playerField[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, playerField.GetLength(0) - moveTo.y, 0);
        //���݂̃v���C���[�I�u�W�F�N�g���ړ���ɑ��
        playerField[moveTo.y, moveTo.x] = playerField[moveFrom.y, moveFrom.x];
        //�ړ��O�̃I�u�W�F�N�g��null����
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

    void PowerChange(Vector2Int vector2)
    {
        field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().GetIsChange());
        field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().isChangeFrame_ = field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().GetIsChange();
        field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().isChangeClear_ = true;
        isChangePower_ = true;

        // �D�揇�ʂ�����
        if (field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().isChangeFrame_)
        {
            powerPriority_ += 1;
            field[vector2.y, vector2.x].GetComponent<PowerSpriteChange>().priority_ = powerPriority_;
        }
        else
        {
            powerPriority_ -= 1;
        }

    }

    // ���̒ʓd����
    void FloorElectrification()
    {

        for (int y1 = 0; y1 < field.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < field.GetLength(1); x1++)
            {
                if (field[y1, x1] != null && field[y1, x1].tag == "Floor")
                {
                    // �ʓd�������t���O
                    bool isElectrification = false;

                    // �d���̓������X�C�b�`������X���AY���ɂ���Ȃ�ʓd����
                    for (int y2 = 0; y2 < field.GetLength(0); y2++)
                    {
                        // ����X���ɂ���A�d���������Ă���Ȃ�ʓd����
                        if (field[y2, x1] != null && field[y2, x1].tag == "Power" && field[y2, x1].GetComponent<PowerSpriteChange>().isChangeFrame_)
                        {
                            // �d���ƑΏۂ̏��̊ԂɊ��ɒʓd���Ă��鏰������Ȃ�A�ʓd���Ȃ�
                            // �����̎擾
                            int tmpVertical = y1 - y2;

                            // �Ԃɒʓd�ς݂����������t���O
                            bool isExisting = false;

                            // �d���̏�����ɂ���ꍇ�ŁA�Ԃɒʓd�ς݂����邩����
                            if (!isExisting && tmpVertical < 0 && y2 > 0)
                            {
                                for (int y3 = (y2 - 1); y3 > y1; y3--)
                                {
                                    if (field[y3, x1] != null && field[y3, x1].tag == "Floor" && field[y3, x1].GetComponent<FloorSpriteChange>().GetIsChange())
                                    {
                                        isExisting = true;
                                        break;
                                    }
                                }
                            }
                            // �d���̉������ɂ���ꍇ�ŁA�Ԃɒʓd�ς݂����邩����
                            if (!isExisting && tmpVertical > 0 && y2 < field.GetLength(0))
                            {
                                for (int y3 = (y2 + 1); y3 < y1; y3++)
                                {
                                    if (field[y3, x1] != null && field[y3, x1].tag == "Floor" && field[y3, x1].GetComponent<FloorSpriteChange>().GetIsChange())
                                    {
                                        isExisting = true;
                                        break;
                                    }
                                }
                            }

                            // �Ԃɒʓd�ς݂��������烋�[�v�𔲂���
                            if (isExisting)
                            {
                                break;
                            }

                            field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(true);
                            isElectrification = true;
                            break;
                        }

                        // �ʓd���Ă��Ȃ��Ȃ炽���̏��ɂ���
                        else if (!isElectrification && field[y1, x1].GetComponent<FloorSpriteChange>().GetIsChange() && field[y2, x1] != null && field[y2, x1].tag == "Power" && !field[y2, x1].GetComponent<PowerSpriteChange>().isChangeFrame_ && field[y2, x1].GetComponent<PowerSpriteChange>().isChangeClear_)
                        {
                            // �Ώۂ̏���X���W�ɓd���̓������X�C�b�`���Ȃ����t���O
                            bool isCheck = false;

                            for (int x3 = 0; x3 < field.GetLength(1); x3++)
                            {
                                if (field[y1, x3] != null && field[y1, x3].tag == "Power" && field[y1, x3].GetComponent<PowerSpriteChange>().GetIsChange())
                                {
                                    // �D�揇�ʂ̔���
                                    if (field[y1, x3].GetComponent<PowerSpriteChange>().priority_ < field[y2, x1].GetComponent<PowerSpriteChange>().priority_)
                                    {
                                        isCheck = true;
                                        break;
                                    }
                                }
                            }

                            if (!isCheck)
                            {
                                field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(false);
                            }
                        }
                    }
                    for (int x2 = 0; x2 < field.GetLength(1); x2++)
                    {
                        // ���łɒʓd���Ă��烋�[�v���Ȃ�
                        if (isElectrification)
                        {
                            break;
                        }

                        // ����Y���ɂ���A�d���������Ă���Ȃ�ʓd����
                        if (field[y1, x2] != null && field[y1, x2].tag == "Power" && field[y1, x2].GetComponent<PowerSpriteChange>().isChangeFrame_)
                        {
                            // �d���ƑΏۂ̏��̊ԂɊ��ɒʓd���Ă��鏰������Ȃ�A�ʓd���Ȃ�
                            // �����̎擾
                            int tmpHorizontal = x1 - x2;

                            // �Ԃɒʓd�ς݂����������t���O
                            bool isExisting = false;

                            // �d���̍������ɂ���ꍇ�ŁA�Ԃɒʓd�ς݂����邩����
                            if (!isExisting && tmpHorizontal < 0 && x2 > 0)
                            {
                                for (int x3 = (x2 - 1); x3 > x1; x3--)
                                {
                                    if (field[y1, x3] != null && field[y1, x3].tag == "Floor" && field[y1, x3].GetComponent<FloorSpriteChange>().GetIsChange())
                                    {
                                        isExisting = true;
                                        break;
                                    }
                                }
                            }
                            // �d���̉E�����ɂ���ꍇ�ŁA�Ԃɒʓd�ς݂����邩����
                            if (!isExisting && tmpHorizontal > 0 && x2 < field.GetLength(1))
                            {
                                for (int x3 = (x2 + 1); x3 < x1; x3++)
                                {
                                    if (field[y1, x3] != null && field[y1, x3].tag == "Floor" && field[y1, x3].GetComponent<FloorSpriteChange>().GetIsChange())
                                    {
                                        isExisting = true;
                                        break;
                                    }
                                }
                            }

                            // �Ԃɒʓd�ς݂��������烋�[�v�𔲂���
                            if (isExisting)
                            {
                                break;
                            }

                            field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(true);
                            isElectrification = true;
                            break;
                        }

                        // �ʓd���Ă��Ȃ��Ȃ炽���̏��ɂ���
                        else if (!isElectrification && field[y1, x1].GetComponent<FloorSpriteChange>().GetIsChange() && field[y1, x2] != null && field[y1, x2].tag == "Power" && !field[y1, x2].GetComponent<PowerSpriteChange>().isChangeFrame_ && field[y1, x2].GetComponent<PowerSpriteChange>().isChangeClear_)
                        {
                            // �Ώۂ̏���Y���W�ɓd���̓������X�C�b�`���Ȃ����t���O
                            bool isCheck = false;

                            for (int y3 = 0; y3 < field.GetLength(0); y3++)
                            {
                                if (field[y3, x1] != null && field[y3, x1].tag == "Power" && field[y3, x1].GetComponent<PowerSpriteChange>().GetIsChange())
                                {
                                    // �D�揇�ʂ̔���
                                    if (field[y3, x1].GetComponent<PowerSpriteChange>().priority_ < field[y1, x2].GetComponent<PowerSpriteChange>().priority_)
                                    {
                                        isCheck = true;
                                        break;
                                    }
                                }
                            }

                            if (!isCheck)
                            {
                                field[y1, x1].GetComponent<FloorSpriteChange>().SetIsChangeReady(false);
                            }
                        }
                    }
                }
            }
        }

        // ��C�ɒʓd����������i�d����؂�ւ����t���[���̂݁j
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

                    // �D�揇�ʂ����炷
                    if (field[y1, x1] != null && field[y1, x1].tag == "Power" && !field[y1, x1].GetComponent<PowerSpriteChange>().isChangeClear_ && field[y1, x1].GetComponent<PowerSpriteChange>().GetIsChange())
                    {
                        for (int y2 = 0; y2 < field.GetLength(0); y2++)
                        {
                            for (int x2 = 0; x2 < field.GetLength(1); x2++)
                            {
                                if (field[y2, x2] != null && field[y2, x2].tag == "Power" && field[y2, x2].GetComponent<PowerSpriteChange>().isChangeClear_ && !field[y2, x2].GetComponent<PowerSpriteChange>().GetIsChange())
                                {
                                    if (field[y2, x2].GetComponent<PowerSpriteChange>().priority_ < field[y1, x1].GetComponent<PowerSpriteChange>().priority_)
                                    {
                                        field[y1, x1].GetComponent<PowerSpriteChange>().priority_ -= 1;
                                    }
                                }
                            }
                        }
                    }

                }
            }

            for (int y1 = 0; y1 < field.GetLength(0); y1++)
            {
                for (int x1 = 0; x1 < field.GetLength(1); x1++)
                {
                    if (field[y1, x1] != null && field[y1, x1].tag == "Power" && field[y1, x1].GetComponent<PowerSpriteChange>().isChangeClear_ && !field[y1, x1].GetComponent<PowerSpriteChange>().GetIsChange())
                    {
                        field[y1, x1].GetComponent<PowerSpriteChange>().priority_ = 100;
                    }
                }
            }
        }
    }

}
