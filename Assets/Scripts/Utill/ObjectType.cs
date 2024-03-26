using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePoolType
{
    Unit = 0,
    Object = 1
}

public enum eUnitType
{
    NONE = -1,

    Player = 0,
    Enemy = 1,
}

public enum eObjectType
{
    NONE = -1,

    Burger = 0,
    Money = 1,
    Trash = 2,
    Table = 3,
}


public class ObjectType : MonoBehaviour
{
    public ePoolType   _PoolType;
    public eUnitType   _UnitType;
    public eObjectType _ObjectType;

    public int GetObjectIdx()
    {
        switch(_PoolType)
        {
            case ePoolType.Unit:
                return getUnitIdx();
                case ePoolType.Object:
                return getObjectIdx();
        }
        return -1;
    }

    private int getUnitIdx()
    {
        switch(_UnitType)
        {
            case eUnitType.Player:
                return 0;
            case eUnitType.Enemy:
                return 1;                
        }
        return -1;
    }

    private int getObjectIdx()
    {
        switch (_ObjectType)
        {
            case eObjectType.Burger:
                return 0; 
        }
        return -1;
    }

}
