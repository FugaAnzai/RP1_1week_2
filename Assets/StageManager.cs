using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    enum TILE_TYPE
    {
        NULL,
        PLAYER,
        WALL,
        POWER
    }

    TILE_TYPE[,] tileTable;
    GameObject[,] field;

    public GameObject playerPrafab_;
    public GameObject wallPrafab_;
    public GameObject powerPrefab_;
    public TextAsset stageFile;

    // Start is called before the first frame update
    void Start()
    {
        LoadTileData();
        CreateStage();
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    public void CreateStage()
    {
        field = new GameObject[tileTable.GetLength(0), tileTable.GetLength(1)];

        for (int y = 0; y < tileTable.GetLength(0); y++)
        {
            for(int x = 0; x < tileTable.GetLength(1); x++)
            {
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
    }

    public Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {

            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] != null && field[y, x].tag == "Player")
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

        //オブジェクトの座標を変更
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        //現在のプレイヤーオブジェクトを移動先に代入
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        //移動前のオブジェクトにnullを代入
        field[moveFrom.y, moveFrom.x] = null;
        return true;

    }

    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {

            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int move = new(1, 0);

            for (int y = 0; y < field.GetLength(0); y++)
            {

                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (field[y, x] != null && field[y, x].tag == "Player")
                    {
                        field[y, x].GetComponent<SpriteRenderer>().flipX = false;
                    }
                }
            }

            MoveObject("Player", playerIndex, playerIndex + move);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            Vector2Int move = new(1, 0);

            for (int y = 0; y < field.GetLength(0); y++)
            {

                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (field[y, x] != null && field[y, x].tag == "Player")
                    {
                        field[y,x].GetComponent<SpriteRenderer>().flipX = true;
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
            PowerChange();
        }

    }

    void PowerChange()
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
            field[upIndex.y, upIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[upIndex.y, upIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());
        }

        if (field[downIndex.y, downIndex.x] != null && field[downIndex.y, downIndex.x].tag == "Power")
        {
            field[downIndex.y, downIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[downIndex.y, downIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

        if (field[rightIndex.y, rightIndex.x] != null && field[rightIndex.y, rightIndex.x].tag == "Power")
        {
            field[rightIndex.y, rightIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[rightIndex.y, rightIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

        if (field[leftIndex.y, leftIndex.x] != null && field[leftIndex.y, leftIndex.x].tag == "Power")
        {
            field[leftIndex.y, leftIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[leftIndex.y, leftIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

        if (field[upRightIndex.y, upRightIndex.x] != null && field[upRightIndex.y, upRightIndex.x].tag == "Power")
        {
            field[upRightIndex.y, upRightIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[upRightIndex.y, upRightIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

        if (field[upLeftIndex.y, upLeftIndex.x] != null && field[upLeftIndex.y, upLeftIndex.x].tag == "Power")
        {
            field[upLeftIndex.y, upLeftIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[upLeftIndex.y, upLeftIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

        if (field[downRightIndex.y, downRightIndex.x] != null && field[downRightIndex.y, downRightIndex.x].tag == "Power")
        {
            field[downRightIndex.y, downRightIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[downRightIndex.y, downRightIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

        if (field[downLeftIndex.y, downLeftIndex.x] != null && field[downLeftIndex.y, downLeftIndex.x].tag == "Power")
        {
            field[downLeftIndex.y, downLeftIndex.x].GetComponent<PowerSpriteChange>().SetIsChange(!field[downLeftIndex.y, downLeftIndex.x].GetComponent<PowerSpriteChange>().GetIsChange());

        }

    }

}
