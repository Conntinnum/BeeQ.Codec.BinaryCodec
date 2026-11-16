namespace BeeQ.Codec.BinaryCodec;

public class InvalidTagBinaryCodecException : Exception
{
    public InvalidTagBinaryCodecException() : base("Binary Tag invalid.") { }
}
public class InvalidBinaryCodecException : Exception
{
    public InvalidBinaryCodecException() : base("Binary invalid.") { }
}
public class MaxLengthExceededBinaryCodecException : Exception
{
    public MaxLengthExceededBinaryCodecException() : base("Maximum length of elements by block exceeded.") { }
}