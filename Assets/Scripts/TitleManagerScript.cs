using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManagerScript : MonoBehaviour
{
    public AudioClip decide;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        //スペースが押されたら遷移
        if (Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed)
        {
            this.GetComponent<AudioSource>().PlayOneShot(decide);

            StartCoroutine(ChangeScene());
        }

    }

    //ステージセレクトへの遷移
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Stage1");
    }

}
