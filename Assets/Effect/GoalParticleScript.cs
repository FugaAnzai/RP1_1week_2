using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticleScript : MonoBehaviour
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

        // 消滅するまでの時間の設定
        lifeTime = 0.5f;
        // 残り時間を初期化
        leftLifeTime = lifeTime;
        // 現在のScaleを記録
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // 残り時間をカウントダウン
        leftLifeTime -= Time.deltaTime;

        // 大きくする
        transform.localScale = Vector3.Lerp
        (
            defaultScale,
            new Vector3(5, 5, 0),
            leftLifeTime / lifeTime
        );

        // 透明度を変える
        alpha = (lifeTime - leftLifeTime) / lifeTime;
        fadeMaterial.color = new Color(red, green, blue, alpha);

        // 残り時間が0以下になったらゲームオブジェクトを消滅
        if (leftLifeTime <= 0) { leftLifeTime = lifeTime; }
    }
}
