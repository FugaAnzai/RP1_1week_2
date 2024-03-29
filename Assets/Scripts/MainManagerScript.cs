using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainManagerScript : MonoBehaviour
{

    //カメラのゲームオブジェクト
    public GameObject mainCamera_;
    public AudioClip selectSE;
    public AudioClip decideSE;

    //ステージセレクト時にカメラをLerp動かすときに使う
    private Vector3 startCamera_ = Vector3.zero;
    private Vector3 endCamera_ = Vector3.zero;
    public float cameraT_ = 0;
    private float elapsedTime = 0;
    private bool isMoveCamera_ = false;
    //ステージセレクトの値
    public static int stageSelect = 1;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    void Update()
    {
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
            //tが1になるまで線形補間
            if (cameraT_ < 1.0f)
            {
                cameraT_ += Time.deltaTime * 2.0f;
                mainCamera_.transform.position = Vector3.Lerp(startCamera_, endCamera_, cameraT_);
            }
            else
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
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isMoveCamera_ && stageSelect < 8)
        {
            this.GetComponent<AudioSource>().PlayOneShot(selectSE);
            //カメラ移動フラグをtrueに
            isMoveCamera_ = true;
            //ステージセレクト加算
            stageSelect++;
            //MoveCameraのstartとendを設定
            SetMoveCamera(mainCamera_.transform.position, mainCamera_.transform.position + new Vector3(4, 0, 0));
        }

        //ステージセレクト左へ移動
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isMoveCamera_ && stageSelect > 1)
        {
            this.GetComponent<AudioSource>().PlayOneShot(selectSE);
            //カメラ移動フラグをtrueに
            isMoveCamera_ = true;
            //ステージセレクト減算
            stageSelect--;
            //MoveCameraのstartとendを設定
            SetMoveCamera(mainCamera_.transform.position, mainCamera_.transform.position + new Vector3(-4, 0, 0));
        }

        //スペースが押されたらシーン遷移
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        this.GetComponent<AudioSource>().PlayOneShot(decideSE);

        yield return new WaitForSeconds(0.3f);

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

        if (stageSelect == 4)
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
