using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Zero
{
    public class ElementosLand
    {
        private List<LandModel> _elementosL = new();
        private List<HouseModel1> _elementosH = new();
        private List<ArbolModel1> _arboles = new();
        private List<ArbolModel2> _arboles2 = new();

        public ElementosLand(ContentManager content, string contentFolder3D, string contentFolderEffects)
        {
            // Terreno
            var land = new LandModel(content, contentFolder3D, contentFolderEffects);
            land.Initialize(new Vector3(0, -990, 0));
            _elementosL.Add(land);

            // Una casa
            var house = new HouseModel1(content, contentFolder3D, contentFolderEffects);
            house.Initialize(new Vector3(0, 490, 1490));
            _elementosH.Add(house);

            var house2 = new HouseModel1(content, contentFolder3D, contentFolderEffects);
            house2.Initialize(new Vector3(-9100, 400, 12390));
            _elementosH.Add(house2);

            //80 arboles de mismo modelo
            var posicionesArboles = new List<Vector3>
            {
                new Vector3(1000, 490, 1000), new Vector3(2000, 490, 5000),
                new Vector3(2000, 490, 3400), new Vector3(2500, 490, 5400),
                new Vector3(2500, 490, 9200), new Vector3(3000, 490, 1200),
                new Vector3(3000, 490, 1800), new Vector3(3000, 490, 9000),
                new Vector3(4000, 490, 3600), new Vector3(4000, 490, 5200),
                new Vector3(3800, 490, 7200), new Vector3(4500, 490, 5600),
                new Vector3(4500, 490, 9500), new Vector3(5000, 490, 1500),
                new Vector3(5000, 490, 2000), new Vector3(5000, 490, 9200),
                new Vector3(6000, 490, 3800), new Vector3(6000, 490, 5500),
                new Vector3(5800, 490, 7400), new Vector3(6500, 490, 5800),
                new Vector3(6500, 490, 9800), new Vector3(7000, 490, 1800),
                new Vector3(7000, 490, 2200), new Vector3(7000, 490, 9400),
                new Vector3(7800, 490, 7600), new Vector3(7800, 490, 13800),
                new Vector3(8000, 490, 4000), new Vector3(8000, 490, 5800),
                new Vector3(8500, 490, 6000), new Vector3(8500, 490, 10000),
                new Vector3(9000, 490, 2000), new Vector3(9000, 490, 2400),
                new Vector3(9000, 490, 6000), new Vector3(9000, 490, 7800),
                new Vector3(9800, 490, 14000), new Vector3(9800, 490, 7800),
                new Vector3(10000, 490, 4200), new Vector3(10000, 490, 6000),
                new Vector3(10000, 490, 5800), new Vector3(10500, 490, 10200),
                new Vector3(10500, 490, 6200), new Vector3(11000, 490, 2500),
                new Vector3(11000, 490, 2600), new Vector3(11800, 490, 8000),
                new Vector3(11800, 490, 14200), new Vector3(12000, 490, 12000),
                new Vector3(12000, 490, 6200), new Vector3(12000, 490, 4400),
                new Vector3(12500, 490, 10500), new Vector3(12500, 490, 6400),
                new Vector3(13000, 490, 2800), new Vector3(13000, 490, 6600),
                new Vector3(13800, 490, 14500), new Vector3(13800, 490, 8200),
                new Vector3(14000, 490, 4600), new Vector3(14000, 490, 6500),
                new Vector3(14500, 490, 10800), new Vector3(14500, 490, 6600),
                new Vector3(15000, 490, 3000), new Vector3(15000, 490, 4600),
                new Vector3(15800, 490, 14800), new Vector3(15800, 490, 8400),
                new Vector3(16000, 490, 4800)
            };

            var posicionesArboles2 = new List<Vector3>
            {
                new Vector3(1000, 490, -1000), new Vector3(2000, 490, -5000),
                new Vector3(2000, 490, -3400), new Vector3(2500, 490, -5400),
                new Vector3(2500, 490, -9200), new Vector3(3000, 490, -1200),
                new Vector3(3000, 490, -1800), new Vector3(3000, 490, -9000),
                new Vector3(4000, 490, -3600), new Vector3(4000, 490, -5200),
                new Vector3(3800, 490, -7200), new Vector3(4500, 490, -5600),
                new Vector3(4500, 490, -9500), new Vector3(5000, 490, -1500),
                new Vector3(5000, 490, -2000), new Vector3(5000, 490, -9200),
                new Vector3(6000, 490, -3800), new Vector3(6000, 490, -5500),
                new Vector3(5800, 490, -7400), new Vector3(6500, 490, -5800),
                new Vector3(6500, 490, -9800), new Vector3(7000, 490, -1800),
                new Vector3(7000, 490, -2200), new Vector3(7000, 490, -9400),
                new Vector3(7800, 490, -7600), new Vector3(7800, 490, -13800),
                new Vector3(8000, 490, -4000), new Vector3(8000, 490, -5800),
                new Vector3(8500, 490, -6000), new Vector3(8500, 490, -10000),
                new Vector3(9000, 490, -2000), new Vector3(9000, 490, -2400),
                new Vector3(9000, 490, -6000), new Vector3(9000, 490, -7800),
                new Vector3(9800, 490, -14000), new Vector3(9800, 490, -7800),
                new Vector3(10000, 490, -4200), new Vector3(10000, 490, -6000),
                new Vector3(10000, 490, -5800), new Vector3(10500, 490, -10200),
                new Vector3(10500, 490, -6200), new Vector3(11000, 490, -2500),
                new Vector3(11000, 490, -2600), new Vector3(11800, 490, -8000),
                new Vector3(11800, 490, -14200), new Vector3(12000, 490, -12000),
                new Vector3(12000, 490, -6200), new Vector3(12000, 490, -4400),
                new Vector3(12500, 490, -10500), new Vector3(12500, 490, -6400),
                new Vector3(13000, 490, -2800), new Vector3(13000, 490, -6600),
                new Vector3(13800, 490, -14500), new Vector3(13800, 490, -8200),
                new Vector3(14000, 490, -4600), new Vector3(14000, 490, -6500),
                new Vector3(14500, 490, -10800), new Vector3(14500, 490, -6600),
                new Vector3(15000, 490, -3000), new Vector3(15000, 490, -4600),
                new Vector3(15800, 490, -14800), new Vector3(15800, 490, -8400),
                new Vector3(16000, 490, -4800)
            };

                

            foreach (var pos in posicionesArboles)
            {
                var arbol = new ArbolModel1(content, contentFolder3D, contentFolderEffects);
                arbol.Initialize(pos);
                _arboles.Add(arbol);
            }
            
            foreach (var pos in posicionesArboles2)
            {
                var arbol = new ArbolModel2(content, contentFolder3D, contentFolderEffects);
                arbol.Initialize(pos);
                _arboles2.Add(arbol);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach (var land in _elementosL)
                land.Draw(gameTime, view, projection);

            foreach (var house in _elementosH)
                house.Draw(gameTime, view, projection);

            foreach (var arbol in _arboles)
                arbol.Draw(gameTime, view, projection);
            
            foreach (var arbol2 in _arboles2)
                arbol2.Draw(gameTime, view, projection);
        }
    }
}
