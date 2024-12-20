using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }

    public enum State //TODO add needed states
    {
        Neutral
    }

    private State currentState;

    public State CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentState = State.Neutral;
    }
}
