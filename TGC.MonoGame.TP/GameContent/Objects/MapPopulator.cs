using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public static class MapPopulator
{
    private static readonly int MinX = -1612;
    private static readonly int MaxX = 1587;
    private static readonly int MinY = -1612;
    private static readonly int MaxY = 1587;

    private static readonly Random rng = new Random(12345); // Seed fijo para reproducibilidad

    public static List<Vector2> Houses { get; private set; }
    public static List<Vector2> BigStones { get; private set; }
    public static List<Vector2> Bushes { get; private set; }
    public static List<Vector2> LittleStones { get; private set; }

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
}
