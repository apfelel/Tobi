using System;
using UnityEngine;

public class CardEnums
{
    public enum Rarities
    {
        SSS, S, A, B, C, D
    }
    [Flags]
    public enum UniqueTypes : byte
    {
        Shiny, Holo        
    }
}
