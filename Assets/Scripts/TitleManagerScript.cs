using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManagerScript : MonoBehaviour
{
    public AudioClip decide;

    private bool isStartChangeScene = false;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //�X�y�[�X�������ꂽ��J��
        if (!isStartChangeScene && (Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed))
        {
            this.GetComponent<AudioSource>().PlayOneShot(decide);

            StartCoroutine(ChangeScene());

            isStartChangeScene = true;
        }

    }

    //�X�e�[�W�Z���N�g�ւ̑J��
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Stage1");
    }

}
