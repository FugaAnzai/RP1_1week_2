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

        // 消滅するまでの時間の設定
        lifeTime = 0.25f;
        // 残り時間を初期化
        leftLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        // 座標
        transform.localScale = new Vector3(32, 18, 0);

        // フェードイン
        if (isNextLevelStart_)
        {
            // 残り時間をカウントダウン
            leftLifeTime -= Time.deltaTime;

            // フェード
            alpha = (lifeTime - leftLifeTime) / lifeTime;
            fadeMaterial.color = new Color(0.0f, 0.0f, 0.0f, alpha);

            // 残り時間が0以下になったら
            if (leftLifeTime <= 0) { isNextLevelClear_ = true; }
        }

        // フェードアウト
        if (isStartLevelStart_)
        {
            // 残り時間をカウントダウン
            leftLifeTime -= Time.deltaTime;

            // フェード
            alpha = leftLifeTime / lifeTime;
            fadeMaterial.color = new Color(0.0f, 0.0f, 0.0f, alpha);

            // 残り時間が0以下になったら
            if (leftLifeTime <= 0) { isStartLevelClear_ = true; }
        }

    }
}
