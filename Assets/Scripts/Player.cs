using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private IPlayerInput input;
    private void Awake()
    {
        input = new KeyboardInput();
    }
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 10);
        transform.Translate(movement * 1.5f * Time.deltaTime);
        input.ReadInput();
        transform.Translate(input.Horizontal * Time.deltaTime, 0,0);
    }
}
