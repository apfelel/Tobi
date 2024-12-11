using System;
public class CardEnums
{
    public enum Rarities
    {
        SSS, S, A, B, C, D
    }
    [Flags] public enum UniqueTypes : uint
    {
        Shiny = 0,
        Holo = 1
    }
}
