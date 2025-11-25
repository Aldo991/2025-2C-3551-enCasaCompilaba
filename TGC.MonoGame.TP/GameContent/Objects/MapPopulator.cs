using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public static class MapPopulator
{
    private static readonly int MinX = -1612;
    private static readonly int MaxX = 1587;
    private static readonly int MinY = -1612;
    private static readonly int MaxY = 1587;
    private static int TotalSpawns = 50;

    private static readonly Random rng = new Random(12345); // Seed fijo para reproducibilidad

    public static List<Vector2> Houses { get; private set; }
    public static List<Vector2> BigStones { get; private set; }
    public static List<Vector2> Bushes { get; private set; }
    public static List<Vector2> LittleStones { get; private set; }
    public static List<Vector2> SpawnPosition { get; private set; }

    public static void Generate()
    {
        Houses = GenerateRandomPositions(250);
        BigStones = GenerateRandomPositions(1000);
        Bushes = GenerateRandomPositions(5000);
        LittleStones = GeneratePebblesNearBushes(Bushes, 11000);
    }
    public static int GenerateRandomInt(int min, int max)
    {
        return rng.Next(min, max);
    }

    private static List<Vector2> GenerateRandomPositions(int count)
    {
        var list = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            float x = rng.Next(MinX, MaxX);
            float y = rng.Next(MinY, MaxY);
            list.Add(new Vector2(x, y));
        }

        return list;
    }

    private static List<Vector2> GeneratePebblesNearBushes(List<Vector2> bushes, int count)
    {
        var list = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            Vector2 bush = bushes[rng.Next(bushes.Count)];

            // Offset pequeño (−50 a 50)
            float offsetX = rng.Next(-5, 5);
            float offsetY = rng.Next(-5, 5);

            float x = Math.Clamp(bush.X + offsetX, MinX, MaxX);
            float y = Math.Clamp(bush.Y + offsetY, MinY, MaxY);

            list.Add(new Vector2(x, y));
        }

        return list;
    }

    // public static List<Vector2> GenerateEnemySpawns()
    public static void GenerateEnemySpawns()
    {
        var list = new List<Vector2>();
        Vector3 playerPos3 = GameManager.GetPlayer().GetPosition();
        Vector2 playerPos = new Vector2(playerPos3.X, playerPos3.Z);

        for (int i = 0; i < TotalSpawns; i++)
        {
            // Elegimos un ángulo al azar para el spawn
            float angle = (float)(rng.NextDouble() * Math.PI * 2f);

            // Radio alrededor de 300 (entre 250 y 350)
            float radius = 100 + (float)rng.NextDouble() * 50f;

            // Convertir el ángulo a desplazamiento en X/Y
            float offsetX = (float)Math.Cos(angle) * radius;
            float offsetY = (float)Math.Sin(angle) * radius;

            // Posición final del spawn
            float x = playerPos.X + offsetX;
            float y = playerPos.Y + offsetY;

            // Limitar al área del mapa
            x = Math.Clamp(x, MinX, MaxX);
            y = Math.Clamp(y, MinY, MaxY);

            list.Add(new Vector2(x, y));
        }
        SpawnPosition = list;

        // return list;
    }

    public static Vector2 RandomEnemyPosition()
    {
        var position = rng.Next(0, SpawnPosition.Count);
        return SpawnPosition[position];
    }
}
