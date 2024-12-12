using System;
public class CardEnums
{
    public enum Rarities
    {
        SSS, S, A, B, C, D
    }
    [Flags] public enum UniqueTypes : uint
    {
        Shiny = 0x1,
        Holo = 0x2
    }
}
