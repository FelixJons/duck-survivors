using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print($"Width = {Screen.width}, Height = {Screen.height}");
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        //print(mousePosition);
    }
}
