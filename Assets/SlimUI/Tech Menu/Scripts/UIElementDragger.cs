using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
// Fromt Unity Drag and Drop Tutorial - How TO Drag and Drop UI Elements in Unity

public class UIElementDragger : MonoBehaviour {

	public const string DRAGGABLE_TAG = "UIDrag";
	private bool dragging = false;
	private Vector2 originalPosition;
	private Transform objectToDrag;
	private Image objecttoDragImage;

	List<RaycastResult> hitObjects = new List<RaycastResult>();

	#region Monobehavior API

	void Update(){
		if(Input.GetButtonDown("Fire")){
			objectToDrag = GetDraggableTransformUnderMouse();

			if(objectToDrag != null){
				dragging = true;

				originalPosition = objectToDrag.position;
				objecttoDragImage = objectToDrag.GetComponent<Image>();
				objecttoDragImage.raycastTarget = false;
			}
		}

		if(dragging){
			objectToDrag.position = Input.mousePosition;
		}

		if(Input.GetButtonUp("Fire") && dragging){
			dragging = false;
			objecttoDragImage.raycastTarget = true;
		}
	}

	#endregion

	private GameObject GetObjectUnderMouse(){
		var pointer = new PointerEventData(EventSystem.current);

		pointer.position = Input.mousePosition;
		EventSystem.current.RaycastAll(pointer,hitObjects);

		if(hitObjects.Count <= 0) return null;

		return hitObjects.First().gameObject;
	}

	private Transform GetDraggableTransformUnderMouse(){
		GameObject clickedObject = GetObjectUnderMouse();

		if(clickedObject != null && clickedObject.tag == DRAGGABLE_TAG){
			return clickedObject.transform;
		}

		return null;
	}
}
