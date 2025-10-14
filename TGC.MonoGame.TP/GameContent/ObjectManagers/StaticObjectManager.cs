#region File Description
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

using TGC.MonoGame.TP;

public class StaticObjectManager
{
    private List<GameObject> _staticObjects;
    public StaticObjectManager(List<GameObject> gameObject = null)
    {
        if (_staticObjects != null)
            _staticObjects = new List<GameObject>(gameObject);
        else
            _staticObjects = new List<GameObject>();
    }
    public void AddGameObject(GameObject gameObject)
    {
        _staticObjects.Add(gameObject);
    }
    public void DeleteGameObject(GameObject gameObject)
    {
        _staticObjects.Remove(gameObject);
    }
    public void Update(GameTime gameTime)
    {
        foreach (GameObject gameObject in _staticObjects)
            gameObject.Update(gameTime);
    }
    public void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        foreach (GameObject gameObject in _staticObjects)
            gameObject.Draw(gameTime, view, projection);
    }
}