using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class MultipleDragging : MonoBehaviour
{
    //DRAGG AND PLACE
    [SerializeField] private GameObject redPlacedPrefab;
    [SerializeField] private GameObject bluePlacedPrefab;

    [SerializeField] private Camera arCamera;
    private List<Cube> placedObjects = new List<Cube>();
    private Vector2 touchPosition = default;
    private ARRaycastManager arRaycastManager;
    private bool onTouchHold = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Cube lastSelectedObject;
    
    //CUBES MOVEMENT
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float radius = 0.1f;

    private int teamsNumber = 0;
    [SerializeField] private Button redPlacedButton;
    [SerializeField] private Button bluePlacedButton;
    [SerializeField] private Button startFightButton;
    private bool isStartFight = false;
    [SerializeField] private Text startText;

    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        
        redPlacedButton.onClick.AddListener(placeRedCube);
        bluePlacedButton.onClick.AddListener(placeBlueCube);
        startFightButton.onClick.AddListener(changeFightPase);
    }

    private void changeFightPase()
    {
        if (isStartFight)
        {
            isStartFight = false;
            startText.text = "END";
        }
        else
        {
            isStartFight = true;
            startText.text = "START";
        }
    }

    private void placeRedCube()
    {
        teamsNumber = 0;
    }

    private void placeBlueCube()
    {
        teamsNumber = 1;
    }


    void Update()
    {
        //DRAGG AND PLACE
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    lastSelectedObject = hitObject.transform.GetComponent<Cube>();
                    if (lastSelectedObject != null)
                    {
                        foreach (Cube placementObject in placedObjects)
                        {
                            placementObject.Selected = placementObject == lastSelectedObject;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                lastSelectedObject.Selected = false;
            }
        }

        if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if(lastSelectedObject == null)
            {
                var vector = new Vector3(hitPose.position.x, hitPose.position.y + 0.1f, hitPose.position.z);

                switch (teamsNumber)
                {
                    case 0:
                        lastSelectedObject =
                            Instantiate(redPlacedPrefab, /*hitPose.position*/ vector, hitPose.rotation)
                                .GetComponent<Cube>();
                        placedObjects.Add(lastSelectedObject);
                        break;

                    case 1:
                        lastSelectedObject =
                            Instantiate(bluePlacedPrefab, /*hitPose.position*/ vector, hitPose.rotation)
                                .GetComponent<Cube>();
                        placedObjects.Add(lastSelectedObject);
                        break;

                    default:
                        break;
                }
            }
            else 
            {
                if(lastSelectedObject.Selected)
                {
                    var vector = new Vector3(hitPose.position.x, hitPose.position.y + 0.05f, hitPose.position.z);

                    lastSelectedObject.transform.position = vector;
                    lastSelectedObject.transform.rotation = hitPose.rotation;
                }
            } 
        }
            
        //CUBES MOVEMENT
        if (isStartFight)
        {
            foreach (var cube in placedObjects)
            {
                cube.move(placedObjects);

                if (cube.CurrentHealt <= 0)
                {
                    cube.Destroy();
                    placedObjects.Remove(cube);
                }
            }
        }
    }
}