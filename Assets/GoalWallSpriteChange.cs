using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class GoalWallSpriteChange : MonoBehaviour
{
    //電源オフ
    public Sprite offImage;
    //電源オン
    public Sprite onImage;
    //spriteRenderer
    SpriteRenderer spriteRenderer;
    // パーティクル
    public GameObject particlePrefab;

    private bool isChange_ = true;
    private bool isPreChange_ = true;

    // Start is called before the first frame update
    void Start()
    {
        isChange_ = true;
        isPreChange_ = isChange_;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        ChangeSprite();
    }

    public bool GetIsChange()
    {
        return isChange_;
    }

    public void SetIsChange(bool isChange)
    {
        isChange_ = isChange;
    }

    void ChangeSprite()
    {
        if (!isChange_)
        {
            spriteRenderer.sprite = offImage;

            if (isPreChange_)
            {
                GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity); ;
            }

        }

        if (isChange_)
        {
            spriteRenderer.sprite = onImage;

            if (!isPreChange_)
            {
                GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity); ;
            }

        }

        isPreChange_ = isChange_;
    }
}
