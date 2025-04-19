namespace _GAME.Scripts
{
  using UnityEngine;
  using System.Collections.Generic;
  using _GAME.Scripts.Enum;

public class Floor : MonoBehaviour
{
    [SerializeField] private List<GroundBrick> bricks = new List<GroundBrick>();
    [SerializeField] private Transform bricksParent;
    [SerializeField] private GameObject groundBrickPrefab;
    [SerializeField] private int floorIndex;
    [SerializeField] private ColorData colorData;

    public int Index
    {
        get { return floorIndex; }
        set { floorIndex = value; }
    }

    public void Init(int index, ColorData colorDataRef)
    {
        floorIndex = index;
        colorData = colorDataRef;
        CollectAllBricks();
    }

    public void SpawnBricks(Dictionary<ColorType, int> colorDistribution)
    {
        // Clear existing bricks
        CollectAllBricks();
        
        // Get positions for spawning bricks
        List<Vector3> spawnPositions = GenerateSpawnPositions();
        
        // Shuffle positions
        ShufflePositions(spawnPositions);
        
        int positionIndex = 0;
        
        // Spawn bricks based on distribution
        foreach (var colorPair in colorDistribution)
        {
            ColorType colorType = colorPair.Key;
            int count = colorPair.Value;
            
            for (int i = 0; i < count && positionIndex < spawnPositions.Count; i++)
            {
                SpawnBrick(colorType, spawnPositions[positionIndex]);
                positionIndex++;
            }
        }
    }

    private List<Vector3> GenerateSpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        float spacing = 1.5f;
        
        Vector3 floorSize = new Vector3(20f, 0, 20f); // Example floor size
        
        for (float x = -floorSize.x/2 + spacing; x < floorSize.x/2 - spacing; x += spacing)
        {
            for (float z = -floorSize.z/2 + spacing; z < floorSize.z/2 - spacing; z += spacing)
            {
                positions.Add(new Vector3(x, 0.5f, z) + transform.position);
            }
        }
        
        return positions;
    }

    private void ShufflePositions(List<Vector3> positions)
    {
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[j];
            positions[j] = temp;
        }
    }

    public void SpawnBrick(ColorType colorType, Vector3 position)
    {
        GameObject brickObj = Instantiate(groundBrickPrefab, position, Quaternion.identity, bricksParent);
        GroundBrick brick = brickObj.GetComponent<GroundBrick>();
        Material material = colorData.GetMaterialByColorType(colorType);
        brick.Init(colorType, material);
        bricks.Add(brick);
    }

    public void RemoveBrick(GroundBrick brick)
    {
        if (bricks.Contains(brick))
        {
            bricks.Remove(brick);
        }
    }

    public void CollectAllBricks()
    {
        foreach (Transform child in bricksParent)
        {
            Destroy(child.gameObject);
        }
        bricks.Clear();
    }
}
}