using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainManagerScript : MonoBehaviour
{

    //�J�����̃Q�[���I�u�W�F�N�g
    public GameObject mainCamera_;
    public AudioClip selectSE;
    public AudioClip decideSE;

    //�X�e�[�W�Z���N�g���ɃJ������Lerp�������Ƃ��Ɏg��
    private Vector3 startCamera_ = Vector3.zero;
    private Vector3 endCamera_ = Vector3.zero;
    public float cameraT_ = 0;
    private float elapsedTime = 0;
    private bool isMoveCamera_ = false;
    //�X�e�[�W�Z���N�g�̒l
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
        //�t���O��true�ɂ���ƈړ��J�n
        if (isMoveCamera_)
        {
            //t��1�ɂȂ�܂Ő��`���
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

    //MoveCamera��Lerp�Ɏg���l���Z�b�g
    private void SetMoveCamera(Vector3 startCamera, Vector3 endCamera)
    {
        startCamera_ = startCamera;
        endCamera_ = endCamera;
    }

    private void SelectStage()
    {
        //�X�e�[�W�Z���N�g�E�ֈړ�
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isMoveCamera_ && stageSelect < 8)
        {
            this.GetComponent<AudioSource>().PlayOneShot(selectSE);
            //�J�����ړ��t���O��true��
            isMoveCamera_ = true;
            //�X�e�[�W�Z���N�g���Z
            stageSelect++;
            //MoveCamera��start��end��ݒ�
            SetMoveCamera(mainCamera_.transform.position, mainCamera_.transform.position + new Vector3(4, 0, 0));
        }

        //�X�e�[�W�Z���N�g���ֈړ�
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isMoveCamera_ && stageSelect > 1)
        {
            this.GetComponent<AudioSource>().PlayOneShot(selectSE);
            //�J�����ړ��t���O��true��
            isMoveCamera_ = true;
            //�X�e�[�W�Z���N�g���Z
            stageSelect--;
            //MoveCamera��start��end��ݒ�
            SetMoveCamera(mainCamera_.transform.position, mainCamera_.transform.position + new Vector3(-4, 0, 0));
        }

        //�X�y�[�X�������ꂽ��V�[���J��
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