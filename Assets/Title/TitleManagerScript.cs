using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

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
        //�X�y�[�X�������ꂽ��J��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<AudioSource>().PlayOneShot(decide);

            StartCoroutine(ChangeScene());
        }

    }

    //���C���V�[���ւ̑J��
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("MainScene");
    }

}
