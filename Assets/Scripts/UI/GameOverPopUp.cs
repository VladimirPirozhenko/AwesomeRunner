using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUp : MonoBehaviour
{
    public void Show(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }
}
