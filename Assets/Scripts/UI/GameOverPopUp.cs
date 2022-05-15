using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUp : MonoBehaviour
{
    Button restartButton;
    private void Awake()
    {
        restartButton = GetComponent<Button>();
    }
    
}
