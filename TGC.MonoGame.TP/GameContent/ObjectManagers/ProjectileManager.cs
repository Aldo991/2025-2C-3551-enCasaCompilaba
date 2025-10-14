#region File Description
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace TGC.MonoGame.TP;

public class ProjectileManager
{
    private List<Projectile> _projectiles;
    public ProjectileManager(List<Projectile> projectiles = null)
    {
        if (_projectiles != null)
            _projectiles = new List<Projectile>(projectiles);
        else
            _projectiles = new List<Projectile>();
    }
    public void AddProjectile(Projectile projectile)
    {
        _projectiles.Add(projectile);
    }
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