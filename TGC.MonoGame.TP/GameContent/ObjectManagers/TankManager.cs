#region FileDescription
/// Tank manager: Se va a encargar de manejar todos los enemigos activos 
/// que se encuentren en el mapa.
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
            _tanks = new List<Tank>(tanks);
        else
            _tanks = new List<Tank>();
    }
    // Agrega un tanque a la lista de tanques activos
    public void AddTank(Tank tank)
    {
        _tanks.Add(tank);
    }
    // Elimina un tanque de la lista de tanques activos
    public void DeleteTank(Tank tank)
    {
        _tanks.Remove(tank);
    }
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