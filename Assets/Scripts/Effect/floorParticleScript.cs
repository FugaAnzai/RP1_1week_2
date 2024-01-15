using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorParticleScript : MonoBehaviour
{
    private float red, green, blue, alpha;
    private float lifeTime;
    private float leftLifeTime;
    private Vector3 defaultScale;

    SpriteRenderer fadeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        fadeMaterial = GetComponent<SpriteRenderer>();
        red = fadeMaterial.color.r;
        green = fadeMaterial.color.g;
        blue = 0.0f;
        alpha = fadeMaterial.color.a;

        // ���ł���܂ł̎��Ԃ̐ݒ�
        lifeTime = 0.3f;
        // �c�莞�Ԃ�������
        leftLifeTime = lifeTime;
        // ���݂�Scale���L�^
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // �c�莞�Ԃ��J�E���g�_�E��
        leftLifeTime -= Time.deltaTime;

        // �傫������
        transform.localScale = Vector3.Lerp
        (
            new Vector3(2, 2, 0),
            defaultScale,
            leftLifeTime / lifeTime
        );

        // �����x��ς���
        alpha = leftLifeTime / lifeTime;
        fadeMaterial.color = new Color(red, green, blue, alpha);

        // �c�莞�Ԃ�0�ȉ��ɂȂ�����Q�[���I�u�W�F�N�g������
        if (leftLifeTime <= 0) { Destroy(gameObject); }
    }
}
