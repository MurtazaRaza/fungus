using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour {


	[Tooltip("Is it possible to interact with this object?")]
	public bool interactionEnabled = true;

	[Range(0f, 100f)]
	public float distance = 5;

	public float maxGazeTime = 3;

	public float outlineWidth = 0.005f;

	public float outlineGrowSpeed = 3f;

	/*Private variables*/
	private Collider myCollider;

	private Transform playerTransform;

	private EventTrigger myTrigger;

	private GameObject outlineObject;

	private MeshRenderer outlineRenderer;

	private Material outlineMaterial;

	private int outlineStatus;

	private bool disableOnOutlineDestroy;

	private float outlineLerp;

	private float currentOutlineWidth;

	private float timer = 0;

	private bool isGazedAt = false;

    
	// Use this for initialization
	void Start () {
        try
        {
            outlineMaterial = Resources.Load("Material/OutlineOnly") as Material;
            Debug.Log("Material loaded");
        }
        catch
        {
            Debug.Log("Material could not be loaded");
        }

		myCollider = gameObject.GetComponent<Collider> ();

		playerTransform = Camera.main.transform;

		myTrigger = gameObject.GetComponent<EventTrigger> ();

		if (myTrigger == null) {
			myTrigger = gameObject.AddComponent<EventTrigger> ();
		}

		//Register Pointer enter
		EventTrigger.Entry entryOver = new EventTrigger.Entry();
		entryOver.eventID = EventTriggerType.PointerEnter;
		entryOver.callback.AddListener ((eventData) => {
			OnPointerEnter ();
		});
		myTrigger.triggers.Add (entryOver);

		//Register pointer exit
		EventTrigger.Entry entryOut = new EventTrigger.Entry();
		entryOver.eventID = EventTriggerType.PointerExit;
		entryOver.callback.AddListener ((eventData) => {
			OnPointerExit ();
		});
		myTrigger.triggers.Add (entryOut);

		//Register pointer click (physical button pressed and releeased)
		EventTrigger.Entry entryClick = new EventTrigger.Entry();
		entryOver.eventID = EventTriggerType.PointerClick;
		entryOver.callback.AddListener ((eventData) => {
			OnPointerClick ();
		});
		myTrigger.triggers.Add (entryClick);

		//Register pointer up (physical button released)
		EventTrigger.Entry entryUp = new EventTrigger.Entry();
		entryOver.eventID = EventTriggerType.PointerUp;
		entryOver.callback.AddListener ((eventData) => {
			OnPointerUp ();
		});
		myTrigger.triggers.Add (entryUp);

		//Register pointer up (physical button released)
		EventTrigger.Entry entryDown = new EventTrigger.Entry();
		entryOver.eventID = EventTriggerType.PointerDown;
		entryOver.callback.AddListener ((eventData) => {
			OnPointerDown ();
		});
		myTrigger.triggers.Add (entryDown);
	
	}

    
    // Update is called once per frame
    void Update () {

		//if interaction is not enabled for this item
		if (interactionEnabled == false) {
			//just stop
			return;
		} else if (outlineStatus == 1) {
			//if the lerp is not at max level
			outlineLerp += Time.deltaTime * outlineGrowSpeed;
			//Calculate new outline width
			currentOutlineWidth = Mathf.Lerp (0f, outlineWidth, outlineLerp);
			//set the outline width on the material
			outlineRenderer.material.SetFloat ("_Outline", currentOutlineWidth);
		} else if (outlineStatus == 2) {
			//if the lerp value is not at 0
			if (outlineLerp > 0f) {
				outlineLerp += Time.deltaTime * outlineGrowSpeed;

				//Calculate new outline width
				currentOutlineWidth = Mathf.Lerp (0f, outlineWidth, outlineLerp);
				//set the outline width on the material
				outlineRenderer.material.SetFloat ("_Outline", currentOutlineWidth);
			} else if (outlineLerp <= 0f) {
				GameObject.Destroy (outlineObject);
				outlineStatus = 0;

				//check if we should disable interaction when the outline fades away
				if (disableOnOutlineDestroy == true) {
					interactionEnabled = false;

					disableOnOutlineDestroy = false;
				}
			
			}
		
		}

		if (isGazedAt) {
			timer += Time.deltaTime;

			if (timer > maxGazeTime) {
				timer = 0f;

				ExecuteEvents.Execute (gameObject, new PointerEventData (EventSystem.current), ExecuteEvents.pointerDownHandler);
				ExecuteEvents.Execute (gameObject, new PointerEventData (EventSystem.current), ExecuteEvents.pointerUpHandler);
				DisableInteraction ();
			}
		
		} else if (timer != 0f) {
			timer -= Time.deltaTime;

			if (timer < 0f) {
				timer = 0f;
			
			}
		}
	}

    /*EventTrigger Events here*/
    public void OnPointerEnter()
    {
        //set the outline status to 1: Gazed at (Grow)
        outlineStatus = 1;

        //spawn an outline gameobject
        SpawnOutlineChild();

        //Spawn "Walk to button"
        //SpawnButtonA();

        isGazedAt = true;
    }

    //Pointer Exit Event
    public void OnPointerExit()
    {
        //Set the outline to 1: Gazed at (Grow)
        outlineStatus = 2;

        isGazedAt = false;
    }
    
    public void OnPointerClick()
    {

    }

    public void OnPointerUp()
    {

    }
    public void OnPointerDown()
    {

    }

    /*Custom Functions*/

    private void SpawnOutlineChild()
    {
        Debug.Log("Ive reached the outline function");
        //if there is already an outlined object
        if(outlineObject != null)
        {
            GameObject.Destroy(outlineObject);
        }

        //Create an empty gameobject
        outlineObject = new GameObject(gameObject.name + "_Outline");

        //make it my child
        outlineObject.transform.SetParent(transform);

        //set position to my position
        outlineObject.transform.localPosition = Vector3.zero;

        //set rotation to parents rotation
        outlineObject.transform.rotation = Quaternion.identity;

        //set rotation to parents scale
        outlineObject.transform.localScale = Vector3.one;

        //add a meshrenderer to the new gameobject
        outlineRenderer = outlineObject.AddComponent<MeshRenderer>();

        //add a meshfilter
        MeshFilter outlineFilter = outlineObject.AddComponent<MeshFilter>();

        //Set the mesh to the same as mine
        outlineFilter.mesh = (Mesh)gameObject.GetComponent<MeshFilter>().mesh;

        //Set the material to the outline material loaded from a resources folder
        outlineRenderer.material = outlineMaterial;

        //Set outline width to 0
        outlineRenderer.material.SetFloat("_Outline", 0f);

        //Set the outline lerp value to 0 so update lerps back to 1
        outlineLerp = 0;

    }

    //Functuon to disable interaction to this object
    public void DisableInteraction()
    {
        outlineStatus = 2;
        disableOnOutlineDestroy = true;
        myCollider.enabled = false;
    }


}
