using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelScript : MonoBehaviour
{
    public bool isNextLevelStart_;
    public bool isNextLevelClear_;
    public bool isStartLevelStart_;
    public bool isStartLevelClear_;
    private float alpha = 0.0f;
    private float lifeTime;
    private float leftLifeTime;

    SpriteRenderer fadeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        isNextLevelClear_ = false;
        isStartLevelClear_ = false;

        fadeMaterial = GetComponent<SpriteRenderer>();
        alpha = 0.0f;

        // ���ł���܂ł̎��Ԃ̐ݒ�
        lifeTime = 0.25f;
        // �c�莞�Ԃ�������
        leftLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        // ���W
        transform.localScale = new Vector3(32, 18, 0);

        // �t�F�[�h�C��
        if (isNextLevelStart_)
        {
            // �c�莞�Ԃ��J�E���g�_�E��
            leftLifeTime -= Time.deltaTime;

            // �t�F�[�h
            alpha = (lifeTime - leftLifeTime) / lifeTime;
            fadeMaterial.color = new Color(0.0f, 0.0f, 0.0f, alpha);

            // �c�莞�Ԃ�0�ȉ��ɂȂ�����
            if (leftLifeTime <= 0) { isNextLevelClear_ = true; }
        }

        // �t�F�[�h�A�E�g
        if (isStartLevelStart_)
        {
            // �c�莞�Ԃ��J�E���g�_�E��
            leftLifeTime -= Time.deltaTime;

            // �t�F�[�h
            alpha = leftLifeTime / lifeTime;
            fadeMaterial.color = new Color(0.0f, 0.0f, 0.0f, alpha);

            // �c�莞�Ԃ�0�ȉ��ɂȂ�����
            if (leftLifeTime <= 0) { isStartLevelClear_ = true; }
        }

    }
}
