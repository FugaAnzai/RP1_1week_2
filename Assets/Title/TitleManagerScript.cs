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
        //�X�y�[�X�������ꂽ��J��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeScene();
        }

    }

    //���C���V�[���ւ̑J��
    void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
