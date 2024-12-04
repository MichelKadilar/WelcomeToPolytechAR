using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class TimerGaze : MonoBehaviour
{
    [Header("Configuration")]
    public Camera mainCamera;
    public float fixationDuration = 2.0f;
    public AudioClip interactionSound;

    [Header("Debug")]
    public bool enableLogging = false;

    // Dictionnaire pour suivre les objets et leur temps de regard
    private Dictionary<GameObject, float> gazedObjects = new Dictionary<GameObject, float>();

    void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray centerRay = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;

        // R�initialiser tous les objets non regard�s
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var entry in gazedObjects)
        {
            if (!IsObjectGazed(entry.Key))
            {
                objectsToRemove.Add(entry.Key);
            }
        }

        // Supprimer les objets qui ne sont plus regard�s
        foreach (var obj in objectsToRemove)
        {
            gazedObjects.Remove(obj);
        }

        // V�rifier si un nouvel objet est regard�
        if (Physics.Raycast(centerRay, out hitInfo))
        {
            GameObject lookedObject = hitInfo.collider.gameObject;

            // Ajouter l'objet au suivi s'il n'est pas d�j� pr�sent
            if (!gazedObjects.ContainsKey(lookedObject))
            {
                gazedObjects[lookedObject] = 0f;
            }

            // Incr�menter le temps de regard
            gazedObjects[lookedObject] += Time.deltaTime;

            // V�rifier si le temps de regard est atteint
            if (gazedObjects[lookedObject] >= fixationDuration)
            {
                HandleGazeCompletedInteraction(lookedObject);
            }
        }
    }

    private bool IsObjectGazed(GameObject obj)
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray centerRay = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;

        return Physics.Raycast(centerRay, out hitInfo) && hitInfo.collider.gameObject == obj;
    }

    private void HandleGazeCompletedInteraction(GameObject gazedObject)
    {
        // Instancier un nouvel objet
        GameObject spawnedObject = Instantiate(gazedObject, Vector3.zero, Quaternion.identity);
        EnsureCollider(spawnedObject);

        Renderer renderer = gazedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        // Vibration
        Handheld.Vibrate();

        if (enableLogging)
        {
            Debug.Log($"Tap detected on gazed object: {gazedObject.name}");
        }

        // Jouer un son
        PlayInteractionSound(spawnedObject);

        // Log de d�bogage
        if (enableLogging)
        {
            Debug.Log($"Interaction completed with object: {gazedObject.name}");
        }

        // R�initialiser le temps de regard
        gazedObjects[gazedObject] = 0f;
    }

    private void EnsureCollider(GameObject obj)
    {
        if (obj.GetComponent<Collider>() == null)
        {
            BoxCollider collider = obj.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
    }

    private void PlayInteractionSound(GameObject obj)
    {
        if (interactionSound == null) return;

        AudioSource audioSource = obj.GetComponent<AudioSource>() ?? obj.AddComponent<AudioSource>();
        audioSource.clip = interactionSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.volume = 1.0f;
        audioSource.Play();
    }
}