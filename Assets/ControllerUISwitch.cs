using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class ControllerUISwitch : MonoBehaviour
{
    private bool connected = false;
    [SerializeField] Sprite contorollerSprite;
    [SerializeField] Sprite keyboardSprite;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = contorollerSprite;
    }

    void Update()
    {
        // ������t���[�����ɐڑ�/�ؒf�������ɂ��Ă͖�����
        // �������̃Q�[���p�b�g�ł͓��얢����
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {

            GetComponent<SpriteRenderer>().sprite = keyboardSprite;

        }

        if (Gamepad.current.dpad.down.isPressed || Gamepad.current.dpad.up.isPressed || Gamepad.current.dpad.left.isPressed || Gamepad.current.dpad.right.isPressed || Gamepad.current.buttonSouth.isPressed)
        {
            GetComponent<SpriteRenderer>().sprite = contorollerSprite;
        }

    }

}
