using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
 {
	 public RectTransform toolTip;

	 void Update(){
		 toolTip.anchoredPosition = Input.mousePosition;
	 }
 }