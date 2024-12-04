using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; // Pour ARTrackedImageManager et les fonctionnalités AR Foundation
using UnityEngine.XR.ARSubsystems; // Pour les types comme XRReferenceImage

public class DetectQRCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    [SerializeField]
    private GameObject arrow;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
            Instantiate(arrow, newImage.transform.position, newImage.transform.rotation);
            Debug.Log("QR Code détecté ! Objet instantié.");
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }
}
