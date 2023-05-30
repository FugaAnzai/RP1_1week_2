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

        // 消滅するまでの時間の設定
        lifeTime = 0.5f;
        // 残り時間を初期化
        leftLifeTime = lifeTime;
        // 現在のScaleを記録
        defaultScale = transform.localScale;
        // 回転の方向をランダムで決める
        targetRotate = new Vector3(0, 0, Random.Range(-2.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        // 残り時間をカウントダウン
        leftLifeTime -= Time.deltaTime;

        // 大きくする
        transform.localScale = Vector3.Lerp
        (
            new Vector3(5, 5, 0),
            defaultScale,
            leftLifeTime / lifeTime
        );

        // 回転させる
        Vector3 rotate = Vector3.Lerp
        (
            targetRotate,
            new Vector3(0, 0, 0),
            leftLifeTime / lifeTime
        );
        transform.Rotate(rotate);

        // 透明度を変える
        alpha = leftLifeTime / lifeTime;
        fadeMaterial.color = new Color(red, green, blue, alpha);

        // 残り時間が0以下になったらゲームオブジェクトを消滅
        if (leftLifeTime <= 0) { Destroy(gameObject); }
    }
}
