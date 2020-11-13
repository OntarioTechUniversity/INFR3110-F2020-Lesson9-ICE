using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileTypes
{
    AIR,
    ROCK,
    DIRT,
    GRASS
}
public class TileController : MonoBehaviour
{
    [Header("World Size")]
    public int width;
    public int height;

    [Header("Generator Settings")]
    [Range(0, 100)]
    public float MinPower = 16.0f;

    [Range(1, 200)]
    public float MaxPower = 24.0f;


    public Grid grid;
    public Tilemap tilemap;
    public Tile rockTile;
    public Tile dirtTile;
    public Tile grassTile;

    [Header("Generation Results")]
    public TileTypes[,] Tiles;
    public Tile[,] TileArray;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Generate();
        }
    }

    public void InitTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = TileTypes.AIR;
                if (TileArray.Length > 0)
                {
                    Destroy(TileArray[x, y]);
                    TileArray[x, y] = null;
                }
            }
        }
    }

    public void Generate()
    {
        Tiles = new TileTypes[width, height];
        TileArray = new Tile[width, height];
        InitTiles();
        DestroyAllTiles();

        float rand = Random.Range(MinPower, MaxPower);

        float offsetX = Random.Range(-1024.0f, 1024.0f);
        float offsetY = Random.Range(-1024.0f, 1024.0f);

        // Generation
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var distribution = Mathf.PerlinNoise((x + offsetX) / rand, (y + offsetY) / rand);
                if (y < distribution * height * 0.5)
                {

                    if (y < (height * 0.10f))
                    {
                        if (distribution < 0.2)
                        {
                            Tiles[x, y] = TileTypes.AIR;
                        }
                        else if (distribution >= 0.2 && distribution < 0.7)
                        {
                            Tiles[x, y] = TileTypes.ROCK;
                        }
                        else
                        {
                            Tiles[x, y] = TileTypes.DIRT;
                        }

                    }
                    else if ((y >= (height * 0.10f)) && (y < (height * 0.25f)))
                    {
                        if (distribution < 0.2)
                        {
                            Tiles[x, y] = TileTypes.AIR;
                        }
                        else if (distribution >= 0.2 && distribution < 0.7)
                        {
                            Tiles[x, y] = TileTypes.DIRT;
                        }
                        else
                        {
                            Tiles[x, y] = TileTypes.GRASS;
                        }
                    }
                    else
                    {
                        Tiles[x, y] = TileTypes.GRASS;
                    }

                }
            }
        }

        InstantiateTiles();
    }

    public void InstantiateTiles()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = null;

                TileTypes TileTypeToGenerate = Tiles[x, y];
                switch (TileTypeToGenerate)
                {
                    case TileTypes.ROCK:
                        tile = rockTile;
                        break;
                    case TileTypes.DIRT:
                        tile = dirtTile;
                        break;
                    case TileTypes.GRASS:
                        tile = grassTile;
                        break;
                    default:
                        continue;
                }

                TileArray[x, y] = tile;
                tilemap.SetTile(new Vector3Int(x - (int)(width * 0.5f), y - (int)(height * 0.25f), 0), tile);

                //TileArray[x, y] = Instantiate(Tile, new Vector2((x * 0.25f) - (width * 0.125f), (y * 0.25f) - (height * 0.05f)), Quaternion.identity);
                //TileArray[x, y].transform.parent = transform;
            }
        }
    }

    public void DestroyAllTiles()
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Destroy(TileArray[x, y]);
                TileArray[x, y] = null;
            }
        }
    }
}
