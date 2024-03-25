// Copyright 2023 Niantic, Inc. All Rights Reserved.

using System.Collections;
using UnityEngine;
using Unity.Netcode;
using Niantic.Lightship.SharedAR.Colocalization;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Niantic.Lightship.SharedAR.Netcode;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System.Linq;

namespace Niantic.Lightship.AR.Samples
{
    public class ImageTrackingColocalizationDemo : NetworkBehaviour
    {
        public static ImageTrackingColocalizationDemo Instance { get; private set; }

        [SerializeField]
        private ARSession _arSession;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _placementObject;

        [SerializeField]
        private SharedSpaceManager _sharedSpaceManager;

        [SerializeField]
        private GameObject _netButtonsPanel;

        [SerializeField]
        private GameObject _buttonPanel;

        [SerializeField]
        private GameObject _endPanel;

        [SerializeField]
        private GameObject _targetImageInstructionPanel;

        [SerializeField]
        private Text _endPanelText;

        [SerializeField]
        private GameObject _sharedRootMarkerPrefab;

        [SerializeField]
        private InputField _roomNameInputField;

        [SerializeField]
        private Text _roomNameDisplayText;

        [SerializeField]
        private Texture2D _targetImage;

        [SerializeField]
        private float _targetImageSize;

        [SerializeField]
        private Button _enableMeshManagerButton;

        [SerializeField]
        private ARMeshManager _arMeshManager;

        private string _roomName;

        private bool _startAsHost;

        [SerializeField]
        private GameObject _cube1;

        [SerializeField]
        private GameObject _cube2;

        [SerializeField]
        private GameObject _cube3;

        [SerializeField]
        private GameObject _chairPrefab;

        [SerializeField]
        private GameObject _tablePrefab;

        [SerializeField]
        private GameObject _wallPrefab;

        private Button chairButton;

        private Button tableButton;

        private Button wallButton;

        private Button doneButton;

        private Button deleteButton;

        private List<GameObject> addedPrefabs = new List<GameObject>();

        private GameObject currentlySelectedObject;

        [SerializeField]
        public Material originalMaterial;

        [SerializeField]
        public Material selectedMaterial;

        private Vector2 initialTouch1Pos;

        private Vector2 initialTouch2Pos;

        private float initialRotationAngle;

        [SerializeField]
        private Slider scaleSliderY;

        [SerializeField]
        private Slider scaleSliderZ;

        [SerializeField]
        private GameObject sliderPanel;

        private logic placementIndicator;

        private string clientSelectedObject = "server";

        void Awake()
        {
            Instance = this;
            _enableMeshManagerButton.onClick.AddListener(EnableMeshManager);

            currentlySelectedObject = null;
        }

        void Start()
        {
            _sharedSpaceManager.sharedSpaceManagerStateChanged += OnColocalizationTrackingStateChanged;
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

            Debug.Log("Starting Image Tracking Colocalization");
            _netButtonsPanel.SetActive(true);
            _roomNameDisplayText.gameObject.SetActive(false);
            _targetImageInstructionPanel.SetActive(false);
            _buttonPanel.SetActive(false);
            sliderPanel.SetActive(false);

            // placementIndicator = FindObjectOfType<logic>();
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.IsPointerOverGameObject()) {
                    return;
                }
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    GameObject placedObject = Instantiate(_placementObject, hit.point, Quaternion.identity);
                    placedObject.GetComponent<NetworkObject>().Spawn();
                }
            }
#endif
#if UNITY_ANDROID
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = touch.position;
                    List<RaycastResult> UIHits = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, UIHits);

                    if (UIHits.Count > 0)
                    {
                        // Debug.Log("Hit a UI Element");
                        return;
                    }

                    if (currentlySelectedObject != null)
                    {
                        Debug.Log("An object is selected!");
                        return;
                    }

                    Ray ray = _camera.ScreenPointToRay(touch.position);

                    Debug.Log("TOUCH | X: " + touch.position.x + " Y: " + touch.position.y);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("Hit Object name: " + hit.transform.gameObject.name);

                        if (hit.transform.gameObject.CompareTag("chairObject") || hit.transform.gameObject.CompareTag("tableObject") || hit.transform.gameObject.CompareTag("wallObject"))
                        {
                            Debug.Log("Hit Object's Tag: " + hit.transform.gameObject.tag);

                            currentlySelectedObject = hit.transform.gameObject;
                            currentlySelectedObject.GetComponent<MeshRenderer>().material = selectedMaterial;

                            if (hit.transform.gameObject.CompareTag("wallObject"))
                            {
                                sliderPanel.SetActive(true);
                                scaleSliderY.onValueChanged.AddListener(ScaleChangedY);
                                scaleSliderZ.onValueChanged.AddListener(ScaleChangedZ);

                                float scaleZ = currentlySelectedObject.transform.parent.localScale.x / 2f;
                                float scaleX = currentlySelectedObject.transform.parent.localScale.y / 2f;
                                scaleSliderY.value = scaleX;
                                scaleSliderZ.value = scaleZ;
                            }
                        }
                        else
                        {
                            Debug.Log("HIT NAME: " + hit.transform.gameObject.name + ", POSITION: " + hit.transform.gameObject.transform.position);
                            Debug.Log("- - - HIT - POINT - - -: X: " + hit.point.x + " Y: " + hit.point.y + " Z: " + hit.point.z);

                            //GameObject localObject = Instantiate(_cube1, new Vector3(-4f, 0f, 1f), Quaternion.identity);

                            //GameObject networkObject = Instantiate(_cube2, new Vector3(-4f, 0f, 1f), Quaternion.identity);
                            //networkObject.GetComponent<NetworkObject>().Spawn();

                            //CubeScript placedObjectBehavior = _placementObject.GetComponent<CubeScript>();
                            //placedObjectBehavior.SpawnObject(hit.point);

                            //GameObject placedObject = Instantiate(_placementObject);
                            //NetworkObject placedNetworkObject = placedObject.GetComponent<NetworkObject>();
                            //placedNetworkObject.SetPosition(hit.point);

                            //GameObject placedObject = Instantiate(_placementObject, hit.point, Quaternion.identity);

                            if (IsServer)
                            {
                                PlaceObjectServer(hit.point, clientSelectedObject);
                            }
                            else
                            {
                                PlaceObjectServerRpc(hit.point, clientSelectedObject);
                            }
                            //GameObject placedObject = Instantiate(NetworkManager.Singleton.GetNetworkPrefabOverride(_placementObject), hit.point, Quaternion.identity);

                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.position);
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.TransformPoint(placedObject.transform.position));
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.InverseTransformPoint(placedObject.transform.position));
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.forward);
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.right);
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.up);
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.root.transform.root.name);
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.localPosition);
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.localToWorldMatrix.ToPosition());
                            //Debug.Log("TRANSFORM -V mono: " + placedObject.transform.worldToLocalMatrix.ToPosition());

                            //Debug.Log("PLACED | X: " + placedObject.transform.position.x + " Y: " + placedObject.transform.position.y + " Z: " + placedObject.transform.position.z);
                            //NetworkObject placedNetworkObject = placedObject.GetComponent<NetworkObject>();
                            //placedNetworkObject.Spawn();

                            //Debug.Log("NetObj Transform mono: " + placedNetworkObject.transform.position);

                            //placedNetworkObject.TrySetParent(base.gameObject.transform, worldPositionStays: false);
                            //placedNetworkObject.TryRemoveParent(worldPositionStays: false);

                            //addedPrefabs.Add(placedObject);
                            //Debug.Log("placedObject Instance ID: " + placedObject.GetInstanceID());
                            //Debug.Log("list item Instance ID: " + addedPrefabs.Last<GameObject>().GetInstanceID());
                            //Debug.Log("Length of addedPrefabs list: " + addedPrefabs.Count);
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    // Debug.Log("touch phase = MOVED");

                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = touch.position;
                    List<RaycastResult> UIHits = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, UIHits);

                    if (UIHits.Count > 0)
                    {
                        // Debug.Log("Hit a UI Element");
                        return;
                    }

                    if (currentlySelectedObject == null)
                    {
                        Debug.Log("An object is not selected to move!");
                        return;
                    }

                    Ray ray = _camera.ScreenPointToRay(touch.position);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("MeshLayer")))
                    {
                        Debug.Log("MOVED HIT POINT: " + hit.point + ", ROTATION?: " + hit.transform.rotation);
                        currentlySelectedObject.transform.position = hit.point;
                    }
                    //if (Physics.Raycast(ray, out hit))
                    //{
                    //    //Debug.Log("Hit Object name: " + hit.transform.gameObject.name);

                    //    if (hit.transform.gameObject.CompareTag("chairObject") || hit.transform.gameObject.CompareTag("tableObject") || hit.transform.gameObject.CompareTag("wallObject"))
                    //    {
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        Debug.Log("HIT NAME: " + hit.transform.gameObject.name + ", POSITION: " + hit.transform.gameObject.transform.position);
                    //        Debug.Log("HIT POINT: " + hit.point);
                    //        currentlySelectedObject.transform.position = hit.point;

                    //        GameObject placedObject = Instantiate(_placementObject, hit.point, Quaternion.identity);
                    //        Debug.Log("PLACED | X: " + placedObject.transform.position.x + " Y: " + placedObject.transform.position.y + " Z: " + placedObject.transform.position.z);
                    //        placedObject.GetComponent<NetworkObject>().Spawn();
                    //        addedPrefabs.Add(placedObject);
                    //        //Debug.Log("placedObject Instance ID: " + placedObject.GetInstanceID());
                    //        //Debug.Log("list item Instance ID: " + addedPrefabs.Last<GameObject>().GetInstanceID());
                    //        //Debug.Log("Length of addedPrefabs list: " + addedPrefabs.Count);
                    //    }
                    //}
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (currentlySelectedObject == null)
                {
                    Debug.Log("An object is not currently selected!");
                    return;
                }

                if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
                {
                    initialTouch1Pos = touch1.position;
                    initialTouch2Pos = touch2.position;
                    initialRotationAngle = Mathf.Atan2(initialTouch2Pos.y - initialTouch1Pos.y, initialTouch2Pos.x - initialTouch1Pos.x) * Mathf.Rad2Deg;
                }

                Vector2 updatedTouch1Pos = touch1.position;
                Vector2 updatedTouch2Pos = touch2.position;
                float updatedRotationAngle = Mathf.Atan2(updatedTouch2Pos.y - updatedTouch1Pos.y, updatedTouch2Pos.x - updatedTouch1Pos.x) * Mathf.Rad2Deg;

                float deltaRotation = updatedRotationAngle - initialRotationAngle;

                currentlySelectedObject.transform.Rotate(Vector3.up, deltaRotation, Space.World);

                //for (int i = 0; i < addedPrefabs.Count; i++)
                //{
                //    if (currentlySelectedObject.GetInstanceID() == addedPrefabs[i].GetInstanceID())
                //    {
                //        Debug.Log("Selected Object Found in List (ROTATION LOGIC)");
                //        addedPrefabs[i] = currentlySelectedObject;
                //        break;
                //    }
                //}

                initialRotationAngle = updatedRotationAngle;

                initialTouch1Pos = updatedTouch1Pos;
                initialTouch2Pos = updatedTouch2Pos;
            }
            #endif
        }

        private void OnDestroy()
        {
            _sharedSpaceManager.sharedSpaceManagerStateChanged -= OnColocalizationTrackingStateChanged;
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectedCallback;
                // shutdown and destroy NetworkManager when switching the scene
                NetworkManager.Singleton.Shutdown();
                Destroy(NetworkManager.Singleton.gameObject);
            }
        }

        private void OnColocalizationTrackingStateChanged(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs args)
        {
            if (args.Tracking)
            {
                Debug.Log("Colocalized.");
                // Hide the target image instruction panel
                _targetImageInstructionPanel.SetActive(false);

                // create an origin marker object and set under the sharedAR origin
                GameObject rootMarker = Instantiate(_sharedRootMarkerPrefab,
                    _sharedSpaceManager.SharedArOriginObject.transform, false);
                Debug.Log("ROOT MARKER: " + rootMarker.transform.position);

                // Hard reset to AR session + init meshing
                //_arSession.Reset();
                //_arMeshManager.enabled = true;

                // Start networking
                if (_startAsHost)
                {
                    NetworkManager.Singleton.StartHost();
                }
                else
                {
                    NetworkManager.Singleton.StartClient();
                }
            }
            else
            {
                Debug.Log($"Image tracking not tracking?");
            }
        }

        public void StartNewRoom()
        {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                _targetImage, _targetImageSize);

            // generate a new room name 3 digit number
            int code = (int)Random.Range(0.0f, 999.0f);
            _roomName = code.ToString("D3");
            var roomOptions = SetupRoomAndUI();

            _sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomOptions);

            // start as host
            _startAsHost = true;

            //_arMeshManager.enabled = true;
        }

        public void Join()
        {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                _targetImage, _targetImageSize);

            //set room name from text box
            _roomName = _roomNameInputField.text;
            var roomOptions = SetupRoomAndUI();

            _sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomOptions);

            // start as client
            _startAsHost = false;

            //_arMeshManager.enabled = true;
        }

        private ISharedSpaceRoomOptions SetupRoomAndUI()
        {
            // Update UI
            _roomNameDisplayText.text = $"PIN: {_roomName}";
            _netButtonsPanel.SetActive(false);
            _roomNameDisplayText.gameObject.SetActive(true);
            _targetImageInstructionPanel.SetActive(true);
            _buttonPanel.SetActive(true);

            chairButton = GameObject.Find("chair").GetComponent<Button>();
            tableButton = GameObject.Find("table").GetComponent<Button>();
            wallButton = GameObject.Find("wall").GetComponent<Button>();
            doneButton = GameObject.Find("Done").GetComponent<Button>();
            deleteButton = GameObject.Find("Delete").GetComponent<Button>();

            chairButton.onClick.AddListener(SwitchToChair);
            tableButton.onClick.AddListener(SwitchToTable);
            wallButton.onClick.AddListener(SwitchToWall);
            doneButton.onClick.AddListener(DeselectObject);
            deleteButton.onClick.AddListener(HideTargetPanel);

            //Create a room options and return it

            return ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                _roomName, 32, "image tracking colocalization demo");
        }

        private void OnServerStarted()
        {
            Debug.Log("Netcode server ready");
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            Debug.Log($"Client connected: {clientId}");
        }

        // Handle network disconnect
        private void OnClientDisconnectedCallback(ulong clientId)
        {
            var selfId = NetworkManager.Singleton.LocalClientId;
            if (NetworkManager.Singleton)
            {
                if (NetworkManager.Singleton.IsHost && clientId != NetworkManager.ServerClientId)
                {
                    // ignore other clients' disconnect event
                    return;
                }
                // show the UI panel for ending
                _endPanelText.text = "Disconnected from network";
                _endPanel.SetActive(true);
            }
        }

        // Handle host disconnected. For now, just show UI to go back home scene
        public void HandleHostDisconnected()
        {
            _endPanelText.text = "Host disconnected";
            _endPanel.SetActive(true);
        }

        public void EnableMeshManager()
        {
            Debug.Log("Enable Mesh Manager Clicked!");
            _arMeshManager.enabled = true;
        }

        private void SwitchToChair()
        {
            Debug.Log("Chair Button clicked");
            clientSelectedObject = "chair";
            _placementObject = _chairPrefab;
            //// Vector3 position = new Vector3((-1f * placementIndicator.transform.position.x), placementIndicator.transform.position.y, (-1 * placementIndicator.transform.position.z));
            //GameObject placedObject = Instantiate(_placementObject, placementIndicator.transform.position, placementIndicator.transform.rotation);
            ////addedPrefabs.Add(placedObject);
            //if (placedObject != null)
            //{

            //    placedObject.GetComponent<NetworkObject>().Spawn();
            //}
        }

        private void SwitchToWall()
        {
            Debug.Log("Wall Button clicked");
            clientSelectedObject = "wall";
            _placementObject = _wallPrefab;

            //GameObject localObject = Instantiate(_cube1, new Vector3(-4f, 0f, 1f), Quaternion.identity);
        }

        private void SwitchToTable()
        {
            Debug.Log("Table Button clicked");
            clientSelectedObject = "table";
            _placementObject = _tablePrefab;

            //GameObject networkObject = Instantiate(_cube2, new Vector3(-4f, 0f, 1f), Quaternion.identity);
            //networkObject.GetComponent<NetworkObject>().Spawn();
        }

        private void HideTargetPanel()
        {
            _targetImageInstructionPanel.SetActive(false);
        }

        private void DeselectObject()
        {
            currentlySelectedObject.GetComponent<MeshRenderer>().material = originalMaterial;
            currentlySelectedObject = null;

            sliderPanel.SetActive(false);
        }

        private void ScaleChangedY(float newValue)
        {
            if (currentlySelectedObject != null)
            {
                GameObject parentObject = currentlySelectedObject.transform.parent.gameObject;
                Debug.Log("SCALING | PARENT OBJECT: " + parentObject);
                Debug.Log("Actual selected object: " + currentlySelectedObject);
                //parentObject.transform.localScale = new Vector3(parentObject.transform.localScale.x, (2f * newValue), parentObject.transform.localScale.z);
                currentlySelectedObject.transform.localScale = new Vector3(currentlySelectedObject.transform.localScale.x, (2f * newValue), currentlySelectedObject.transform.localScale.z);

                for (int i = 0; i < addedPrefabs.Count; i++)
                {
                    if (currentlySelectedObject.GetInstanceID() == addedPrefabs[i].GetInstanceID())
                    {
                        Debug.Log("Selected Object Found in List (SCALING Y LOGIC)");
                        //addedPrefabs[i] = currentlySelectedObject;
                        break;
                    }
                }
            }
        }

        private void ScaleChangedZ(float newValue)
        {
            if (currentlySelectedObject != null)
            {
                currentlySelectedObject.transform.localScale = new Vector3((2f * newValue), currentlySelectedObject.transform.localScale.y, currentlySelectedObject.transform.localScale.z);

                for (int i = 0; i < addedPrefabs.Count; i++)
                {
                    if (currentlySelectedObject.GetInstanceID() == addedPrefabs[i].GetInstanceID())
                    {
                        Debug.Log("Selected Object Found in List (SCALING Y LOGIC)");
                        //addedPrefabs[i] = currentlySelectedObject;
                        break;
                    }
                }
            }
        }

        private void PlaceObjectServer(Vector3 hitPoint, string objectType, ulong clientId = 0)
        {
            GameObject placedObject = null;
            if (objectType == "server")
            {
                placedObject = Instantiate(NetworkManager.Singleton.GetNetworkPrefabOverride(_placementObject), hitPoint, Quaternion.identity);
            }
            else if (objectType == "chair")
            {
                placedObject = Instantiate(NetworkManager.Singleton.GetNetworkPrefabOverride(_chairPrefab), hitPoint, Quaternion.identity);
            }
            else if (objectType == "table")
            {
                placedObject = Instantiate(NetworkManager.Singleton.GetNetworkPrefabOverride(_tablePrefab), hitPoint, Quaternion.identity);
            }
            else if (objectType == "wall")
            {
                placedObject = Instantiate(NetworkManager.Singleton.GetNetworkPrefabOverride(_wallPrefab), hitPoint, Quaternion.identity);
            }
            Debug.Log(" - - - PLACED OBJECT  - - - | X: " + placedObject.transform.position.x + " Y: " + placedObject.transform.position.y + " Z: " + placedObject.transform.position.z);
            NetworkObject placedNetworkObject = placedObject.GetComponent<NetworkObject>();

            if (objectType == "server")
            {
                placedNetworkObject.Spawn();
            }
            else
            {
                placedNetworkObject.SpawnWithOwnership(clientId);
            }
            addedPrefabs.Add(placedObject);
            Debug.Log("placedObject Instance ID: " + placedObject.GetInstanceID());
            Debug.Log("list item Instance ID: " + addedPrefabs.Last<GameObject>().GetInstanceID());
            Debug.Log("Length of addedPrefabs list: " + addedPrefabs.Count);
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlaceObjectServerRpc(Vector3 hitpoint, string objectType, ServerRpcParams serverRpcParams = default)
        {
            Debug.Log("serverrpc params client id in serverrpc: " + serverRpcParams.Receive.SenderClientId);
            Debug.Log("hit poiunt in serverrpc: " + hitpoint);
            PlaceObjectServer(hitpoint, objectType, serverRpcParams.Receive.SenderClientId);
        }
    }
}
