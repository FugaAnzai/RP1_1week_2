using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GoalWallParticleScript : MonoBehaviour
{
    private float red, green, blue, alpha;
    private float lifeTime;
    private float leftLifeTime;
    private Vector3 defaultScale;
    private Vector3 targetRotate;

    SpriteRenderer fadeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        fadeMaterial = GetComponent<SpriteRenderer>();
        red = fadeMaterial.color.r;
        green = fadeMaterial.color.g;
        blue = fadeMaterial.color.b;
        alpha = fadeMaterial.color.a;

        // ���ł���܂ł̎��Ԃ̐ݒ�
        lifeTime = 0.5f;
        // �c�莞�Ԃ�������
        leftLifeTime = lifeTime;
        // ���݂�Scale���L�^
        defaultScale = transform.localScale;
        // ��]�̕����������_���Ō��߂�
        targetRotate = new Vector3(0, 0, Random.Range(-2.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        // �c�莞�Ԃ��J�E���g�_�E��
        leftLifeTime -= Time.deltaTime;

        // �傫������
        transform.localScale = Vector3.Lerp
        (
            new Vector3(5, 5, 0),
            defaultScale,
            leftLifeTime / lifeTime
        );

        // ��]������
        Vector3 rotate = Vector3.Lerp
        (
            targetRotate,
            new Vector3(0, 0, 0),
            leftLifeTime / lifeTime
        );
        transform.Rotate(rotate);

        // �����x��ς���
        alpha = leftLifeTime / lifeTime;
        fadeMaterial.color = new Color(red, green, blue, alpha);

        // �c�莞�Ԃ�0�ȉ��ɂȂ�����Q�[���I�u�W�F�N�g������
        if (leftLifeTime <= 0) { Destroy(gameObject); }
    }
}
