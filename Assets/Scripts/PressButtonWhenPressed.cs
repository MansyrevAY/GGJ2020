using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressButtonWhenPressed : MonoBehaviour
{

    public Image button;
    public KeyCode keyCode;
    public Color selectedColor;
    private Color defaultColor;

    // Start is called before the first frame update
    void Awake()
    {
        defaultColor = button.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(keyCode))
        {
            button.color = selectedColor;
        }
        else
        {
            button.color = defaultColor;
        }
    }
}
