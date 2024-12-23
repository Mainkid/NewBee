using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPointSystem : MonoBehaviour
{
    [SerializeField]
    public GameObject[] MySystems;

    private static EntryPointSystem _instance;
    public static EntryPointSystem Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        foreach (var system in MySystems)
        {
            system.GetComponent<ISystem>().Init();
        }
    }

}
