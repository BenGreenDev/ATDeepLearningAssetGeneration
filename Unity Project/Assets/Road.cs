using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Road : MonoBehaviour
{
    //BASIC
    public GameObject StraightRoad;
    public GameObject StraightRoadWE;

    //ROAD ENDINGS
    public GameObject RoadEnd;
    public GameObject NorthToSouthEnd;
    public GameObject SouthToNorthEnd;
    public GameObject EastToWestEnd;
    public GameObject WestToEastEnd;

    //TURNINGS
    public GameObject NorthToSouthEastTurn;
    public GameObject NorthToSouthWestTurn;
    public GameObject WestToEastSouthTurn;
    public GameObject WestToEastNorthTurn;

   
    //CROSSINGS
    public GameObject Junction;

    public NeighbouringTileCheck neighbouringTiles;


    public void SetUp(NeighbouringTileCheck _neighbouringTiles)
    {
        neighbouringTiles = _neighbouringTiles;
        EnableRelevantTile();
    }

    private void EnableRelevantTile()
    {
        //Verticals
        if((neighbouringTiles.north || neighbouringTiles.south) && (!neighbouringTiles.east && !neighbouringTiles.west))
        {
            StraightRoad.SetActive(true);
        }

        if((neighbouringTiles.north || neighbouringTiles.south) && neighbouringTiles.east)
        {
            DisableAllChildren();
            NorthToSouthEastTurn.SetActive(true);
        }

        if ((neighbouringTiles.north || neighbouringTiles.south) && neighbouringTiles.west)
        {
            DisableAllChildren();
            NorthToSouthWestTurn.SetActive(true);
        }

        //Horizontals
        if (neighbouringTiles.east || neighbouringTiles.west && (!neighbouringTiles.north && !neighbouringTiles.south))
        {
            DisableAllChildren();
            StraightRoadWE.SetActive(true);
        }

        if ((neighbouringTiles.east || neighbouringTiles.west) && neighbouringTiles.north)
        {
            DisableAllChildren();
            WestToEastNorthTurn.SetActive(true);
        }

        if ((neighbouringTiles.east || neighbouringTiles.west) && neighbouringTiles.south)
        {
            DisableAllChildren();
            WestToEastSouthTurn.SetActive(true);
        }

        int neighbouringTileCount = Convert.ToInt32(neighbouringTiles.north) + Convert.ToInt32(neighbouringTiles.south) + Convert.ToInt32(neighbouringTiles.west) + Convert.ToInt32(neighbouringTiles.east);
        if (neighbouringTileCount > 2)
        {
            DisableAllChildren();
            Junction.SetActive(true);
        }

        if (neighbouringTileCount == 1)
        {
            if (neighbouringTiles.south)
            {
                DisableAllChildren();
                SouthToNorthEnd.SetActive(true);
            }
            else if (neighbouringTiles.north)
            {
                DisableAllChildren();
                NorthToSouthEnd.SetActive(true);
            }
            else if (neighbouringTiles.west)
            {
                DisableAllChildren();
                WestToEastEnd.SetActive(true);
            }
            else
            {
                DisableAllChildren();
                EastToWestEnd.SetActive(true);
            }

        }
    }
    
    private void DisableAllChildren()
    {
        for (int a = 0; a < transform.childCount; a++)
        {
            transform.GetChild(a).gameObject.SetActive(false);
        }
    }
}
