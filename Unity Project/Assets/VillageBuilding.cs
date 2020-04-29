using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VillageBuilding : MonoBehaviour
{
    public GameObject VillageBuildingCenter;

    public GameObject VillageBuildingFrontN;
    public GameObject VillageBuildingFrontW;
    public GameObject VillageBuildingFrontE;
    public GameObject VillageBuildingFrontS;


    public GameObject VillageBuildingEndN;
    public GameObject VillageBuildingEndW;
    public GameObject VillageBuildingEndE;
    public GameObject VillageBuildingEndS;

    private NeighbouringTileCheck neighbouringTiles;

    public void SetUp(NeighbouringTileCheck _neighbouringTiles)
    {
        neighbouringTiles = _neighbouringTiles;
        EnableRelevantTile();
    }

    private void EnableRelevantTile()
    {

        if ((Convert.ToInt32(neighbouringTiles.south) + Convert.ToInt32(neighbouringTiles.north) + (Convert.ToInt32(neighbouringTiles.east) + Convert.ToInt32(neighbouringTiles.west)) == 1))
        {
            DisableAllChildren();

            if (neighbouringTiles.south)
            {
                VillageBuildingFrontN.SetActive(true);
            }

            else if (neighbouringTiles.north)
            {
                VillageBuildingFrontS.SetActive(true);
            }

            else if (neighbouringTiles.east)
            {
                VillageBuildingFrontW.SetActive(true);
            }

            else if (neighbouringTiles.west)
            {
                VillageBuildingFrontE.SetActive(true);
            }

        }


            //if ((Convert.ToInt32(neighbouringTiles.south) + Convert.ToInt32(neighbouringTiles.north)) + Convert.ToInt32(neighbouringTiles.east) + Convert.ToInt32(neighbouringTiles.west) == 1)
            //{
            //    DisableAllChildren();

            //    if (neighbouringTiles.south)
            //    {
            //        VillageBuildingEnd.SetActive(true);
            //    }
            //}

        if ((Convert.ToInt32(neighbouringTiles.south) + Convert.ToInt32(neighbouringTiles.north) + Convert.ToInt32(neighbouringTiles.east) + Convert.ToInt32(neighbouringTiles.west) == 0))
        {
            DisableAllChildren();

            int randomNum = UnityEngine.Random.Range(0, 4);

            switch(randomNum)
            {
                case 0:
                    VillageBuildingFrontN.SetActive(true);
                    break;
                case 1:
                    VillageBuildingFrontS.SetActive(true);
                    break;
                case 2:
                    VillageBuildingFrontE.SetActive(true);
                    break;
                case 3:
                    VillageBuildingFrontW.SetActive(true);
                    break;
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
