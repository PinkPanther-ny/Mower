using UnityEngine;
using UnityEngine.UI;


public class JoystickSetterExample : MonoBehaviour
{
    public FloatingJoystick verticalJoystick;
    public FloatingJoystick horizontalJoystick;
    public Text valueText;
    public Slider forwardSlider;
    public Slider rotateSlider;
    string action = "X01_01";

    private void Update()
    {
        if (verticalJoystick.Direction.y == 0 && horizontalJoystick.Direction.x == 0)
        {
            action = "X";
        }
        else
        {
            var rad = Mathf.Atan2(verticalJoystick.Direction.y, horizontalJoystick.Direction.x); // In radians
            if (-1 * Mathf.PI / 8 <= rad && rad < 1 * Mathf.PI / 8)
            {
                action = "D";
            }
            if (1 * Mathf.PI / 8 <= rad && rad < 3 * Mathf.PI / 8)
            {
                action = "E";
            }
            if (3 * Mathf.PI / 8 <= rad && rad < 5 * Mathf.PI / 8)
            {
                action = "W";
            }
            if (5 * Mathf.PI / 8 <= rad && rad < 7 * Mathf.PI / 8)
            {
                action = "Q";
            }
            if (7 * Mathf.PI / 8 <= rad || rad < -7 * Mathf.PI / 8)
            {
                action = "A";
            }
            if (-7 * Mathf.PI / 8 <= rad && rad < -5 * Mathf.PI / 8)
            {
                action = "Z";
            }
            if (-5 * Mathf.PI / 8 <= rad && rad < -3 * Mathf.PI / 8)
            {
                action = "S";
            }
            if (-3 * Mathf.PI / 8 <= rad && rad < -1 * Mathf.PI / 8)
            {
                action = "C";
            }
        }
        action += ((int)(forwardSlider.value * Mathf.Abs(verticalJoystick.Direction.y))).ToString() + "_" + ((int)(rotateSlider.value * Mathf.Abs(horizontalJoystick.Direction.x))).ToString();
        valueText.text = action;
    }
}