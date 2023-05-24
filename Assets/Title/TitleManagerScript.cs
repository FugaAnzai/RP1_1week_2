using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //スペースが押されたら遷移
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeScene();
        }

    }

    //メインシーンへの遷移
    void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
