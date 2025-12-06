#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class GameElements
{
    private List<GameObject> _gameElements;
    public GameElements()
    {
        // Inicializo las variables
        _gameElements = new List<GameObject>();
        // inicializo las posiciones
        MapPopulator.Generate();
        List<Vector2> housePosition = MapPopulator.Houses;
        List<Vector2> bigStonePosition = MapPopulator.BigStones;
        List<Vector2> bushes = MapPopulator.Bushes;
        List<Vector2> littleStonePosition = MapPopulator.LittleStones;
        // 0.003f está bien, aunque podría ser apenas un poquito más grande, para hacerlo más alto al árbol
        // Son los 3 el mismo modelo?
        // float scaleTreeModel0 = 0.003f;
        // float scaleTreeModel1 = 0.003f;
        // float scaleTreeModel2 = 0.015f;
        // Está bien, pero sería más un muro más que una pared
        // float scaleWallModel0 = 0.03f;

        foreach (var pos in housePosition)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Y), pos.Y);
            var casaModelo1 = new House(ContentLoader.GetModel("house", 0), finalPos, House.DefaultScaleHouse, 0);
            Texture2D texture = ContentLoader.GetTexture("house", 0);
            Texture2D normal = ContentLoader.GetNormal("house", 0);
            casaModelo1.SetTexture(texture);
            casaModelo1.SetNormal(normal);
            _gameElements.Add(casaModelo1);
        }
        foreach(var pos in bigStonePosition)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Y), pos.Y);
            int modelIndex = MapPopulator.GenerateRandomInt(0,5);
            var stone = new Stone(ContentLoader.GetModel("stone", modelIndex), finalPos, Stone.DefaultScaleBigStone);
            Texture2D texture = ContentLoader.GetTexture("stone", 2);
            stone.SetTexture(texture);
            _gameElements.Add(stone);
        }
        foreach(var pos in bushes)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Y), pos.Y);
            var bush = new Bush(ContentLoader.GetModel("bush", 1), finalPos);
            bush.SetTexture(ContentLoader.GetTexture("bush", 0));
            _gameElements.Add(bush);
        }
        foreach (var pos in littleStonePosition)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Y), pos.Y);
            int modelIndex = MapPopulator.GenerateRandomInt(0,5);
            var stone = new Stone(ContentLoader.GetModel("stone", modelIndex), finalPos, Stone.DefaultScaleLittleStone);
            Texture2D texture = ContentLoader.GetTexture("stone", 2);
            stone.SetTexture(texture);
            _gameElements.Add(stone);
        }

    }
    public void Update(GameTime gameTime)
    {
        foreach (GameObject gameObject in _gameElements)
            gameObject.Update(gameTime);
    }
    // public void Draw(GameTime gameTime, Matrix view, Matrix projection)
    public void Draw(GameTime gameTime, FollowCamera camera)
    {
        foreach (GameObject gameObject in _gameElements)
        {
            if (camera.IsOnCamera(gameObject.GetBoundingBoxToDraw()))
                gameObject.Draw(gameTime, camera.ViewMatrix, camera.ProjectionMatrix);
        }
    }
}