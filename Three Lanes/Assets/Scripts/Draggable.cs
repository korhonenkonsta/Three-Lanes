using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	
	public Transform parentToReturnTo = null;
	public Transform placeholderParent = null;
    public new Camera camera;


    GameObject placeholder = null;

    public void Start()
    {
        camera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData) {
		//Debug.Log ("OnBeginDrag");
		
		placeholder = new GameObject();
		placeholder.transform.SetParent( this.transform.parent );
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex( this.transform.GetSiblingIndex() );
		
		parentToReturnTo = this.transform.parent;
		placeholderParent = parentToReturnTo;
		this.transform.SetParent( this.transform.parent.parent );
		
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag(PointerEventData eventData) {

        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f);
        }
        else
        {
            //Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            //Debug.Log("Layer: " + eventData.pointerCurrentRaycast.gameObject.layer);
            if (eventData.pointerCurrentRaycast.gameObject.layer != 5)
            {
                GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f);
            }
            else
            {
                GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
            }
        }
        //Debug.Log ("OnDrag");
        //if (GetComponent<Card>())
        //{
        //    GameManager.Instance.buildingToBuild = GetComponent<Card>().buildingPrefab;
        //}
		
		this.transform.position = eventData.position;

		if(placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent(placeholderParent);

		int newSiblingIndex = placeholderParent.childCount;

		for(int i=0; i < placeholderParent.childCount; i++) {
			if(this.transform.position.x < placeholderParent.GetChild(i).position.x) {

				newSiblingIndex = i;

				if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
					newSiblingIndex--;

				break;
			}
		}

		placeholder.transform.SetSiblingIndex(newSiblingIndex);

	}
	
	public void OnEndDrag(PointerEventData eventData) {

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            //print(objectHit.name);
            Card c = GetComponent<Card>();
            if (objectHit.GetComponent<BuildArea>() && c)
            {
                objectHit.GetComponent<BuildArea>().Build(c.buildingPrefab, c.buildingPrefab.GetComponent<Building>().cost, this, c);
            }

            // Do something with the object that was hit by the raycast.
        }
        //print("Object: "+eventData.pointerCurrentRaycast.gameObject.name);

        //Debug.Log ("OnEndDrag");
        this.transform.SetParent( parentToReturnTo );
		this.transform.SetSiblingIndex( placeholder.transform.GetSiblingIndex() );
		GetComponent<CanvasGroup>().blocksRaycasts = true;

		Destroy(placeholder);
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
    }
	
	
	
}
