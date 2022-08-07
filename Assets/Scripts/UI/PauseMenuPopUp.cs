using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPopUp : MonoBehaviour
{

    public void Show(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }
}
