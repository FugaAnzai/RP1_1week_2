using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class TutroialManagerScript : MonoBehaviour
{

    private bool isTutorial1 = true;
    private bool isTutorial2 = false;
    [SerializeField] GameObject tutorial1;
    [SerializeField] GameObject tutorial2;
    [SerializeField] GameObject tutorial3;
    [SerializeField] Vector3 UIpos;
    private int tutorial1Progress;

    // Start is called before the first frame update
    void Start()
    {
        tutorial1.transform.DOMoveY(UIpos.y, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
            || Gamepad.current.dpad.down.isPressed || Gamepad.current.dpad.up.isPressed || Gamepad.current.dpad.left.isPressed || Gamepad.current.dpad.right.isPressed) {

            tutorial1Progress += 10;

        }

        if(tutorial1Progress >= 80 && isTutorial1)
        {
            isTutorial1 = false;
            isTutorial2 = true;
            tutorial1.transform.DOMoveY(-5, 0.8f);
            tutorial2.transform.DOMoveY(UIpos.y, 0.8f);
        }

        if(isTutorial2 && Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed)
        {
            tutorial2.transform.DOMoveY(-5, 0.8f);
            tutorial3.transform.DOMoveY(UIpos.y, 0.8f);
        }

    }
}
