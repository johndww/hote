using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    const int LEFT_MOUSE_BUTTON = 0;
    const int RIGHT_MOUSE_BUTTON = 1;

    public enum Type { MOVE, SELECT, BUTTON1, BUTTON2, BUTTON3, BUTTON4, CHAR1, CHAR2, CHAR3, NONE }

    public static Type getPlayerInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.tapCount == 1)
                {
                    return Type.SELECT;
                }
                if (touch.tapCount == 2)
                {
                    return Type.MOVE;
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(LEFT_MOUSE_BUTTON))
            {
                return Type.SELECT;
            }
            else if (Input.GetMouseButton(RIGHT_MOUSE_BUTTON))
            {
                return Type.MOVE;
            }
        }
        return Type.NONE;
    }
}