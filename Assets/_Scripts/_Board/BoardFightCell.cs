using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFightCell : MonoBehaviour
{
    private void OnMouseDown()
    {
        int x = (int)transform.position.x;  
        int y = (int)transform.position.y;   
        Debug.Log("select board cell"+ x + " " + y);
        BoardManager.instance.SelectCell(GameFlowManager.instance.playerTeam, x, y);
    }
}
