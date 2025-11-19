#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;
#endregion

namespace TGC.MonoGame.TP;

public class ElementosLand
{
    private List<GameObject> _gameElements;
    public ElementosLand(ContentManager content)
    {
        // Inicializo las variables
        _gameElements = new List<GameObject>();
        // Con 0.003f queda bien
        float scaleBushModel0 = 0.003f;
        float scaleHouseModel0 = 0.003f;
        // float scaleProjectileModel0 = 0.001f;
        // Con 0.003f queda como una piedra más chica que la rueda.
        // Si fuera 0.03f queda del tamaño del tanque, se puede tomar como una montaña?
        float scaleStoneModel0 = 0.003f;
        float scaleStoneModel1 = 0.003f;
        // float scaleTankModel1 = 0.01f;
        // 0.003f está bien, aunque podría ser apenas un poquito más grande, para hacerlo más alto al árbol
        // Son los 3 el mismo modelo?
        float scaleTreeModel0 = 0.003f;
        float scaleTreeModel1 = 0.003f;
        float scaleTreeModel2 = 0.015f;
        // Está bien, pero sería más un muro más que una pared
        float scaleWallModel0 = 0.03f;

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
        var posicionesWalls1 = new List<Vector3>
        {
            new Vector3(740, 4, -1000), new Vector3(370, 4, -1000),
            new Vector3(0, 4, -1000), new Vector3(-370, 5, -1000),
            new Vector3(740, 8, -11050), new Vector3(370, 8, -11050),
            new Vector3(0, 8, -11050), new Vector3(-370, 9, -11050),
            new Vector3(-740, 8, -11050), new Vector3(-8890, 5, -1000),
            new Vector3(-2960, 8, -11050), new Vector3(-3330, 9, -11050),
            new Vector3(-3700, 9, -11050), new Vector3(-4070, 9, -11050),
            new Vector3(-4440, 9, -11050), new Vector3(-4810, 8, -11050),
            new Vector3(-5180, 8, -11050), new Vector3(-5550, 8, -11050),
            new Vector3(-5920, 8, -11050), new Vector3(-6290, 8, -11050),
            new Vector3(-2590, 5, -1000), new Vector3(-9260, 5, -1000),
            new Vector3(-2960, 5, -1000), new Vector3(-3330, 5, -1000),
            new Vector3(-3700, 5, -1000), new Vector3(-4070, 5, -1000),
            new Vector3(-4440, 5, -1000), new Vector3(-4810, 5, -1000),
            new Vector3(-5180, 5, -1000), new Vector3(-5550, 5, -1000),
            new Vector3(-5920, 5, -1000), new Vector3(-6290, 5, -1000),
            new Vector3(-6660, 5, -1000), new Vector3(-9630, 5, -1000),
            new Vector3(-8540, 8, -11050), new Vector3(-8890, 9, -11050),
            new Vector3(-9260, 10, -11050), new Vector3(-9630, 11, -11050),
            new Vector3(-9800, 11, -11050), new Vector3(-9800, 5, -1000),
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
        /*
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
        */
        var posicionesArboles2 = new List<Vector3>
        {
            new Vector3(4500, 0, -9500), new Vector3(5000, 0, -1500),
            new Vector3(5000, 0, -2000), new Vector3(5000, 0, -9200),
            new Vector3(6000, 0, -3800), new Vector3(6000, 0, -5500),
            new Vector3(5800, 0, -7400), new Vector3(6500, 0, -5800),
            new Vector3(6500, 0, -9800), new Vector3(7000, 0, -1800),
            new Vector3(7000, 0, -2200), new Vector3(7000, 0, -9400),
            new Vector3(7800, 0, -7600), new Vector3(7800, 0, -13800),
            new Vector3(8000, 0, -4000), new Vector3(8000, 0, -5800),
            new Vector3(8500, 0, -6000), new Vector3(8500, 0, -10000),
            new Vector3(9000, 0, -2000), new Vector3(9000, 0, -2400),
            new Vector3(9000, 0, -6000), new Vector3(9000, 0, -7800),
            new Vector3(9800, 0, -14000), new Vector3(9800, 0, -7800),
            new Vector3(10000, 0, -4200), new Vector3(10000, 0, -6000),
            new Vector3(10000, 0, -5800), new Vector3(10500, 0, -10200),
            new Vector3(10500, 0, -6200), new Vector3(11000, 0, -2500),
            new Vector3(11000, 0, -2600), new Vector3(11800, 0, -8000),
            new Vector3(11800, 0, -14200), new Vector3(12000, 0, -12000),
            new Vector3(12000, 0, -6200), new Vector3(12000, 0, -4400),
            new Vector3(12500, 0, -10500), new Vector3(12500, 0, -6400),
            new Vector3(13000, 0, -2800), new Vector3(13000, 0, -6600),
            new Vector3(13800, 0, -14500), new Vector3(13800, 0, -8200),
            new Vector3(14000, 0, -4600), new Vector3(14000, 0, -6500),
            new Vector3(14500, 0, -10800), new Vector3(14500, 0, -6600),
            new Vector3(15000, 0, -3000), new Vector3(15000, 0, -4600),
            new Vector3(15800, 0, -14800), new Vector3(15800, 0, -8400),
            new Vector3(16000, 0, -4800), new Vector3(-3500, 0, -3000),
            new Vector3(-6200, 0, -3000), new Vector3(-9700, 0, -7000),
            new Vector3(-2800, 0, -9800), new Vector3(-7800, 0, 11800),
            new Vector3(-9800, 0, 13700)
        };
        
        var posicionesCasasModelo1 = new List<Vector3>
        {
            new Vector3(225, 0, -150),      new Vector3(225, 0, -165),
            new Vector3(225, 0, -180),      new Vector3(225, 0, -135),
            new Vector3(225, 0, -120),      new Vector3(225, 0, -195),

            new Vector3(-725, 0, -400),     new Vector3(-725, 0, -385),
            new Vector3(-725, 0, -415),     new Vector3(-725, 0, -370),
            new Vector3(-725, 0, -430),     new Vector3(-725, 0, -355),
        };
        var posicionesCasasModelo2 = new List<Vector3>
        {
            new Vector3(200, 0, -150),      new Vector3(200, 0, -165),
            new Vector3(200, 0, -180),      new Vector3(200, 0, -135),
            new Vector3(200, 0, -120),      new Vector3(200, 0, -195),

            new Vector3(-700, 0, -400),     new Vector3(-700, 0, -385),
            new Vector3(-700, 0, -415),     new Vector3(-700, 0, -370),
            new Vector3(-700, 0, -430),     new Vector3(-700, 0, -355),
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
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var arbol = new Tree(ContentLoader.GetModel("tree", 0), finalPos, scaleTreeModel0, 0f);
            _gameElements.Add(arbol);
        }
        foreach (var pos in posicionesArboles2)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var arbol = new Tree(ContentLoader.GetModel("tree", 1), finalPos, scaleTreeModel1, 0f);
            _gameElements.Add(arbol);
        }
        foreach (var pos in posicionesRocas)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var roca = new Stone(ContentLoader.GetModel("stone", 0), finalPos, scaleStoneModel0);
            _gameElements.Add(roca);
        }
        foreach (var pos in posicionesPiedras)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var piedra = new Stone(ContentLoader.GetModel("stone", 1), finalPos, scaleStoneModel1);
            _gameElements.Add(piedra);
        }
        foreach (var pos in posicionesArbustos)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var bush = new Bush(ContentLoader.GetModel("bush", 0), finalPos, scaleBushModel0);
            _gameElements.Add(bush);
        }
        foreach (var pos in posicionesCasasModelo1)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var casaModelo1 = new House(ContentLoader.GetModel("house", 0), finalPos, House.DefaultScaleHouse, 0);
            Texture2D texture = ContentLoader.GetTexture("house", 0);
            Texture2D normal = ContentLoader.GetNormal("house", 0);
            casaModelo1.SetTexture(texture);
            casaModelo1.SetNormal(normal);
            _gameElements.Add(casaModelo1);
        }
        foreach (var pos in posicionesCasasModelo2) // Cambiado a modelo 1
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var casaModelo2 = new House(ContentLoader.GetModel("house", 0), finalPos, House.DefaultScaleHouse, 180f);
            Texture2D texture = ContentLoader.GetTexture("house", 0);
            Texture2D normal = ContentLoader.GetNormal("house", 0);
            casaModelo2.SetTexture(texture);
            casaModelo2.SetNormal(normal);
            _gameElements.Add(casaModelo2);
        }
        foreach (var pos in posicionesWalls1)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var wall = new Wall(ContentLoader.GetModel("wall", 0), finalPos, scaleWallModel0, 0f);
            _gameElements.Add(wall);
        }
        foreach (var pos in posicionesWalls1Rotated)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var wall = new Wall(ContentLoader.GetModel("wall", 0), finalPos, scaleWallModel0, 90f);
            _gameElements.Add(wall);
        }
        foreach (var pos in posicionesWalls2Rot)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var wall = new Wall(ContentLoader.GetModel("wall", 0), finalPos, scaleWallModel0, 90f);
            _gameElements.Add(wall);
        }
        foreach (var pos in posicionesWalls2)
        {
            Vector3 finalPos = new Vector3(pos.X, Land.Height(pos.X, pos.Z), pos.Z);
            var wall = new Wall(ContentLoader.GetModel("wall", 0), finalPos, scaleWallModel0, 0f);
            _gameElements.Add(wall);
        }

        /* Arbustos de prueba */
        var bushModel0TestHeight = Land.Height(25,30);
        Vector3 bushModel0TestPosition = new Vector3(25f, bushModel0TestHeight, 30f);
        Bush bushModel0Test = new Bush(ContentLoader.GetModel("bush", 0), bushModel0TestPosition, scaleBushModel0);
        _gameElements.Add(bushModel0Test);
        var bushModel1TestHeight = Land.Height(20,30);
        Vector3 bushModel1TestPosition = new Vector3(20f, bushModel1TestHeight, 30f);
        Bush bushModel1Test = new Bush(ContentLoader.GetModel("bush", 1), bushModel1TestPosition, scaleBushModel0);
        _gameElements.Add(bushModel1Test);
        /* Arbustos de prueba */

        /* Casas de prueba */
        var houseModel0TestHeight = Land.Height(50,50);
        Texture2D houseModel0TestTexture = ContentLoader.GetTexture("house", 0);
        Vector3 houseModel0TestPosition = new Vector3(50f, houseModel0TestHeight, 50f);
        House houseModel0Test = new House(ContentLoader.GetModel("house", 0), houseModel0TestPosition, House.DefaultScaleHouse);
        houseModel0Test.SetTexture(houseModel0TestTexture);
        Texture2D houseModel0TextureNormal = ContentLoader.GetNormal("house", 0);
        houseModel0Test.SetNormal(houseModel0TextureNormal);
        _gameElements.Add(houseModel0Test);
        /* Casas de prueba */

        /* Projectiles de prueba
        var projectileModel0TestHeight = Land.Height(20,30);
        Vector3 projectileModel0TestPosition = new Vector3(20f, projectileModel0TestHeight, 30f);
        Vector3 direction = Vector3.Up;
        Projectile projectileModel0Test = new Projectile(ContentLoader.GetModel("projectile", 0), projectileModel0TestPosition, direction, null, 0.001f, 500, scaleProjectileModel0);
        _gameElements.Add(projectileModel0Test);
        /* Projectiles de prueba */

        /* Piedras de prueba */
        /* Piedra 1 */
        var stoneModel0TestHeight = Land.Height(30,30);
        Vector3 stoneModel0TestPosition = new Vector3(30f, stoneModel0TestHeight, 30f);
        Stone stoneModel0Test = new Stone(ContentLoader.GetModel("stone", 0), stoneModel0TestPosition, scaleStoneModel0);
        Texture2D stoneModel0TestTexture = ContentLoader.GetTexture("stone", 2);
        stoneModel0Test.SetTexture(stoneModel0TestTexture);
        _gameElements.Add(stoneModel0Test);
        /* Piedra 2 */
        var stoneModel1TestHeight = Land.Height(30,35);
        Vector3 stoneModel1TestPosition = new Vector3(30f, stoneModel1TestHeight, 35f);
        Stone stoneModel1Test = new Stone(ContentLoader.GetModel("stone", 1), stoneModel1TestPosition, scaleStoneModel1);
        Texture2D stoneModel1TestTexture = ContentLoader.GetTexture("stone", 2);
        stoneModel1Test.SetTexture(stoneModel1TestTexture);
        _gameElements.Add(stoneModel1Test);
        /* Piedra 3 */
        var stoneModel2TestHeight = Land.Height(30,40);
        Vector3 stoneModel2TestPosition = new Vector3(30f, stoneModel2TestHeight, 40f);
        Stone stoneModel2Test = new Stone(ContentLoader.GetModel("stone", 2), stoneModel2TestPosition, scaleStoneModel1);
        Texture2D stoneModel2TestTexture = ContentLoader.GetTexture("stone", 2);
        stoneModel2Test.SetTexture(stoneModel2TestTexture);
        _gameElements.Add(stoneModel2Test);
        /* Piedra 4 */
        var stoneModel3TestHeight = Land.Height(30,45);
        Vector3 stoneModel3TestPosition = new Vector3(30f, stoneModel3TestHeight, 45f);
        Stone stoneModel3Test = new Stone(ContentLoader.GetModel("stone", 3), stoneModel3TestPosition, scaleStoneModel0);
        Texture2D stoneModel3TestTexture = ContentLoader.GetTexture("stone", 2);
        stoneModel3Test.SetTexture(stoneModel3TestTexture);
        _gameElements.Add(stoneModel3Test);
        /* Piedra 5 */
        var stoneModel4TestHeight = Land.Height(30,50);
        Vector3 stoneModel4TestPosition = new Vector3(30f, stoneModel4TestHeight, 50f);
        Stone stoneModel4Test = new Stone(ContentLoader.GetModel("stone", 4), stoneModel4TestPosition, scaleStoneModel1);
        Texture2D stoneModel4TestTexture = ContentLoader.GetTexture("stone", 2);
        stoneModel4Test.SetTexture(stoneModel4TestTexture);
        _gameElements.Add(stoneModel4Test);
        /* Piedra 6 */
        var stoneModel5TestHeight = Land.Height(30,55);
        Vector3 stoneModel5TestPosition = new Vector3(30f, stoneModel5TestHeight, 55f);
        Stone stoneModel5Test = new Stone(ContentLoader.GetModel("stone", 5), stoneModel5TestPosition, scaleStoneModel1);
        Texture2D stoneModel5TestTexture = ContentLoader.GetTexture("stone", 2);
        stoneModel5Test.SetTexture(stoneModel5TestTexture);
        _gameElements.Add(stoneModel5Test);
        /* Piedras de prueba */

        /* Árboles de prueba */
        /* Árbol 1 */
        var treeModel0TestHeight = Land.Height(35,30);
        Vector3 treeModel0TestPosition = new Vector3(35f, treeModel0TestHeight, 30f);
        Tree treeModel0Test = new Tree(ContentLoader.GetModel("tree", 3), treeModel0TestPosition, scaleTreeModel0);
        _gameElements.Add(treeModel0Test);
        /* Árbol 2 */
        var treeModel1TestHeight = Land.Height(35,35);
        Vector3 treeModel1TestPosition = new Vector3(35f, treeModel1TestHeight, 35f);
        Tree treeModel1Test = new Tree(ContentLoader.GetModel("tree", 3), treeModel1TestPosition, scaleTreeModel1);
        _gameElements.Add(treeModel1Test);
        /* Árbol 3 */
        var treeModel2TestHeight = Land.Height(35,40);
        Vector3 treeModel2TestPosition = new Vector3(35f, treeModel2TestHeight, 40f);
        Tree treeModel2Test = new Tree(ContentLoader.GetModel("tree", 3), treeModel2TestPosition, scaleTreeModel2);
        Texture2D treeModel3TestTexture = ContentLoader.GetTexture("tree", 0);
        treeModel2Test.SetTexture(treeModel3TestTexture);
        _gameElements.Add(treeModel2Test);
        /* Árboles de prueba */

        /* Paredes de prueba*/
        var wallModel0TestHeight = Land.Height(40,40);
        Vector3 wallModel0TestPosition = new Vector3(40f, wallModel0TestHeight, 40f);
        Wall wallModel0Test = new Wall(ContentLoader.GetModel("wall", 0), wallModel0TestPosition, scaleWallModel0);
        Texture2D wallModel0TestTexture = ContentLoader.GetTexture("wall", 0);
        wallModel0Test.SetTexture(wallModel0TestTexture);
        _gameElements.Add(wallModel0Test);
        /* Paredes de prueba*/

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