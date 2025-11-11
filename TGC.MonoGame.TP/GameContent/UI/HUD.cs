#region File Description
/// HUD se encarga de dibujar el HUD del juego en la pantalla
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

internal class Hud
{
    private HudState _hudState;
    private bool _showScoreboard;
    private Dictionary<GameState, HudState> _dictHudStates;
    public Hud(GraphicsDevice graphicsDevice, Tank player)
    {
        _dictHudStates = new Dictionary<GameState, HudState>()
        {
            {GameState.Menu, new Menu(graphicsDevice)},
            {GameState.Options, new Options(graphicsDevice)},
            {GameState.Playing, new Playing(graphicsDevice, player)}
        };
    }
    public void SetHudState(GameState gameState)
    {
        if (gameState == GameState.Exit)
            gameState = GameState.Playing;
        _hudState = _dictHudStates.GetValueOrDefault(gameState);
    }
    public void SetScoreboard(bool mode) => _showScoreboard = mode;
    public void Update(GameTime gameTime)
    {
        _hudState.Update(gameTime);
    }
    public void Draw()
    {
        _hudState.Draw();
    }
}