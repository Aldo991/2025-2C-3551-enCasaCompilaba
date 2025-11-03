#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

#endregion

namespace TGC.MonoGame.TP
{
    public class ElementosLand
    {
        private List<Land> _landElements;
        private List<House> _houses;
        private List<Tree> _trees;
        private List<Stone> _stones;
        private List<Bush> _bushes;
        private List<Wall> _walls;
        private GameManager _gameManager;
        private List<Vector3> _landVertices; // vértices del terreno en world-space para muestreo

        public ElementosLand(ContentManager content,
            string contentFolderEffects,
            GameManager gameManager)
        {
            // Inicializo las variables
            _gameManager = gameManager;
            _landElements = new List<Land>();
            _houses = new List<House>();
            _trees = new List<Tree>();
            _stones = new List<Stone>();
            _bushes = new List<Bush>();
            _walls = new List<Wall>();

            // Terreno
            var posicionesWalls2Rot = new List<Vector3>
            {
                new Vector3(-22500, 0, 22700), new Vector3(-22500, 0, 20500),
                new Vector3(-22500, -200, 18300), new Vector3(-22500, 0, 16100),
                new Vector3(-22500, -250, 13900), new Vector3(-22500, -250, 11700),
                new Vector3(-22500, -250, 9500), new Vector3(-22500, -250, 7300),
                new Vector3(-22500, -250, 5100), new Vector3(-22500, 0, 2900),
                new Vector3(-22500, 0, 700), new Vector3(-22500, 0, -1500),
                new Vector3(22500, -250, 13900), new Vector3(22500, -250, 11700),
                new Vector3(22500, -250, 9500), new Vector3(22500, -250, 7300),
                new Vector3(22500, -250, 5100), new Vector3(22500, 0, 2900),
                new Vector3(22500, 0, 700), new Vector3(22500, 0, -1500),
                new Vector3(22500, 0, -3700), new Vector3(22500, 0, -5900),
                new Vector3(22500, 0, -8100)
            };
            var posicionesWalls2 = new List<Vector3>
            {
                new Vector3(-22000, 0, 22330), new Vector3(-19800, -150, 22330),
                new Vector3(-17600, -150, 22330), new Vector3(-15400, -150, 22330),
                new Vector3(-13200, -150, 22330), new Vector3(-11000, -150, 22330),
                new Vector3(-8800, -150, 22330), new Vector3(-6600, -150, 22330),
                new Vector3(-4400, -150, 22330), new Vector3(-2200, -150, 22330),
                new Vector3(0, -150, 22330), new Vector3(2200, -150, 22330),
                new Vector3(4400, -150, 22330)
            };

            var land = new Land(ContentLoader.GetModel("land", 0), new Vector3(0, -990, 0), 9f, 0f);
            _landElements.Add(land);
            // Precalcular vértices del terreno una sola vez
            _landVertices = GetTransformedVertices(land.Model, land.World);

            var stone1 = new Stone(ContentLoader.GetModel("stone", 0), new Vector3(3000, 770, 0));
            _stones.Add(stone1);

            var stone2 = new Stone(ContentLoader.GetModel("stone", 1), new Vector3(-3000, 690, 0));
            _stones.Add(stone2);

            var posicionesWalls1 = new List<Vector3>
            {
                new Vector3(740, 490, -1000), new Vector3(370, 490, -1000),
                new Vector3(0, 490, -1000), new Vector3(-370, 490, -1000),
                new Vector3(740, 850, -11050), new Vector3(370, 850, -11050),
                new Vector3(0, 850, -11050), new Vector3(-370, 850, -11050),
                new Vector3(-740, 850, -11050), new Vector3(-8890, 560, -1000),
                new Vector3(-2960, 850, -11050), new Vector3(-3330, 900, -11050),
                new Vector3(-3700, 950, -11050), new Vector3(-4070, 950, -11050),
                new Vector3(-4440, 900, -11050), new Vector3(-4810, 850, -11050),
                new Vector3(-5180, 850, -11050), new Vector3(-5550, 850, -11050),
                new Vector3(-5920, 850, -11050), new Vector3(-6290, 850, -11050),
                new Vector3(-2590, 500, -1000), new Vector3(-9260, 560, -1000),
                new Vector3(-2960, 500, -1000), new Vector3(-3330, 500, -1000),
                new Vector3(-3700, 500, -1000), new Vector3(-4070, 500, -1000),
                new Vector3(-4440, 500, -1000), new Vector3(-4810, 500, -1000),
                new Vector3(-5180, 500, -1000), new Vector3(-5550, 500, -1000),
                new Vector3(-5920, 500, -1000), new Vector3(-6290, 500, -1000),
                new Vector3(-6660, 500, -1000), new Vector3(-9630, 560, -1000),
                new Vector3(-8540, 850, -11050), new Vector3(-8890, 950, -11050),
                new Vector3(-9260, 1000, -11050), new Vector3(-9630, 1100, -11050),
                new Vector3(-9800, 1130, -11050), new Vector3(-9800, 560, -1000),
            };

            var posicionesWalls1Rotated = new List<Vector3>
            {
                new Vector3(940, 490, -1200), new Vector3(940, 490, -1570),
                new Vector3(940, 490, -1940), new Vector3(940, 590, -2310),
                new Vector3(940, 760, -2680), new Vector3(940, 1050, -3050),
                new Vector3(940, 1400, -3420), new Vector3(940, 1500, -3790),
                new Vector3(940, 1400, -4160), new Vector3(940, 1320, -4530),
                new Vector3(940, 1000, -4900), new Vector3(940, 850, -5270),
                new Vector3(940, 750, -5640), new Vector3(940, 650, -6010),
                new Vector3(940, 650, -6380), new Vector3(940, 650, -6750),
                new Vector3(940, 650, -7120), new Vector3(940, 710, -7490),
                new Vector3(940, 760, -7860), new Vector3(940, 810, -8230),
                new Vector3(940, 850, -8600), new Vector3(940, 910, -8970),
                new Vector3(940, 950, -9340), new Vector3(940, 930, -9710),
                new Vector3(940, 850, -10080), new Vector3(940, 850, -10450),
                new Vector3(940, 850, -10820), new Vector3(-10000, 1130, -10820),
                new Vector3(-10000, 560, -1200), new Vector3(-10000, 615, -1570),
                new Vector3(-10000, 650, -1940), new Vector3(-10000, 675, -2310),
                new Vector3(-10000, 750, -2680), new Vector3(-10000, 780, -3050),
                new Vector3(-10000, 840, -3420), new Vector3(-10000, 880, -3790),
                new Vector3(-10000, 925, -4160), new Vector3(-10000, 950, -4530),
                new Vector3(-10000, 1050, -4900), new Vector3(-10000, 1150, -5270),
                new Vector3(-10000, 1200, -5640), new Vector3(-10000, 1150, -6010),
                new Vector3(-10000, 1050, -6380), new Vector3(-10000, 950, -6750),
                new Vector3(-10000, 850, -7120), new Vector3(-10000, 850, -7490),
                new Vector3(-10000, 850, -7860), new Vector3(-10000, 850, -8230),
                new Vector3(-10000, 850, -8600), new Vector3(-10000, 850, -8970),
                new Vector3(-10000, 950, -9340), new Vector3(-10000, 950, -9710),
                new Vector3(-10000, 980, -10080), new Vector3(-10000, 1050, -10450),
            };
            //80 arboles de mismo modelo (MAS O MENOS)
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
                new Vector3(16000, 490, 4800), new Vector3(-3200, 490, -2500),
                new Vector3(-9200, 940, -6500), new Vector3(-2500, 730, -9500),
                new Vector3(-7500, -400, 12000), new Vector3(-9500, -400, 14000),
                new Vector3(-14000, -400, 12000), new Vector3(-12000, -400, 14000),
                new Vector3(-10000, -400, 17000), new Vector3(-13000, -400, 17000)
            };
            var posicionesRocas = new List<Vector3>
            {
                new Vector3(1000, 580, 1000), new Vector3(2000, 590, 5000),
                new Vector3(2000, 500, 3400), new Vector3(2500, 590, 5400),
                new Vector3(2500, 640, 9200), new Vector3(3000, 740, 1200),
                new Vector3(3000, 690, 1800), new Vector3(3000, 690, 9000),
                new Vector3(4000, 690, 3600), new Vector3(4000, 690, 5200),
                new Vector3(3800, 690, 7200), new Vector3(4500, 690, 5600),
                new Vector3(4500, 690, 9500), new Vector3(5000, 690, 1500),
                new Vector3(5000, 600, 2000), new Vector3(5000, 690, 9200),
                new Vector3(6000, 690, 3800), new Vector3(6000, 690, 5500),
                new Vector3(5800, 700, 7400), new Vector3(6500, 590, 5800),
                new Vector3(6500, 1290, 9800), new Vector3(7000, 590, 1800),
                new Vector3(7000, 690, 2200), new Vector3(7000, 800, 9400),
                new Vector3(7800, 890, 7600), new Vector3(7800, 1890, 13800),
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
                new Vector3(15800, 950, 14800), new Vector3(15800, 420, 8400),
                new Vector3(16000, 420, 4800), new Vector3(100, 740, -8000),
            };

            var posicionesPiedras = new List<Vector3>
            {
                new Vector3(-1000, 390, 1000), new Vector3(-2000, 300, 5000),
                new Vector3(-2000, 390, 3400), new Vector3(-2500, 300, 5400),
                new Vector3(-2500, 390, 9200), new Vector3(-3000, 390, 1200),
                new Vector3(-3000, 390, 1800), new Vector3(-3000, 300, 9000),
                new Vector3(-4000, 190, 3600), new Vector3(-4000, 190, 5200),
                new Vector3(-3800, 190, 7200), new Vector3(-4500, -190, 5600),
                new Vector3(-4500, -190, 9500), new Vector3(-5000, -190, 1500),
                new Vector3(-5000, 290, 2000), new Vector3(-5000, 190, 9200),
                new Vector3(-6000, 390, 3800), new Vector3(-6000, 190, 5500),
                new Vector3(-5800, -290, 7400), new Vector3(-6500, -290, 5800),
                new Vector3(-6500, -190, 9800), new Vector3(-7000, 290, 1800),
                new Vector3(-7000, -190, 2200), new Vector3(-7000, -290, 9400),
                //cuadrante x>0 y z<0
                new Vector3(1450, 570, -900), new Vector3(2450, 670, -4900),
                new Vector3(2450, 670, -3300), new Vector3(2950, 660, -5300),
                new Vector3(2950, 900, -9100), new Vector3(3450, 670, -1100),
                new Vector3(3450, 640, -1700), new Vector3(3450, 870, -8900),
                new Vector3(4450, 670, -3500), new Vector3(4450, 1250, -5100),
                new Vector3(4250, 670, -7100), new Vector3(4950, 1250, -5500),
                new Vector3(4950, 740, -9400), new Vector3(5450, 700, -1400),
                new Vector3(5450, 610, -1900), new Vector3(5450, 740, -9100),
                new Vector3(6450, 1550, -3700), new Vector3(6450, 1500, -5400),
                new Vector3(6250, 1506, -7300), new Vector3(6950, 1580, -5700),
                new Vector3(6950, 1550, -9700), new Vector3(7450, 940, -1700),
                new Vector3(7450, 540, -2100), new Vector3(7450, 940, -9300),
                new Vector3(8250, 780, -7500), new Vector3(8250, 2540, -13700),
                new Vector3(8450, 740, -3900), new Vector3(8450, 940, -5700),
                new Vector3(8950, 559, -5900), new Vector3(8950, 2350, -9900),
                new Vector3(9450, 540, -1900), new Vector3(9450, 940, -2300),
                new Vector3(9450, 540, -5900), new Vector3(9450, 1640, -7700),
                new Vector3(10250, 2720, -13900), new Vector3(10250, 1640, -7700),
                new Vector3(10450, 540, -4100), new Vector3(10450, 940, -5900),
                new Vector3(10450, 540, -5700), new Vector3(10950, 2500, -10100),
                new Vector3(10950, 1040, -6100), new Vector3(11450, 940, -2400),
                new Vector3(11450, 1040, -2500), new Vector3(12250, 2150, -7900),
                new Vector3(12250, 2650, -14100), new Vector3(12450, 2450, -11900),
                new Vector3(12450, 1040, -6100), new Vector3(12450, 940, -4300),
                new Vector3(12950, 2550, -10400), new Vector3(12950, 940, -6300),
                new Vector3(13450, 1040, -2700), new Vector3(13450, 940, -6500),
                new Vector3(14250, 3250, -14400), new Vector3(14250, 1950, -8100),
                new Vector3(14450, 780, -4500), new Vector3(14450, 790, -6400),
                new Vector3(14950, 2950, -10700), new Vector3(14950, 790, -6500),
                new Vector3(15450, 640, -2900), new Vector3(15450, 640, -4500),
                new Vector3(16250, 3250, -14700), new Vector3(16250, 2640, -8300),
                new Vector3(16450, 540, -4700), new Vector3(-1800, 540, -2000),
                new Vector3(-1700, 600, -4000), new Vector3(-1900, 540, -5500),
                new Vector3(-1800, 540, -7000), new Vector3(-800, 540, -6000),
                new Vector3(-500, 540, -3200), new Vector3(-2800, 730, -2500),
                new Vector3(-3500, 565, -4500), new Vector3(-4200, 565, -7200),
                new Vector3(-5000, 565, -2800), new Vector3(-5800, 565, -5000),
                new Vector3(-6600, 565, -2200), new Vector3(-7200, 565, -8000),
                new Vector3(-8000, 100, -3000), new Vector3(-8800, 565, -6000),
                new Vector3(-9500, 940, -2500), new Vector3(-10200, 565, -7000),
                new Vector3(-3000, 565, -8500), new Vector3(-6000, 565, -9800),
                new Vector3(-8700, 565, 11000), new Vector3(-10500, 100, 13500)
            };
            var posicionesArboles2 = new List<Vector3>
            {
                new Vector3(1500, 520, -1500), new Vector3(2000, 620, -5000),
                new Vector3(2000, 620, -3400), new Vector3(2500, 610, -5400),
                new Vector3(2500, 850, -9200), new Vector3(3000, 620, -1200),
                new Vector3(3000, 590, -1800), new Vector3(3000, 820, -9000),
                new Vector3(4000, 620, -3600), new Vector3(4000, 1200, -5200),
                new Vector3(3800, 620, -7200), new Vector3(4500, 1200, -5600),
                new Vector3(4500, 690, -9500), new Vector3(5000, 650, -1500),
                new Vector3(5000, 560, -2000), new Vector3(5000, 690, -9200),
                new Vector3(6000, 1500, -3800), new Vector3(6000, 1450, -5500),
                new Vector3(5800, 1456, -7400), new Vector3(6500, 1530, -5800),
                new Vector3(6500, 1500, -9800), new Vector3(7000, 890, -1800),
                new Vector3(7000, 490, -2200), new Vector3(7000, 890, -9400),
                new Vector3(7800, 730, -7600), new Vector3(7800, 2490, -13800),
                new Vector3(8000, 690, -4000), new Vector3(8000, 890, -5800),
                new Vector3(8500, 509, -6000), new Vector3(8500, 2300, -10000),
                new Vector3(9000, 490, -2000), new Vector3(9000, 890, -2400),
                new Vector3(9000, 490, -6000), new Vector3(9000, 1590, -7800),
                new Vector3(9800, 2670, -14000), new Vector3(9800, 1590, -7800),
                new Vector3(10000, 490, -4200), new Vector3(10000, 890, -6000),
                new Vector3(10000, 490, -5800), new Vector3(10500, 2450, -10200),
                new Vector3(10500, 990, -6200), new Vector3(11000, 890, -2500),
                new Vector3(11000, 990, -2600), new Vector3(11800, 2100, -8000),
                new Vector3(11800, 2600, -14200), new Vector3(12000, 2400, -12000),
                new Vector3(12000, 990, -6200), new Vector3(12000, 890, -4400),
                new Vector3(12500, 2500, -10500), new Vector3(12500, 890, -6400),
                new Vector3(13000, 990, -2800), new Vector3(13000, 890, -6600),
                new Vector3(13800, 3200, -14500), new Vector3(13800, 1900, -8200),
                new Vector3(14000, 730, -4600), new Vector3(14000, 740, -6500),
                new Vector3(14500, 2900, -10800), new Vector3(14500, 740, -6600),
                new Vector3(15000, 590, -3000), new Vector3(15000, 590, -4600),
                new Vector3(15800, 3200, -14800), new Vector3(15800, 2590, -8400),
                new Vector3(16000, 490, -4800), new Vector3(-3500, 490, -3000),
                new Vector3(-6200, 490, -3000), new Vector3(-9700, 940, -7000),
                new Vector3(-2800, 730, -9800), new Vector3(-7800, -130, 11800),
                new Vector3(-9800, -400, 13700)
            };
            var posicionesCasasModelo1 = new List<Vector3>
            {
                new Vector3(0, 490, 0), new Vector3(0, 490, 1490),
                new Vector3(-8100, -480, 14390), new Vector3(-10100, -550, 16390),
                new Vector3(-3000, 490, -2000), new Vector3(-3000, 700, -6000),
                new Vector3(-3000, 830, -10500), new Vector3(1050, 490, 550)
            };
            var posicionesCasasModelo3 = new List<Vector3>
            {
                new Vector3(-8000, -400, 17090), new Vector3(100, 490, -2000),
                new Vector3(100, 490, -6000), new Vector3(100, 900, -10000),
                new Vector3(-6000, 600, -2000), new Vector3(-9000, 940, -10000),
                new Vector3(-14100, -580, 14390)

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
                new Vector3(-6000, 490, -3800), new Vector3(-7800, 490, -13800),
                new Vector3(-6500, 590, -9800), new Vector3(-7000, 490, -1800),
                new Vector3(-7000, 490, -2200), new Vector3(-7000, 490, -9400),
                new Vector3(-8000, 490, -4000), new Vector3(-8500, 940, -10000),
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
                var arbol = new Tree(ContentLoader.GetModel("tree", 0), pos, 0.3f, 0f);
                _trees.Add(arbol);
            }

            foreach (var pos in posicionesArboles2)
            {
                var arbol = new Tree(ContentLoader.GetModel("tree", 1), pos, 0.3f, 0f);
                _trees.Add(arbol);
            }

            foreach (var pos in posicionesRocas)
            {
                var roca = new Stone(ContentLoader.GetModel("stone", 0), pos);
                _stones.Add(roca);
            }

            foreach (var pos in posicionesPiedras)
            {
                var piedra = new Stone(ContentLoader.GetModel("stone", 1), pos);
                _stones.Add(piedra);
            }

            foreach (var pos in posicionesArbustos)
            {
                var bush = new Bush(ContentLoader.GetModel("bush", 0), pos);
                _bushes.Add(bush);
            }

            foreach (var house in posicionesCasasModelo1)
            {
                var casaModelo1 = new House(ContentLoader.GetModel("house", 0), house, 0.3f, 0);
                _houses.Add(casaModelo1);
            }

            foreach (var house in posicionesCasasModelo3)
            {
                var casaModelo3 = new House(ContentLoader.GetModel("house", 1), house, 1f, 0f);
                _houses.Add(casaModelo3);
            }

            foreach (var wallPos in posicionesWalls1)
            {
                var wall = new Wall(ContentLoader.GetModel("wall", 0), wallPos, 6f, 0f);
                _walls.Add(wall);
            }

            foreach (var wallPos in posicionesWalls1Rotated)
            {
                var wall = new Wall(ContentLoader.GetModel("wall", 0), wallPos, 6f, 90f);
                _walls.Add(wall);
            }

            foreach (var wallsRot in posicionesWalls2Rot)
            {
                var wall = new Wall(ContentLoader.GetModel("wall", 0), wallsRot, 6f, 90f);
                _walls.Add(wall);
            }

            foreach (var wallPos in posicionesWalls2)
            {
                var wall = new Wall(ContentLoader.GetModel("wall", 0), wallPos, 6f, 0f);
                _walls.Add(wall);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach (var land in _landElements)
                land.Draw(gameTime, view, projection);
            foreach (var house in _houses)
                house.Draw(gameTime, view, projection);
            foreach (var arbol in _trees)
                arbol.Draw(gameTime, view, projection);
            foreach (var stone in _stones)
                stone.Draw(gameTime, view, projection);
            foreach (var bush in _bushes)
                bush.Draw(gameTime, view, projection);
            foreach (var wall in _walls)
                wall.Draw(gameTime, view, projection);
            /*
            */
        }

        public bool CheckCollisionMesh(GameObject tank, Vector3 newPosition)
        {

            var stonesToRemove = new List<Stone>();
            var bushesToRemove = new List<Bush>();
            if (tank is not Tank t)
                return false;

            // Crear world matrix hipotética del tanque en su nueva posición
            var newWorld = Matrix.CreateScale(tank.Scale) *
                           Matrix.CreateRotationY(tank.Rotation) *
                           Matrix.CreateTranslation(newPosition);

            var tankVertices = GetTransformedVertices(t.Model, newWorld);

            // Comparar con cada casa
            foreach (var stone in _stones)
            {
                float distance = Vector3.Distance(newPosition, stone.Position);
                if (distance < tank.CollisionRadius + stone.CollisionRadius)
                {
                    stonesToRemove.Add(stone);
                }
            }

            foreach (var s in stonesToRemove)
                _stones.Remove(s);

            // --- Collisiones con arbustos ---
            foreach (var bush in _bushes)
            {
                float distance = Vector3.Distance(newPosition, bush.Position);
                if (distance < tank.CollisionRadius + bush.CollisionRadius)
                {
                    bushesToRemove.Add(bush);
                }
            }

            foreach (var b in bushesToRemove)
                _bushes.Remove(b);

            // --- Colisiones con muros ---
            foreach (var wall in _walls)
            {
                float distance = Vector3.Distance(newPosition, wall.Position);
                if (distance < tank.CollisionRadius + wall.CollisionRadius)
                    return true; // Detener movimiento
            }

            // --- Colisiones con casas ---
            foreach (var house in _houses)
            {
                float distance = Vector3.Distance(newPosition, house.Position);
                if (distance < tank.CollisionRadius + house.CollisionRadius)
                    return true; // Detener movimiento
            }

            // --- Colisiones con árboles ---
            foreach (var tree in _trees)
            {
                float distance = Vector3.Distance(newPosition, tree.Position);
                if (distance < tank.CollisionRadius + tree.CollisionRadius)
                    return true; // Detener movimiento
            }

            return stonesToRemove.Count > 0 || bushesToRemove.Count > 0;
        }

        // Resuelve la colisión del tanque contra el entorno usando esferas sencillas y empuje (push-out).

        // Devuelve una altura de suelo aproximada para (x,z) promediando los vértices más cercanos
        // Devuelve una altura de suelo aproximada en (x,z) usando un plano de los 3 v�rtices m�s cercanos.
        public float SampleGroundHeight(float x, float z)
        {
            if (_landVertices == null || _landVertices.Count < 3) return 0f;
            // Buscar 3 m�s cercanos en XZ
            int i1 = -1, i2 = -1, i3 = -1;
            float d1 = float.MaxValue, d2 = float.MaxValue, d3 = float.MaxValue;
            for (int i = 0; i < _landVertices.Count; i++)
            {
                var v = _landVertices[i];
                float dx = v.X - x;
                float dz = v.Z - z;
                float d2i = dx * dx + dz * dz;
                if (d2i < d1)
                {
                    d3 = d2;
                    i3 = i2;
                    d2 = d1;
                    i2 = i1;
                    d1 = d2i;
                    i1 = i;
                }
                else if (d2i < d2)
                {
                    d3 = d2;
                    i3 = i2;
                    d2 = d2i;
                    i2 = i;
                }
                else if (d2i < d3)
                {
                    d3 = d2i;
                    i3 = i;
                }
            }

            if (i1 < 0 || i2 < 0 || i3 < 0) return 0f;
            var p1 = _landVertices[i1];
            var p2 = _landVertices[i2];
            var p3 = _landVertices[i3];
            // Plano por p1,p2,p3: n = (p2-p1) x (p3-p1)
            var u = p2 - p1;
            var v3 = p3 - p1;
            var n = Vector3.Cross(u, v3);
            if (Math.Abs(n.Y) < 1e-5f)
            {
                // Plano casi vertical: usar promedio ponderado de Y de los 3 cercanos
                float w1 = 1f / (d1 + 1e-4f);
                float w2 = 1f / (d2 + 1e-4f);
                float w3 = 1f / (d3 + 1e-4f);
                return (p1.Y * w1 + p2.Y * w2 + p3.Y * w3) / (w1 + w2 + w3);
            }

            // Ecuaci�n del plano: A(x-x1)+B(y-y1)+C(z-z1)=0, con n=(A,B,C)
            float A = n.X, B = n.Y, C = n.Z;
            float y = (-A * (x - p1.X) - C * (z - p1.Z)) / B + p1.Y;
            // Clamp a rango de los 3 v�rtices para evitar extrapolar demasiado
            float minY = Math.Min(p1.Y, Math.Min(p2.Y, p3.Y));
            float maxY = Math.Max(p1.Y, Math.Max(p2.Y, p3.Y));
            if (y < minY) y = minY;
            else if (y > maxY) y = maxY;
            return y;
        }

        private List<Vector3> GetTransformedVertices(Model model, Matrix world)
        {
            var vertices = new List<Vector3>();

            foreach (var mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var vertexBuffer = part.VertexBuffer;
                    var vertexSize = vertexBuffer.VertexDeclaration.VertexStride;
                    var vertexData = new byte[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(vertexData);

                    for (int i = 0; i < vertexBuffer.VertexCount; i++)
                    {
                        var position = new Vector3(
                            BitConverter.ToSingle(vertexData, i * vertexSize),
                            BitConverter.ToSingle(vertexData, i * vertexSize + 4),
                            BitConverter.ToSingle(vertexData, i * vertexSize + 8)
                        );
                        position = Vector3.Transform(position, world);
                        vertices.Add(position);
                    }
                }
            }

            return vertices;
        }

        private bool IsPointInsideMesh(Vector3 point, List<Vector3> meshVertices)
        {
            Vector3 min = Vector3.One * float.MaxValue;
            Vector3 max = Vector3.One * float.MinValue;

            foreach (var v in meshVertices)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            // Si el punto está dentro de los límites del modelo (en los tres ejes)
            return (point.X >= min.X && point.X <= max.X &&
                    point.Y >= min.Y && point.Y <= max.Y &&
                    point.Z >= min.Z && point.Z <= max.Z);
        }

    }

}


