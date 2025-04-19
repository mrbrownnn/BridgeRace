namespace _GAME.Scripts
{
   using UnityEngine;
   using System.Collections.Generic;
   using _GAME.Scripts.Enum;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs;
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<GameObject> botPrefabs;
    [SerializeField] private ColorData colorData;
    [SerializeField] private int botsPerLevel = 3;
    
    private int currentLevel = 0;
    private List<Floor> floors = new List<Floor>();
    private GameObject currentLevelInstance;
    private Player player;
    private List<Bot> bots = new List<Bot>();
    private Transform startPoint;
    private int currentFloor = 0;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            LoadLevel(currentLevel);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        // Ensure index is valid
        levelIndex = Mathf.Clamp(levelIndex, 0, levelPrefabs.Count - 1);
        currentLevel = levelIndex;
        
        // Clear current level if exists
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }
        
        floors.Clear();
        bots.Clear();
        
        // Instantiate new level
        currentLevelInstance = Instantiate(levelPrefabs[levelIndex], levelParent);
        
        // Find all floors in the level
        floors = new List<Floor>(currentLevelInstance.GetComponentsInChildren<Floor>());
        
        // Find start point
        startPoint = currentLevelInstance.transform.Find("StartPoint");
        if (startPoint == null)
        {
            Debug.LogError("StartPoint not found in level prefab!");
            return;
        }
        
        // Initialize floors
        for (int i = 0; i < floors.Count; i++)
        {
            floors[i].Init(i, colorData);
        }
        
        // Spawn player and bots
        SpawnCharacters();
        
        // Initialize floor bricks
        InitializeFloorBricks();
    }

    private void SpawnCharacters()
    {
        // Create list of available colors
        List<ColorType> availableColors = new List<ColorType>();
        for (int i = 0; i < System.Enum.GetValues(typeof(ColorType)).Length; i++)
        {
            availableColors.Add((ColorType)i);
        }
        ShuffleList(availableColors);
        
        // Spawn player
        Vector3 playerPos = startPoint.position + Vector3.right * 2f;
        GameObject playerObj = Instantiate(playerPrefab, playerPos, Quaternion.identity);
        player = playerObj.GetComponent<Player>();
        player.OnInit(availableColors[0]);
        
        // Spawn bots
        for (int i = 0; i < botsPerLevel && i + 1 < availableColors.Count; i++)
        {
            Vector3 botPos = startPoint.position + Vector3.left * (i + 1) * 2f;
            GameObject botObj = Instantiate(botPrefabs[Random.Range(0, botPrefabs.Count)], botPos, Quaternion.identity);
            Bot bot = botObj.GetComponent<Bot>();
            bot.OnInit(availableColors[i + 1]);
            bots.Add(bot);
        }
    }

    private void InitializeFloorBricks()
    {
        if (floors.Count == 0) return;
        
        foreach (Floor floor in floors)
        {
            // Create distribution of bricks
            Dictionary<ColorType, int> colorDistribution = new Dictionary<ColorType, int>();
            
            // Add player color
            colorDistribution.Add(player.GetCharColor(), 10);
            
            // Add bot colors
            foreach (Bot bot in bots)
            {
                colorDistribution.Add(bot.GetCharColor(), 10);
            }
            
            // Spawn bricks on the floor
            floor.SpawnBricks(colorDistribution);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public void SetCurrentFloor(int floorIndex)
    {
        currentFloor = floorIndex;
    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }
}
}