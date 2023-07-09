using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapScripts;

public class WayPoint : MonoBehaviour
{
    Cell cell;
    
    private void Awake()
    {
        cell = GetComponent<Cell>();
        cell.waypointActivate += ReverseRole;
    }
    public void ReverseRole()
    {
        ResourceManager.instance.CurrentState = ResourceManager.State.nature;
    }
}
