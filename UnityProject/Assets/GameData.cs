using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MetaData
{
    public string imageName;
    public int imageWidth;
    public int imageHeight;
    public int tileWidth;
    public int tileHeight;
}

[System.Serializable]
public class Tile
{
    public string objectFound;
    public int enumNumber;
    public int xPos;
    public int yPos;
    public bool hasBeenUpdated = false;
}

[System.Serializable]
public class GameData
{
    public MetaData metaData;
    public Tile[] tileArray;
}
