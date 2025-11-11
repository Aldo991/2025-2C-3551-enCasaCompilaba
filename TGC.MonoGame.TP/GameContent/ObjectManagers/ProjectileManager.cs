#region File Description
/// Projectile Manager: Se encarga del manejo de las entidades proyectiles
/// que se encuentran en el juego
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace TGC.MonoGame.TP;

public class ProjectileManager
{
    private List<Projectile> _projectiles;
    public ProjectileManager()
    {
        _projectiles = new List<Projectile>();
    }
    // Agrega un projectil a la lista de projectiles activos en el juego
    public void AddProjectile(Projectile projectile)
    {
        _projectiles.Add(projectile);
    }
    // Elimina un projectil de la lista de projectiles activos. Se puede deber a que impactó en un objeto,
    // enemigo o simplemente desapareció por el tiempo
    public void DeleteProjectile(Projectile projectile)
    {
        _projectiles.Remove(projectile);
    }
    public void Update(GameTime gameTime)
    {
        foreach (Projectile projectile in _projectiles)
            projectile.Update(gameTime);
    }
    public void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        foreach (Projectile projectile in _projectiles)
            projectile.Draw(gameTime, view, projection);
    }
}