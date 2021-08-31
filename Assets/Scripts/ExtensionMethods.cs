using UnityEngine;

public static class ExtensionMethods {
 
    /// <summary>
    /// Remap a value from one range onto another.
    /// </summary>
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
    public enum CardinalDirection
    {
        North,
        East,
        South,
        West
    }

    /// <summary>
    /// Snap a 2D vector to the nearest (normalized) cardinal direction.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static CardinalDirection SnapToCardinalDirection(Vector2 v)
    {
        var normalizedV = v.normalized;
        var isXGreatest = Mathf.Abs(normalizedV.x) > Mathf.Abs(normalizedV.y);
        
        if (isXGreatest)
            return (v.x >= 0) ? CardinalDirection.East : CardinalDirection.West;
        return (v.y >= 0) ? CardinalDirection.North : CardinalDirection.South;
    }

}