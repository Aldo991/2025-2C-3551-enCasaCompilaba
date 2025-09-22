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

        private List<Rock1Model> _rocks1 = new();
        private List<stonesModel1> _stones1 = new();
        private List<BushModel1> _bushes;

        private List<HouseModel2> _elementoH3 = new();

        public ElementosLand(ContentManager content, string contentFolder3D, string contentFolderEffects)
        {
            // Terreno
            var land = new LandModel(content, contentFolder3D, contentFolderEffects);
            land.Initialize(new Vector3(0, -990, 0));
            _elementosL.Add(land);
            /*
            var house3 = new HouseModel2(content, contentFolder3D, contentFolderEffects);
            house3.Initialize(new Vector3(-10000, 450, 1000));
            _elementoH3.Add(house3);
            */


            var rock1 = new Rock1Model(content, contentFolder3D, contentFolderEffects);
            rock1.Initialize(new Vector3(3000, 490, 0));
            _rocks1.Add(rock1);

            var stones1 = new stonesModel1(content, contentFolder3D, contentFolderEffects);
            stones1.Initialize(new Vector3(-3000, 490, 0));
            _stones1.Add(stones1);

            // Inicializo los arbustos
            _bushes = new List<BushModel1>();
            
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
                new Vector3(10500, 490, 6200), new Vector3(30000, 490, 2500),
                new Vector3(30000, 490, 2600), new Vector3(11800, 490, 8000),
                new Vector3(11800, 490, 14200), new Vector3(12000, 490, 12000),
                new Vector3(12000, 490, 6200), new Vector3(12000, 490, 4400),
                new Vector3(12500, 490, 10500), new Vector3(12500, 490, 6400),
                new Vector3(13000, 490, 2800), new Vector3(13000, 490, 6600),
                new Vector3(13800, 490, 14500), new Vector3(13800, 490, 8200),
                new Vector3(14000, 490, 4600), new Vector3(14000, 490, 6500),
                new Vector3(14500, 490, 10800), new Vector3(14500, 490, 6600),
                new Vector3(15000, 490, 3000), new Vector3(15000, 490, 4600),
                new Vector3(15800, 490, 14800), new Vector3(15800, 490, 8400),
                new Vector3(16000, 490, 4800), 
                new Vector3(-3200, 490, -2500),
                new Vector3(-5800, 800, -2500),
                new Vector3(-9200, 940, -6500),
                new Vector3(-2500, 730, -9500),
                new Vector3(-7500, 490, 12000),
                new Vector3(-9500, 490, 14000)
            };
            var posicionesRocas = new List<Vector3>
            {   
                new Vector3(1000, 520, 1000), new Vector3(2000, 590, 5000),
                new Vector3(2000, 500, 3400), new Vector3(2500, 590, 5400),
                new Vector3(2500, 640, 9200), new Vector3(3000, 740, 1200),
                new Vector3(3000, 690, 1800), new Vector3(3000, 690, 9000),
                new Vector3(4000, 690, 3600), new Vector3(4000, 690, 5200),
                new Vector3(3800, 690, 7200), new Vector3(4500, 690, 5600),
                new Vector3(4500, 690, 9500), new Vector3(5000, 690, 1500),
                new Vector3(5000, 600, 2000), new Vector3(5000, 600, 9200),
                new Vector3(6000, 690, 3800), new Vector3(6000, 590, 5500),
                new Vector3(5800, 700, 7400), new Vector3(6500, 590, 5800),
                new Vector3(6500, 1290, 9800), new Vector3(7000, 590, 1800),
                new Vector3(7000, 690, 2200), new Vector3(7000, 590, 9400),
                new Vector3(7800, 790, 7600), new Vector3(7800, 590, 13800),
                new Vector3(8000, 640, 200), new Vector3(8500, 640, 1200),
                new Vector3(8800, 640, 2500), new Vector3(9100, -100, 400),
                new Vector3(9400, 640, 5200), new Vector3(9700, -100, 600),
                new Vector3(10000, 0, 1800), new Vector3(10300, 640, 3500),
                new Vector3(10600, 590, 4700), new Vector3(10900, 640, 6000),
                new Vector3(11200, 440, 750), new Vector3(11500, 540, 2200),
                new Vector3(11800, 580, 3800), new Vector3(12100, 640, 5000),
                new Vector3(12400, 630, 6700), new Vector3(12700, -200, 1000),
                new Vector3(13000, 620, 3200), new Vector3(13300, 420, 4600),
                new Vector3(13600, 620, 6000), new Vector3(13900, 420, 7800),
                new Vector3(14000, 260, 4600), new Vector3(14000, 420, 6500),
                new Vector3(14500, 420, 10800), new Vector3(14500, 420, 6600),
                new Vector3(15000, 420, 3000), new Vector3(15000, 420, 4600),
                new Vector3(15800, 420, 14800), new Vector3(15800, 420, 8400),
                new Vector3(16000, 420, 4800),
                new Vector3(100,740,-8000),
            };
            
            var posicionesPiedras = new List<Vector3>
            {
                new Vector3(-1000, 490, 1000), new Vector3(-2000, 490, 5000),
                new Vector3(-2000, 490, 3400), new Vector3(-2500, 490, 5400),
                new Vector3(-2500, 490, 9200), new Vector3(-3000, 490, 1200),
                new Vector3(-3000, 490, 1800), new Vector3(-3000, 490, 9000),
                new Vector3(-4000, 490, 3600), new Vector3(-4000, 490, 5200),
                new Vector3(-3800, 490, 7200), new Vector3(-4500, 490, 5600),
                new Vector3(-4500, 490, 9500), new Vector3(-5000, 490, 1500),
                new Vector3(-5000, 490, 2000), new Vector3(-5000, 490, 9200),
                new Vector3(-6000, 490, 3800), new Vector3(-6000, 490, 5500),
                new Vector3(-5800, 490, 7400), new Vector3(-6500, 490, 5800),
                new Vector3(-6500, 490, 9800), new Vector3(-7000, 490, 1800),
                new Vector3(-7000, 490, 2200), new Vector3(-7000, 490, 9400),
                new Vector3(500, 490, -600), new Vector3(1200, 490, -800),
                new Vector3(1800, 490, -1400), new Vector3(2400, 490, -600),
                new Vector3(3000, 490, -1000), new Vector3(3500, 490, -1600),
                new Vector3(4000, 490, -2000), new Vector3(4500, 490, -2500),
                new Vector3(5000, 490, -3000), new Vector3(5500, 490, -3500),

                new Vector3(600, 490, -2200), new Vector3(1300, 490, -2600),
                new Vector3(1900, 490, -3200), new Vector3(2600, 490, -3800),
                new Vector3(3200, 490, -4200), new Vector3(3800, 490, -4600),
                new Vector3(4400, 490, -5000), new Vector3(5000, 490, -5400),
                new Vector3(5600, 490, -5800), new Vector3(6200, 490, -6200),

                new Vector3(700, 490, -4200), new Vector3(1400, 490, -4800),
                new Vector3(2100, 490, -5200), new Vector3(2800, 490, -5800),
                new Vector3(3500, 490, -6400), new Vector3(4200, 490, -7000),
                new Vector3(4900, 490, -7400), new Vector3(5600, 490, -7800),
                new Vector3(6300, 490, -7600), new Vector3(7000, 490, -7200),

                new Vector3(7500, 490, -200), new Vector3(7800, 490, -1000),
                new Vector3(7600, 490, -3000), new Vector3(7900, 490, -5000),
                new Vector3(8000, 490, -7000),

                new Vector3(-1800,540,-2000),
                new Vector3(-1700,600,-4000),
                new Vector3(-1900,540,-5500),
                new Vector3(-1800,540,-7000),
                new Vector3(-800,540,- 6000),
                new Vector3(-500,540,-3200),
                new Vector3(-2800, 730, -2500),
                new Vector3(-3500, 565, -4500),
                new Vector3(-4200, 565, -7200),
                new Vector3(-5000, 565, -2800),
                new Vector3(-5800, 565, -5000),
                new Vector3(-6600, 565, -2200),
                new Vector3(-7200, 565, -8000),
                new Vector3(-8000, 100, -3000),
                new Vector3(-8800, 565, -6000),
                new Vector3(-9500, 940, -2500),
                new Vector3(-10200, 565, -7000),
                new Vector3(-3000, 565, -8500),
                new Vector3(-6000, 565, -9800),
                new Vector3(-8700, 565, 11000),
                new Vector3(-10500, 100, 13500)



            };
            var posicionesArboles2 = new List<Vector3>
            {
                new Vector3(1000, 490, -1000), new Vector3(2000, 490, -5000),
                new Vector3(2000, 520, -3400), new Vector3(2500, 520, -5400),
                new Vector3(2500, 490, -9200), new Vector3(3000, 490, -1200),
                new Vector3(3000, 490, -1800), new Vector3(3000, 490, -9000),
                new Vector3(4000, 520, -3600), new Vector3(4000, 1200, -5200),
                new Vector3(3800, 520, -7200), new Vector3(4500, 1200, -5600),
                new Vector3(4500, 490, -9500), new Vector3(5000, 490, -1500),
                new Vector3(5000, 490, -2000), new Vector3(5000, 490, -9200),
                new Vector3(6000, 1500, -3800), new Vector3(6000, 1450, -5500),
                new Vector3(5800, 1456, -7400), new Vector3(6500, 1530, -5800),
                new Vector3(6500, 1500, -9800), new Vector3(7000, 890, -1800),
                new Vector3(7000, 490, -2200), new Vector3(7000, 890, -9400),
                new Vector3(7800, 490, -7600), new Vector3(7800, 2490, -13800),
                new Vector3(8000, 490, -4000), new Vector3(8000, 890, -5800),
                new Vector3(8500, 509, -6000), new Vector3(8500, 2000, -10000),
                new Vector3(9000, 490, -2000), new Vector3(9000, 890, -2400),
                new Vector3(9000, 490, -6000), new Vector3(9000, 1590, -7800),
                new Vector3(9800, 2000, -14000), new Vector3(9800, 1590, -7800),
                new Vector3(10000, 490, -4200), new Vector3(10000, 890, -6000),
                new Vector3(10000, 490, -5800), new Vector3(10500, 2300, -10200),
                new Vector3(10500, 990, -6200), new Vector3(11000, 890, -2500),
                new Vector3(11000, 990, -2600), new Vector3(11800, 1890, -8000),
                new Vector3(11800, 2300, -14200), new Vector3(12000, 2200, -12000),
                new Vector3(12000, 990, -6200), new Vector3(12000, 890, -4400),
                new Vector3(12500, 2500, -10500), new Vector3(12500, 890, -6400),
                new Vector3(13000, 990, -2800), new Vector3(13000, 890, -6600),
                new Vector3(13800, 3200, -14500), new Vector3(13800, 1900, -8200),
                new Vector3(14000, 690, -4600), new Vector3(14000, 490, -6500),
                new Vector3(14500, 2900, -10800), new Vector3(14500, 690, -6600),
                new Vector3(15000, 590, -3000), new Vector3(15000, 590, -4600),
                new Vector3(15800, 3200, -14800), new Vector3(15800, 2590, -8400),
                new Vector3(16000, 490, -4800),
                new Vector3(-3500, 490, -3000),   
                new Vector3(-6200, 490, -3000),   
                new Vector3(-9700, 940, -7000),   
                new Vector3(-2800, 730, -9800),   
                new Vector3(-7800, 100, 11800),   
                new Vector3(-9800, 100, 13700) 

            };
            var posicionesCasasModelo1 = new List<Vector3>
            {
                new Vector3(0, 490, 0), new Vector3(0, 490, 1490),
                new Vector3(-10100, 100, 14390), new Vector3(-3000, 490, -2000),
                new Vector3(-3000, 700, -6000), new Vector3(-3000, 830, -10500)
            };
            var posicionesCasasModelo3 = new List<Vector3>
            {   
                new Vector3(-8000, 100, 10090),
                new Vector3(100, 490, -2000), new Vector3(100, 490, -6000),new Vector3(100, 900, -10000),
                new Vector3(-6000, 800, -2000),
                new Vector3(-9000, 940, -10000),

            };
            var posicionesArbustos = new List<Vector3>
            {
                new Vector3(-1000, 490, -1000), new Vector3(-2000, 490, -5000),
                new Vector3(-2000, 490, -3400), new Vector3(-2500, 490, -5400),
                new Vector3(-2500, 730, -9200), new Vector3(-3000, 490, -1200),
                new Vector3(-3000, 490, -1800), new Vector3(-3000, 490, -9000),
                new Vector3(-4000, 490, -3600), new Vector3(-4000, 490, -5200),
                new Vector3(-3800, 490, -7200), new Vector3(-4500, 490, -5600),
                new Vector3(-4500, 490, -9500), new Vector3(-5000, 490, -1500),
                new Vector3(-5000, 490, -2000), new Vector3(-5000, 490, -9200),
                new Vector3(-6000, 490, -3800),
                new Vector3(-6500, 590, -9800), new Vector3(-7000, 490, -1800),
                new Vector3(-7000, 490, -2200), new Vector3(-7000, 490, -9400),
                new Vector3(-7800, 490, -13800),
                new Vector3(-8000, 490, -4000),
                new Vector3(-8500, 940, -10000),
                new Vector3(-9000, 940, -2000), new Vector3(-9000, 940, -2400),
                new Vector3(-9800, 1190, -14000), new Vector3(-9800, 990, -7800),
                new Vector3(-10000, 490, -4200), new Vector3(-10000, 590, -6000),
                new Vector3(-10000, 490, -5800), new Vector3(-10500, 1090, -10200),
                new Vector3(-10500, 490, -6200), new Vector3(-11000, 490, -2500),
                new Vector3(-11000, 490, -2600), new Vector3(-11800, 490, -8000),
                new Vector3(-11800, 1190, -14200), new Vector3(-12000, 1090, -12000),
                new Vector3(-12000, 490, -6200), new Vector3(-12000, 490, -4400),
                new Vector3(-12500, 1190, -10500), new Vector3(-12500, 490, -6400),
                new Vector3(-13000, 490, -2800), new Vector3(-13000, 690, -6600),
                new Vector3(-13800, 1090, -14500), new Vector3(-13800, 990, -8200),
                new Vector3(-14000, 490, -4600), new Vector3(-14000, 490, -6500),
                new Vector3(-14500, 1190, -10800), new Vector3(-14500, 490, -6600),
                new Vector3(-15000, 490, -3000), new Vector3(-15000, 990, -4600),
                new Vector3(-15800, 1000, -14800), new Vector3(-15800, 990, -8400),
                new Vector3(-16000, 1090, -4800)

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

            foreach (var pos in posicionesRocas)
            {
                var roca = new Rock1Model(content, contentFolder3D, contentFolderEffects);
                roca.Initialize(pos);
                _rocks1.Add(roca);
            }
            foreach (var pos in posicionesPiedras)
            {
                var piedra = new stonesModel1(content, contentFolder3D, contentFolderEffects);
                piedra.Initialize(pos);
                _stones1.Add(piedra);
            }
            foreach (var pos in posicionesArbustos)
            {
                var bush = new BushModel1(content, contentFolder3D, contentFolderEffects);
                bush.Initialize(pos);
                _bushes.Add(bush);
            }

            foreach (var house in posicionesCasasModelo1)
            {
                var casaModelo1 = new HouseModel1(content, contentFolder3D, contentFolderEffects);
                casaModelo1.Initialize(house);
                _elementosH.Add(casaModelo1);
            }

            foreach (var house in posicionesCasasModelo3)
            {
                var casaModelo3 = new HouseModel2(content, contentFolder3D, contentFolderEffects);
                casaModelo3.Initialize(house);
                _elementoH3.Add(casaModelo3);
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

            foreach (var arbol in _arboles)
                arbol.Draw(gameTime, view, projection);

            foreach (var rock in _rocks1)
                rock.Draw(gameTime, view, projection);

            foreach (var stone in _stones1)
                stone.Draw(gameTime, view, projection);
            foreach (var bush in _bushes)
                bush.Draw(gameTime, view, projection);

            foreach (var house in _elementoH3)
                house.Draw(gameTime, view, projection);
        }
    }
}
