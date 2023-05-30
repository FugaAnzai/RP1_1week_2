using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainManagerScript : MonoBehaviour
{

    //カメラのゲームオブジェクト
    public GameObject mainCamera_;

    //ステージセレクト時にカメラをLerp動かすときに使う
    private Vector3 startCamera_ = Vector3.zero;
    private Vector3 endCamera_ = Vector3.zero;
    private float cameraT_ = 0;
    private bool isMoveCamera_ = false;
    //ステージセレクトの値
    public int stageSelect = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();

        SelectStage();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    private void MoveCamera()
    {
        //フラグをtrueにすると移動開始
        if (isMoveCamera_)
        {
            //t加算
            cameraT_ += 0.01f;

            //tが1になるまで線形補間
            if (cameraT_ <= 1.0f)
            {
                mainCamera_.transform.position = Vector3.Lerp(startCamera_, endCamera_, cameraT_);
            }

            //tが1以上になったらフラグをfalseに、tを0に
            if (cameraT_ > 1.0f)
            {
                cameraT_ = 0.0f;
                isMoveCamera_ = false;
            }
        }

    }

    //MoveCameraのLerpに使う値をセット
    private void SetMoveCamera(Vector3 startCamera, Vector3 endCamera)
    {
        startCamera_ = startCamera;
        endCamera_ = endCamera;
    }

    private void SelectStage()
    {
        //ステージセレクト右へ移動
        if (Input.GetKeyDown(KeyCode.D) && !isMoveCamera_)
        {
            //カメラ移動フラグをtrueに
            isMoveCamera_ = true;
            //ステージセレクト加算
            stageSelect++;
            //MoveCameraのstartとendを設定
            SetMoveCamera(mainCamera_.transform.position, mainCamera_.transform.position + new Vector3(4, 0, 0));
        }

        //ステージセレクト左へ移動
        if (Input.GetKeyDown(KeyCode.A) && !isMoveCamera_ && stageSelect > 1)
        {
            //カメラ移動フラグをtrueに
            isMoveCamera_ = true;
            //ステージセレクト減算
            stageSelect--;
            //MoveCameraのstartとendを設定
            SetMoveCamera(mainCamera_.transform.position, mainCamera_.transform.position + new Vector3(-4, 0, 0));
        }

        //1以下にならないように
        if (stageSelect <= 1)
        {
            stageSelect = 1;
        }

        //スペースが押されたらシーン遷移
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stageSelect == 1)
            {
                SceneManager.LoadScene("Stage1");
            }

            if (stageSelect == 2)
            {
                SceneManager.LoadScene("Stage2");
            }

            if (stageSelect == 3)
            {
                SceneManager.LoadScene("Stage3");
            }

            if(stageSelect == 4)
            {
                SceneManager.LoadScene("Stage4");
            }

            if (stageSelect == 5)
            {
                SceneManager.LoadScene("Stage5");
            }

            if (stageSelect == 6)
            {
                SceneManager.LoadScene("Stage6");
            }

            if (stageSelect == 7)
            {
                SceneManager.LoadScene("Stage7");
            }

            if (stageSelect == 8)
            {
                SceneManager.LoadScene("Stage8");
            }

        }
    }

}
