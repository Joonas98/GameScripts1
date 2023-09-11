using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacing : MonoBehaviour
{
    [Header("Variables")]
    public float rotationSpeed;

    [System.Serializable]
    public struct PlacingInfo
    {
        public GameObject aimerPrefab;
        public GameObject prefab;

        public float minAllowedAngle;
        public float maxAllowedAngle;
    }

    public List<PlacingInfo> placingObjects;
    public KeyCode placingModeHotkey, choosingMenuHotkey;
    public Color validPlacementColor, invalidPlacementColor;
    public Color defaultColor, highlightColor;
    [HideInInspector] public static ObjectPlacing instance;
    public bool isPlacing = false;
    public bool isChoosingObject = false;
    public GameObject choosingMenu;
    public Image[] objectPanels;
    public float objectSelectionTimeSlow;

    private RaycastHit hit;
    private GameObject activeAimer;
    private GameObject placingObject;
    private Vector3 initialAimerPosition;
    private int chosenObjectIndex = 0;
    private bool isRotating = false; // Flag to check if the player is currently rotating the object.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
        ChooseObject(0);
    }

    void Update()
    {
        HandleInputs();
        HandlePlacement(placingObjects[chosenObjectIndex]);
    }

    public void ChooseObject(int index)
    {
        objectPanels[chosenObjectIndex].color = defaultColor;
        objectPanels[index].color = highlightColor;
        chosenObjectIndex = index;
    }

    private void HandleInputs()
    {
        if (Time.timeScale <= 0) return; // Game paused

        // Open object selection menu
        if (Input.GetKey(choosingMenuHotkey) && !GrenadeThrow.instance.selectingGrenade)
        {
            if (!isChoosingObject)
            {
                choosingMenu.SetActive(true);
                isChoosingObject = true;
                MouseLook.instance.canRotate = false;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = objectSelectionTimeSlow;
                if (isPlacing) StopPlacing();
            }
        }
        else // Close object selection menu
        {
            if (isChoosingObject)
            {
                choosingMenu.SetActive(false);
                isChoosingObject = false;
                MouseLook.instance.canRotate = true;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            }
        }

        // Handle object placement
        if (Input.GetKeyDown(placingModeHotkey))
        {
            if (!isPlacing)
            {
                StartPlacing(placingObjects[chosenObjectIndex]);  // Use the chosen object index
            }
            else
            {
                StopPlacing();
            }
        }

        // // Rotating objects that player is about to place
        // // Check for right mouse button click to start rotating.
        // if (Input.GetMouseButtonDown(1))
        // {
        //     isRotating = true;
        // }
        //
        // // Check for right mouse button release to stop rotating.
        // if (Input.GetMouseButtonUp(1))
        // {
        //     isRotating = false;
        // }
        //
        // // Rotate the object if the player is holding the right mouse button.
        // if (isRotating)
        // {
        //     float rotationSpeed = 50.0f;  // Adjust the rotation speed as needed.
        //     float mouseX = Input.GetAxis("Mouse X");
        //
        //     // Rotate the activeAimer around its up axis based on mouse input.
        //     activeAimer.transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        // }

    }

    private void StartPlacing(PlacingInfo placingInfo)
    {
        if (placingInfo.aimerPrefab == null)
        {
            Debug.LogError("Aimer Prefab is null for the chosen object!");
            return;
        }

        activeAimer = Instantiate(placingInfo.aimerPrefab, Vector3.zero, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0));
        isPlacing = true;
        placingObject = placingInfo.prefab;
        initialAimerPosition = activeAimer.transform.position;
    }

    private void StopPlacing()
    {
        if (activeAimer != null) Destroy(activeAimer);
        isPlacing = false;
        placingObject = null;
        MouseLook.instance.canRotate = true; // Bug prevention
    }

    private void HandlePlacement(PlacingInfo placingInfo)
    {
        if (activeAimer == null) return;

        Renderer renderer = activeAimer.GetComponent<Renderer>();

        if (renderer == null)
        {
            // Try to find a Renderer in children
            renderer = activeAimer.GetComponentInChildren<Renderer>();
        }

        if (renderer != null)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f))
            {
                Vector3 aimerPosition = hit.point;
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle >= placingInfo.minAllowedAngle && angle <= placingInfo.maxAllowedAngle)
                {
                    renderer.material.color = validPlacementColor;
                    aimerPosition = hit.point;

                    // Rotate the aimed object with right mouse button
                    if (Input.GetMouseButton(1))
                    {
                        float mouseX = Input.GetAxis("Mouse X");
                        activeAimer.transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
                        MouseLook.instance.canRotate = false;
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        MouseLook.instance.canRotate = true;
                    }

                    // activeAimer.transform.SetPositionAndRotation(aimerPosition, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0));
                    activeAimer.transform.position = (aimerPosition);

                    // Place the aimed object when left clicking
                    if (Input.GetMouseButtonDown(0))
                    {
                        _ = Instantiate(placingInfo.prefab, activeAimer.transform.position, activeAimer.transform.rotation);
                    }
                }
                else
                {
                    renderer.material.color = invalidPlacementColor;
                    aimerPosition = initialAimerPosition;
                    activeAimer.transform.SetPositionAndRotation(aimerPosition, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0));
                }
            }
        }
    }

}
