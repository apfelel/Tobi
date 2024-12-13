using System;
public class CardEnums
{
    public enum Rarities
    {
        SSS, L, S, A, B, C, D
    }
    [Flags] public enum UniqueTypes : uint
    {
        Normal = 1,
        Shiny = 2,
        Holo = 4
    }
}
