namespace BeeQ.Codec.BinaryCodec;

/// <summary>
/// Represents a decoded codec item containing a numeric value and optional associated data.
/// This is the base type for specific typed items (byte, int, long, etc.).
/// </summary>
public class CodecItem
{
    /// <summary>
    /// The numeric value stored in this item as an unsigned 64-bit integer.
    /// For typed subclasses this property may be shadowed with a more specific numeric type.
    /// </summary>
    public ulong Number { get; set; }

    /// <summary>
    /// Optional textual data associated with the numeric value.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Returns a string representation of the item value and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the codec item.</returns>
    public override string ToString()
    {
        return $"{Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Marker interface indicating that a codec item supports compression in the encoder implementation.
/// </summary>
public interface ICodecItemCompressable { }

/// <summary>
/// Represents a codec item whose numeric value is a single byte.
/// </summary>
public class CodecItemByte : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as a byte.
    /// </summary>
    public new byte Number { get; set; }

    /// <summary>
    /// Returns a string representation including the byte prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the byte item.</returns>
    public override string ToString()
    {
        return $"(Byte) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is an unsigned 16-bit integer.
/// </summary>
public class CodecItemUshort : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as an unsigned 16-bit integer.
    /// </summary>
    public new ushort Number { get; set; }

    /// <summary>
    /// Returns a string representation including the UInt16 prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(UInt16) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a signed 16-bit integer.
/// </summary>
public class CodecItemShort : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as a signed 16-bit integer.
    /// </summary>
    public new short Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Int16 prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Int16) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is an unsigned 32-bit integer.
/// </summary>
public class CodecItemUint : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as an unsigned 32-bit integer.
    /// </summary>
    public new uint Number { get; set; }

    /// <summary>
    /// Returns a string representation including the UInt32 prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(UInt32) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a signed 32-bit integer.
/// </summary>
public class CodecItemInt : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as a signed 32-bit integer.
    /// </summary>
    public new int Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Int32 prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Int32) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is an unsigned 64-bit integer.
/// </summary>
public class CodecItemUlong : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as an unsigned 64-bit integer.
    /// </summary>
    public new ulong Number { get; set; }

    /// <summary>
    /// Returns a string representation including the UInt64 prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(UInt64) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a signed 64-bit integer.
/// </summary>
public class CodecItemLong : CodecItem, ICodecItemCompressable
{
    /// <summary>
    /// The numeric value as a signed 64-bit integer.
    /// </summary>
    public new long Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Int64 prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Int64) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a single-precision floating point value.
/// </summary>
public class CodecItemFloat : CodecItem
{
    /// <summary>
    /// The numeric value as a single-precision float.
    /// </summary>
    public new float Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Single prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Single) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a double-precision floating point value.
/// </summary>
public class CodecItemDouble : CodecItem
{
    /// <summary>
    /// The numeric value as a double-precision float.
    /// </summary>
    public new double Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Double prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Double) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a decimal value.
/// </summary>
public class CodecItemDecimal : CodecItem
{
    /// <summary>
    /// The numeric value as a decimal.
    /// </summary>
    public new decimal Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Decimal prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Decimal) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}

/// <summary>
/// Represents a codec item whose numeric value is a Guid.
/// </summary>
public class CodecItemGuid : CodecItem
{
    /// <summary>
    /// The numeric value as a Guid.
    /// </summary>
    public new Guid Number { get; set; }

    /// <summary>
    /// Returns a string representation including the Guid prefix and optional data.
    /// </summary>
    /// <returns>A human-readable representation of the item.</returns>
    public override string ToString()
    {
        return $"(Guid) {Number}{(Data != null ? $" : {Data}" : "")}";
    }
}
