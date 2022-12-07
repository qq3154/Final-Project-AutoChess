using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class TaskMovetoTarget : Node
{
    private Hero _hero;
    private Hero _target;

    private bool _isReady = true;
    

    public TaskMovetoTarget(Hero hero)
    {
        _hero = hero;
    }

    public override NodeState Evaluate()
    {
        int distance = BoardManager.instance._X * BoardManager.instance._Y;
        
        if (_target == null)
        {
            _target = _hero.Target;
            //return NodeState.FAILURE;
        }

        //_moveCounter += Time.deltaTime;
        if (!_isReady) return NodeState.RUNNING;
        
        _isReady = false;
        {
            for (int i = 0; i < BoardManager.instance._X; i++)
            {
                for (int j = 0; j < BoardManager.instance._Y; j++)
                {
                    int range = Mathf.Max(Mathf.Abs(_target.PosX - i), Mathf.Abs(_target.PosY - j));
                    if (range <= _hero.HeroStats.AtkRange)
                    {
                        if (_hero.PosX == i && _hero.PosY == j)
                        {
                            return NodeState.SUCCESS;
                        }
                        else
                        {
                            if (BoardManager.instance.Pos[i, j] == false)
                            {
                                var findPath = new FindShortestPath();
                                findPath.Setup(_hero.PosX, _hero.PosY, i, j);
                                if (findPath.distance < distance)
                                {
                                    distance = findPath.distance;

                                    _hero.TargetPosX = i;
                                    _hero.TargetPosY = j;
                                    state = NodeState.SUCCESS;
                                }
                            }
                        }

                    }
                }
            }

            if ((_hero.PosX == _hero.TargetPosX) && (_hero.PosY == _hero.TargetPosY))
            {
                return NodeState.SUCCESS;
            }
            
            var findPath2 = new FindShortestPath();
            findPath2.Setup(_hero.PosX, _hero.PosY, _hero.TargetPosX, _hero.TargetPosY);
            if (findPath2.distance >1)
            {
                
                object[] content = new object[] {_hero.PosX, _hero.PosY, findPath2.nextMoveX, findPath2.nextMoveY}; 
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };  
                PhotonNetwork.RaiseEvent(PhotonEvent.OnHeroMove, content, raiseEventOptions, SendOptions.SendReliable);
                
                BoardManager.instance.Pos[_hero.PosX, _hero.PosY] = false;
                BoardManager.instance.Pos[findPath2.nextMoveX, findPath2.nextMoveY] = true;
                
                _hero.PosX = findPath2.nextMoveX;
                _hero.PosY = findPath2.nextMoveY;
                
                _hero.transform.DOMove(new Vector3(_hero.PosX, _hero.PosY, 0), 1/_hero.HeroStats.MoveSpeed)
                    .SetDelay(0)
                    .SetEase(Ease.InFlash)
                    .OnStart(() => DoOnStartMove())
                    .OnComplete(() => DoOnFinishMove(findPath2.nextMoveX, findPath2.nextMoveY));
                
                Debug.Log("move");
            
            
            }
            state =  NodeState.FAILURE;
        }

        return state;
    }

    private void DoOnStartMove()
    {
        _hero._axieFigureController.SetRun(_hero.HeroStats.MoveSpeed);
    }

    private void DoOnFinishMove(int nextX, int nextY)
    {

        _isReady = true;
        state =  NodeState.SUCCESS;
    }
}
