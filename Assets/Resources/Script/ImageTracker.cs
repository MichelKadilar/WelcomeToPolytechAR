using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public GameObject[] ArPrefabs;
    private Dictionary<ARTrackedImage, GameObject> instantiatedObjects = new Dictionary<ARTrackedImage, GameObject>();

    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
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
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if (!instantiatedObjects.ContainsKey(trackedImage))
            {
                foreach (var arPrefab in ArPrefabs)
                {
                    if (trackedImage.referenceImage.name == arPrefab.name)
                    {
                        GameObject prefabInstance = Instantiate(arPrefab, trackedImage.transform);
                        instantiatedObjects[trackedImage] = prefabInstance;
                        break;
                    }
                }
            }
            else
            {
                GameObject existingObject = instantiatedObjects[trackedImage];
                existingObject.SetActive(true);
                existingObject.transform.position = trackedImage.transform.position;
                existingObject.transform.rotation = trackedImage.transform.rotation;
            }
        }
        else
        {
            DisableARObject(trackedImage);
        }
    }

    private void DisableARObject(ARTrackedImage trackedImage)
    {
        if (instantiatedObjects.ContainsKey(trackedImage))
        {
            instantiatedObjects[trackedImage].SetActive(false);
        }
    }
}
