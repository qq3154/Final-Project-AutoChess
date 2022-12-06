using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFightCell : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (BoardManager.instance._IsLock)
        {
            return;
        }
        
      
        
        int x = (int)transform.position.x;  
        int y = (int)transform.position.y;

        if (GameFlowManager.instance.playerTeam == TeamID.Blue && y > 3)
        {
            Debug.Log("Cannot select opponent team");
            return;
        }
        
        if (GameFlowManager.instance.playerTeam == TeamID.Red && y < 4)
        {
            Debug.Log("Cannot select opponent team");
            return;
        }
        
        Debug.Log("select board cell"+ x + " " + y);
        BoardManager.instance.SelectCell(GameFlowManager.instance.playerTeam, x, y);
    }
}
