using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour, ISystem
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;


    private GameData _gameData;
    private static SaveSystem _instance;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    public ref GameData GameData { get { return ref _gameData; } }
    public static SaveSystem Instance { get { return _instance; } }
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

    public void Init()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        this._dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        DontDestroyOnLoad(gameObject);
        LoadGame();
    }

    public void SaveGame() 
    {
        foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref _gameData);
        }

        Debug.Log("Game Saved");

        _dataHandler.Save(_gameData);
    }

    public void LoadGame() 
    {
        this._gameData = _dataHandler.Load();

        if (this._gameData == null)
        {
            Debug.Log("No data was found. Init by default data");
            _gameData = new GameData();
        }

        foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
        {
            dataPersistence.LoadData(_gameData);
        }
    }

    public void NewGame() 
    { 
        _gameData = new GameData();
        _dataHandler.Save(_gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects);
    }

    private void OnSceneUnloaded(Scene current)
    {
        //SaveGame();
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
}
