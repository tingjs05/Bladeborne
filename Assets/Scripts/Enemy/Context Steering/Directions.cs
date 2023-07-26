using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions
{
    // directions arrays to be accessed by other scripts
    public static Vector2[] directions {get; private set;} = new[] {
        new Vector2(0, 1).normalized,
        new Vector2(1, 1).normalized,
        new Vector2(1, 0).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(0, -1).normalized,
        new Vector2(-1, -1).normalized,
        new Vector2(-1, 0).normalized,
        new Vector2(-1, 1).normalized
    };
}
