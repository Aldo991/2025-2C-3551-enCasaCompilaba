#region FileDescription
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System.Collections.Generic;
#endregion

namespace TGC.MonoGame.TP;

public class TankManager
{
    private List<Tank> _tanks;

    public TankManager(List<Tank> tanks = null)
    {
        if (tanks != null)
            _tanks = tanks;
        else
            _tanks = new List<Tank>();
    }

    public void AddTank(Tank tank)
    {_tanks.Add(tank);}

    public void Update(GameTime gameTime)
    {
        foreach (var tank in _tanks)
            tank.Update(gameTime);
    }

    public void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        foreach (var tank in _tanks)
            tank.Draw(gameTime, view, projection);
    }
}