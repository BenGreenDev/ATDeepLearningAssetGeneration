using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System;

[System.Serializable]
public class NeighbouringTileCheck
{
    public bool east;
    public bool west;
    public bool north;
    public bool south;
    public bool northeast;
    public bool northwest;
    public bool southwest;
    public bool southeast;

}

public enum TileTypes
{
    CITYBUILDING = 0,
    DENSEFOREST = 1,
    GRASS = 2,
    ROAD = 3,
    SAND = 4,
    SPARSETREES = 5,
    VILLAGEBUILDING = 6,
    WATER = 7
}
public class WorldSpawn : MonoBehaviour
{
    public GameData gameData = null;
    public List<GameObject> spawnables;
    public List<GameObject> roadSpawnables;

    public GameObject CityBuilding;
    public GameObject DenseTrees;
    public GameObject Grass;
    public GameObject Road;
    public GameObject Sand;
    public GameObject SparseTrees;
    public GameObject VillageBuilding;
    public GameObject Water;

    public string jsonFilePath = Application.streamingAssetsPath + "/JSONFiles/";
    public List<Tile> sortedTileList;

    // Start is called before the first frame update
    void Start()
    {
        gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(jsonFilePath + "ClassifiedLevel.json"));
        OrderList();
        //LoadRoads();
        LinkRoads();
        RemoveStandaloneHouses();
        LoadScene();
    }

    private void LoadScene()
    {
        int numberOfCols = gameData.metaData.imageWidth / gameData.metaData.tileWidth;
        int numberOfRows = gameData.metaData.imageHeight / gameData.metaData.tileHeight;

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                int i = col + numberOfCols * row;
                if (i < gameData.tileArray.Length)
                {
                    //TileTypes tileType = (TileTypes)sortedTileList[i].enumNumber;

                    //switch(tileType)   
                    //{
                    //    case TileTypes.CITYBUILDING:
                    //        Instantiate(CityBuilding, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        //Call to scripterino
                    //        break;
                    //    case TileTypes.DENSEFOREST:
                    //        Instantiate(DenseTrees, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        break;
                    //    case TileTypes.GRASS:
                    //        Instantiate(Grass, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        break;
                    //    case TileTypes.ROAD:
                    //        Instantiate(Road, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        //Call to scripterino
                    //        break;
                    //    case TileTypes.SAND:
                    //        Instantiate(Sand, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        break;
                    //    case TileTypes.SPARSETREES:
                    //        Instantiate(SparseTrees, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        break;
                    //    case TileTypes.VILLAGEBUILDING:
                    //        Instantiate(VillageBuilding, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        //Call to scripterino
                    //        break;
                    //    case TileTypes.WATER:
                    //        Instantiate(Water, new Vector3(-row, 0, -col), Quaternion.identity);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    if (sortedTileList[i].objectFound != "Road" && sortedTileList[i].objectFound != "CityBuilding")
                    {
                        int indexToInstantiate = sortedTileList[i].enumNumber;
                        Instantiate(spawnables[indexToInstantiate], new Vector3(-row * 3, 0, -col * 3), Quaternion.identity);
                    }
                    else if (sortedTileList[i].objectFound == "Road")
                    {
                        GameObject temp = Instantiate(Road, new Vector3((-row * 3) - 1.5f, 0, (-col * 3) - 1.5f), Quaternion.identity);
                        temp.GetComponent<Road>().SetUp(GetPositionalData("Road", sortedTileList[i]));
                    }
                    else if (sortedTileList[i].objectFound == "CityBuilding")
                    {
                        GameObject temp = Instantiate(CityBuilding, new Vector3((-row * 3) + 1.5f, 0.45f, (-col * 3) + 1.5f), Quaternion.identity);
                        temp.GetComponent<VillageBuilding>().SetUp(GetPositionalData("CityBuilding", sortedTileList[i]));
                    }

                }
            }
        }
    }

    private void LoadRoads()
    {
        int numberOfCols = gameData.metaData.imageWidth / gameData.metaData.tileWidth;
        int numberOfRows = gameData.metaData.imageHeight / gameData.metaData.tileHeight;

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                int i = col + numberOfCols * row;
                if (i < gameData.tileArray.Length)
                {
                    if (sortedTileList[i].objectFound == "Road")
                    {
                        int indexToInstantiate = sortedTileList[i].enumNumber;
                        Instantiate(roadSpawnables[indexToInstantiate], new Vector3(col, 0, row), Quaternion.identity);
                    }
                }
            }
        }
    }

    private void RemoveStandaloneHouses()
    {
        foreach (var tile in sortedTileList)
        {
            if (tile.objectFound == "CityBuilding" || tile.objectFound == "VillageBuilding")
            {
                string tileType = tile.objectFound;

                NeighbouringTileCheck neighbouringTileCheck = new NeighbouringTileCheck();
                neighbouringTileCheck.east = CheckRight(tileType, tile.xPos, tile.yPos);

                neighbouringTileCheck.west = CheckLeft(tileType, tile.xPos, tile.yPos);

                neighbouringTileCheck.north = CheckUp(tileType, tile.xPos, tile.yPos);

                neighbouringTileCheck.south = CheckDown(tileType, tile.xPos, tile.yPos);

                neighbouringTileCheck.northeast = CheckUpRight("Road", tile.xPos, tile.yPos);

                neighbouringTileCheck.northwest = CheckUpLeft("Road", tile.xPos, tile.yPos);

                neighbouringTileCheck.southeast = CheckDownRight("Road", tile.xPos, tile.yPos);

                neighbouringTileCheck.southwest = CheckDownLeft("Road", tile.xPos, tile.yPos);

                if (Convert.ToInt32(neighbouringTileCheck.east) + Convert.ToInt32(neighbouringTileCheck.west) + Convert.ToInt32(neighbouringTileCheck.north) + Convert.ToInt32(neighbouringTileCheck.south) == 0)
                {
                    SetElementAsRoad("Grass", 2, tile.xPos, tile.yPos);
                }
            }
        }
    }


    private void OrderList()
    {

        int maxXPos = gameData.metaData.imageWidth;
        int maxYPos = gameData.metaData.imageHeight;

        int numberOfCols = gameData.metaData.imageWidth / gameData.metaData.tileWidth;
        int numberOfRows = gameData.metaData.imageHeight / gameData.metaData.tileHeight;

        int currentRow = 0;
        int currentCol = 0;

        for (int i = 0; i < gameData.tileArray.Length; i++)
        {
            if (currentCol == numberOfCols)
            {
                if (currentRow + 1 < numberOfRows)
                {
                    currentRow++;
                    currentCol = 0;
                }
            }

            Tile temp = gameData.tileArray.Where(x => x.yPos == currentRow * gameData.metaData.tileHeight && x.xPos == currentCol * gameData.metaData.tileHeight).FirstOrDefault();
            sortedTileList.Add(temp);

            currentCol++;
        }
    }

    private NeighbouringTileCheck GetPositionalData(string SearchQuery, Tile tile)
    {
        NeighbouringTileCheck neighbouringTileCheck = new NeighbouringTileCheck();
        neighbouringTileCheck.east = CheckRight(SearchQuery, tile.xPos, tile.yPos);

        neighbouringTileCheck.west = CheckLeft(SearchQuery, tile.xPos, tile.yPos);

        neighbouringTileCheck.north = CheckUp(SearchQuery, tile.xPos, tile.yPos);

        neighbouringTileCheck.south = CheckDown(SearchQuery, tile.xPos, tile.yPos);

        return neighbouringTileCheck;
    }
    private void LinkRoads()
    {
        foreach (var tile in sortedTileList)
        {
            if (tile.objectFound == "Road")
            {
                NeighbouringTileCheck neighbouringTileCheck = new NeighbouringTileCheck();
                neighbouringTileCheck.east = CheckRight("Road", tile.xPos, tile.yPos);
                //if (neighbouringTileCheck.east)
                //{
                //    SetElementAsRoad(tile.xPos + gameData.metaData.tileWidth, tile.yPos);
                //}


                neighbouringTileCheck.west = CheckLeft("Road", tile.xPos, tile.yPos);

                //if (neighbouringTileCheck.west)
                //{
                //    SetElementAsRoad(tile.xPos - gameData.metaData.tileWidth, tile.yPos);
                //}

                neighbouringTileCheck.north = CheckUp("Road", tile.xPos, tile.yPos);

                //if (neighbouringTileCheck.north)
                //{
                //    SetElementAsRoad(tile.xPos, tile.yPos - gameData.metaData.tileHeight);
                //}

                neighbouringTileCheck.south = CheckDown("Road", tile.xPos, tile.yPos);

                //if(neighbouringTileCheck.south)
                //{
                //    SetElementAsRoad(tile.xPos, tile.yPos + gameData.metaData.tileHeight);
                //}

                //neighbouringTileCheck.northeast = CheckUpRight("Road", tile.xPos, tile.yPos);

                //if (neighbouringTileCheck.northeast)
                //{
                //    if(!CheckRight("Road", tile.xPos, tile.yPos))
                //    {
                //        SetElementAsRoad(tile.xPos + gameData.metaData.tileWidth, tile.yPos);
                //    }
                //}

                //neighbouringTileCheck.northwest = CheckUpLeft("Road", tile.xPos, tile.yPos);

                //if (neighbouringTileCheck.northwest)
                //{
                //    if (!CheckLeft("Road", tile.xPos, tile.yPos))
                //    {
                //        SetElementAsRoad(tile.xPos - gameData.metaData.tileWidth, tile.yPos);
                //    }
                //}

                //neighbouringTileCheck.southeast = CheckDownRight("Road", tile.xPos, tile.yPos);

                //if (neighbouringTileCheck.southeast)
                //{
                //    if (!CheckRight("Road", tile.xPos, tile.yPos))
                //    {
                //        SetElementAsRoad(tile.xPos + gameData.metaData.tileWidth, tile.yPos);
                //    }
                //}

                //neighbouringTileCheck.southwest = CheckDownLeft("Road", tile.xPos, tile.yPos);

                //if (neighbouringTileCheck.southwest)
                //{
                //    if (!CheckLeft("Road", tile.xPos, tile.yPos))
                //    {
                //        SetElementAsRoad(tile.xPos - gameData.metaData.tileWidth, tile.yPos);
                //    }
                //}
            }
        }
    }

    private bool CheckLeft(string checkFor, int currentXPos, int currentYPos)
    {
        Tile temp = sortedTileList.Where(x => x.objectFound == checkFor && x.xPos == currentXPos - gameData.metaData.tileWidth && x.yPos == currentYPos).FirstOrDefault();
        return sortedTileList.Any(x => x.objectFound == checkFor && x.xPos == currentXPos - gameData.metaData.tileWidth && x.yPos == currentYPos);
    }
    private bool CheckRight(string checkFor, int currentXPos, int currentYPos)
    {
        Tile temp = sortedTileList.Where(x => x.objectFound == checkFor && x.xPos == currentXPos + gameData.metaData.tileWidth && x.yPos == currentYPos).FirstOrDefault();
        return sortedTileList.Any(x => x.objectFound == checkFor && x.xPos == currentXPos + gameData.metaData.tileWidth && x.yPos == currentYPos);
    }
    private bool CheckUp(string checkFor, int currentXPos, int currentYPos)
    {
        Tile temp = sortedTileList.Where(x => x.objectFound == checkFor && x.yPos == currentYPos - gameData.metaData.tileHeight && x.yPos == currentXPos).FirstOrDefault();
        return sortedTileList.Any(x => x.objectFound == checkFor && x.yPos == currentYPos - gameData.metaData.tileHeight && x.xPos == currentXPos);
    }
    private bool CheckDown(string checkFor, int currentXPos, int currentYPos)
    {
        Tile temp = sortedTileList.Where(x => x.objectFound == checkFor && x.yPos == currentYPos + gameData.metaData.tileHeight && x.yPos == currentXPos).FirstOrDefault();
        return sortedTileList.Any(x => x.objectFound == checkFor && x.yPos == currentYPos + gameData.metaData.tileHeight && x.xPos == currentXPos);
    }

    private bool CheckUpLeft(string checkFor, int currentXPos, int currentYPos)
    {
        return sortedTileList.Any(x => x.objectFound == checkFor && x.xPos == currentXPos - gameData.metaData.tileWidth && x.yPos == currentYPos - gameData.metaData.tileHeight);
    }

    private bool CheckUpRight(string checkFor, int currentXPos, int currentYPos)
    {
        return sortedTileList.Any(x => x.objectFound == checkFor && x.xPos == currentXPos + gameData.metaData.tileWidth && x.yPos == currentYPos - gameData.metaData.tileHeight);
    }

    private bool CheckDownLeft(string checkFor, int currentXPos, int currentYPos)
    {
        return sortedTileList.Any(x => x.objectFound == checkFor && x.xPos == currentXPos - gameData.metaData.tileWidth && x.yPos == currentYPos + gameData.metaData.tileHeight);
    }

    private bool CheckDownRight(string checkFor, int currentXPos, int currentYPos)
    {
        return sortedTileList.Any(x => x.objectFound == checkFor && x.xPos == currentXPos + gameData.metaData.tileWidth && x.yPos == currentYPos + gameData.metaData.tileHeight);
    }

    private void SetElementAsRoad(string setTo, int enumNumber, int xPos, int YPos)
    {
        int temp = sortedTileList.FindIndex(x => x.yPos == YPos && x.xPos == xPos);

        if (temp != -1)
        {

            if (sortedTileList[temp].hasBeenUpdated != true)
            {
                sortedTileList[temp].objectFound = setTo;
                    sortedTileList[temp].enumNumber = enumNumber;
                sortedTileList[temp].hasBeenUpdated = true;
            }
        }
    }
}

