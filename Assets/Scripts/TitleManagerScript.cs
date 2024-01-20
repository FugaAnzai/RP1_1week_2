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
        //スペースが押されたら遷移
        if (!isStartChangeScene && (Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed))
        {
            this.GetComponent<AudioSource>().PlayOneShot(decide);

            StartCoroutine(ChangeScene());

            isStartChangeScene = true;
        }

    }

    //ステージセレクトへの遷移
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Stage1");
    }

}
