using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;

public class ImageTracker : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;

    public Vector3 offset1 = new Vector3(0.1f, 0f, 0f);
    public Vector3 offset2 = new Vector3(-0.1f, 0.2f, 0f);
    public Vector3 offset3 = new Vector3(0f, 0.1f, 0.1f);
    public Vector3 offset4 = new Vector3(0f, 0.1f, -0.1f);

    private Dictionary<ARTrackedImage, GameObject[]> instantiatedObjects = new Dictionary<ARTrackedImage, GameObject[]>();

    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        if (trackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager introuvable. Assurez-vous qu'il est ajouté à la scène.");
        }
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            UpdateARObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            UpdateARObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.removed)
        {
            DisableARObject(trackedImage);
        }
    }

    private void UpdateARObject(ARTrackedImage trackedImage)
    {
        // Check if the image is tracked
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(!ClientManager.Instance.mobilesStatus[ClientManager.Instance.clientId]) {
                ClientManager.Instance.mobilesStatus[ClientManager.Instance.clientId] = true;
                ClientManager.Instance.SendNetworkMessage("MOBILE_AR_ON", ClientManager.Instance.clientId.ToString());
            }
            if (!instantiatedObjects.ContainsKey(trackedImage))
            {
                // Instanciate objects
                GameObject[] prefabInstances = new GameObject[] {
                    Instantiate(prefab1, trackedImage.transform.position + offset1, trackedImage.transform.rotation),
                    Instantiate(prefab2, trackedImage.transform.position + offset2, trackedImage.transform.rotation),
                    Instantiate(prefab2, trackedImage.transform.position + offset3, trackedImage.transform.rotation),
                    Instantiate(prefab2, trackedImage.transform.position + offset4, trackedImage.transform.rotation)
                };

                // Assign objects and hide them
                foreach(GameObject prefabInstance in prefabInstances) {
                    prefabInstance.transform.SetParent(trackedImage.transform, true);
                    prefabInstance.SetActive(false);
                }

                for(int i = 0; i < ClientManager.Instance.mobiles.Count && i < 4; i++) {
                    if(ClientManager.Instance.mobilesStatus[ClientManager.Instance.mobiles[i]]) {
                        prefabInstances[i].SetActive(true);
                    }
                }

                // Register created objects
                instantiatedObjects[trackedImage] = prefabInstances;
            }
            else
            {
                // Update existing objects
                GameObject[] existingObjects = instantiatedObjects[trackedImage];

                foreach(GameObject prefabInstance in existingObjects) {
                    prefabInstance.transform.SetParent(trackedImage.transform, true);
                    prefabInstance.SetActive(false);
                }

                for(int i = 0; i < ClientManager.Instance.mobiles.Count && i < 4; i++) {
                    if(ClientManager.Instance.mobilesStatus[ClientManager.Instance.mobiles[i]]) {
                        existingObjects[i].SetActive(true);
                    }
                }

                // Update position and rotation
                existingObjects[0].transform.position = trackedImage.transform.position + offset1;
                existingObjects[0].transform.rotation = trackedImage.transform.rotation;

                existingObjects[1].transform.position = trackedImage.transform.position + offset2;
                existingObjects[1].transform.rotation = trackedImage.transform.rotation;
                
                existingObjects[2].transform.position = trackedImage.transform.position + offset3;
                existingObjects[2].transform.rotation = trackedImage.transform.rotation;
                
                existingObjects[3].transform.position = trackedImage.transform.position + offset4;
                existingObjects[3].transform.rotation = trackedImage.transform.rotation;
            }
        }
        else
        {
            DisableARObject(trackedImage);
        }
    }

    private void DisableARObject(ARTrackedImage trackedImage)
    {
        if(ClientManager.Instance.mobilesStatus[ClientManager.Instance.clientId]) {
            ClientManager.Instance.mobilesStatus[ClientManager.Instance.clientId] = false;
            ClientManager.Instance.SendNetworkMessage("MOBILE_AR_OFF", ClientManager.Instance.clientId.ToString());
        }
        if (instantiatedObjects.ContainsKey(trackedImage))
        {
            // Hide objects
            instantiatedObjects[trackedImage][0].SetActive(false);
            instantiatedObjects[trackedImage][1].SetActive(false);
            instantiatedObjects[trackedImage][2].SetActive(false);
            instantiatedObjects[trackedImage][3].SetActive(false);
        }
    }
}