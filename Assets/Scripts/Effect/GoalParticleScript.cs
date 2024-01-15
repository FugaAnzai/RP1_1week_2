using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticleScript : MonoBehaviour
{
    private float red, green, blue, alpha;
    private float lifeTime;
    private float leftLifeTime;

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
        lifeTime = 0.8f;
        // 残り時間を初期化
        leftLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        // 残り時間をカウントダウン
        leftLifeTime -= Time.deltaTime;

        // 大きくする
        transform.localScale = Vector3.Lerp
        (
            new Vector3(0.8f, 0.8f, 0),
            new Vector3(5, 5, 0),
            leftLifeTime * leftLifeTime
        );

        // 透明度を変える
        alpha = lifeTime - leftLifeTime * leftLifeTime;
        fadeMaterial.color = new Color(red, green, blue, alpha);

        // 残り時間が0以下になったらゲームオブジェクトを消滅
        if (leftLifeTime <= 0.01f) { leftLifeTime = lifeTime; }
    }
}
