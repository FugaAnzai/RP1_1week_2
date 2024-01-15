using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpriteChange : MonoBehaviour
{
    //電源オフ
    public Sprite offImage;
    //電源オン
    public Sprite onImage;
    //spriteRenderer
    SpriteRenderer spriteRenderer;
    // パーティクル
    public GameObject particlePrefab;

    private bool isChange_ = false;
    private bool isPreChange_ = false;
    private bool isChangeReady_ = false;
    public int[] number_;

    // Start is called before the first frame update
    void Start()
    {
        isChange_ = false;
        isPreChange_ = isChange_;
        isChangeReady_ = false;
        number_ = new int[8];
        for (int i = 0; i < 8; i++)
        {
            number_[i] = 0;
        }
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

    public void SetIsChange()
    {
        isChange_ = isChangeReady_;
    }

    public void SetIsChangeReady(bool isChangeReady)
    {
        isChangeReady_ = isChangeReady;
    }

    void ChangeSprite()
    {
        if (!isChange_)
        {
            spriteRenderer.sprite = offImage;
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
