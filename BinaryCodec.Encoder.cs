using System.Text;

namespace BeeQ.Codec.BinaryCodec;

internal class TElement(byte code, object number, byte[] numberBytes, object? extradata, byte[]? extradataBytes)
{
    public byte Code { get; set; } = code;
    public object Number { get; set; } = number;
    public byte[] NumberBytes { get; set; } = numberBytes;
    public object? ExtraData { get; set; } = extradata;
    public byte[]? ExtraDataBytes { get; set; } = extradataBytes;
}

/// <summary>
/// Defines the public encoder API used to build binary codec payloads.
/// Implementations allow adding single values, ranges and building the final byte array.
/// </summary>
public interface ICodecEncoder
{
    /// <summary>
    /// Adds a single byte value to the encoder.
    /// </summary>
    /// <param name="num">The byte value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(byte num, string? data = null);

    /// <summary>
    /// Adds a single unsigned 16-bit value to the encoder.
    /// </summary>
    /// <param name="num">The unsigned 16-bit value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(ushort num, string? data = null);

    /// <summary>
    /// Adds a single signed 16-bit value to the encoder.
    /// </summary>
    /// <param name="num">The signed 16-bit value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(short num, string? data = null);

    /// <summary>
    /// Adds a single unsigned 32-bit value to the encoder.
    /// </summary>
    /// <param name="num">The unsigned 32-bit value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(uint num, string? data = null);

    /// <summary>
    /// Adds a single signed 32-bit value to the encoder.
    /// </summary>
    /// <param name="num">The signed 32-bit value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(int num, string? data = null);

    /// <summary>
    /// Adds a single unsigned 64-bit value to the encoder.
    /// </summary>
    /// <param name="num">The unsigned 64-bit value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(ulong num, string? data = null);

    /// <summary>
    /// Adds a single signed 64-bit value to the encoder.
    /// </summary>
    /// <param name="num">The signed 64-bit value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(long num, string? data = null);

    /// <summary>
    /// Adds a single single-precision floating point value to the encoder.
    /// </summary>
    /// <param name="num">The float value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(float num, string? data = null);

    /// <summary>
    /// Adds a single double-precision floating point value to the encoder.
    /// </summary>
    /// <param name="num">The double value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(double num, string? data = null);

    /// <summary>
    /// Adds a single decimal value to the encoder.
    /// </summary>
    /// <param name="num">The decimal value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(decimal num, string? data = null);

    /// <summary>
    /// Adds a single Guid value to the encoder.
    /// </summary>
    /// <param name="num">The Guid value to add.</param>
    /// <param name="data">Optional associated string data.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Add(Guid num, string? data = null);

    /// <summary>
    /// Adds multiple byte values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of byte values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<byte> values);

    /// <summary>
    /// Adds multiple unsigned 16-bit values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of unsigned 16-bit values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<ushort> values);

    /// <summary>
    /// Adds multiple signed 16-bit values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of signed 16-bit values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<short> values);

    /// <summary>
    /// Adds multiple unsigned 32-bit values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of unsigned 32-bit values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<uint> values);

    /// <summary>
    /// Adds multiple signed 32-bit values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of signed 32-bit values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<int> values);

    /// <summary>
    /// Adds multiple unsigned 64-bit values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of unsigned 64-bit values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<ulong> values);

    /// <summary>
    /// Adds multiple signed 64-bit values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of signed 64-bit values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<long> values);

    /// <summary>
    /// Adds multiple float values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of float values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<float> values);

    /// <summary>
    /// Adds multiple double values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of double values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<double> values);

    /// <summary>
    /// Adds multiple decimal values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of decimal values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<decimal> values);

    /// <summary>
    /// Adds multiple Guid values to the encoder.
    /// </summary>
    /// <param name="values">Sequence of Guid values to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<Guid> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a byte and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<byte, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is an unsigned 16-bit integer and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<ushort, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a signed 16-bit integer and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<short, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is an unsigned 32-bit integer and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<uint, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a signed 32-bit integer and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<int, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is an unsigned 64-bit integer and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<ulong, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a signed 64-bit integer and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<long, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a float and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<float, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a double and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<double, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a decimal and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<decimal, string?>> values);

    /// <summary>
    /// Adds multiple key/value pairs where the key is a Guid and the value is optional data.
    /// </summary>
    /// <param name="values">Sequence of key/value pairs to add.</param>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder AddRange(IEnumerable<KeyValuePair<Guid, string?>> values);

    /// <summary>
    /// Marks the encoder as an empty payload.
    /// </summary>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder SetEmpty();

    /// <summary>
    /// Marks the encoder as a full payload (no additional items expected).
    /// </summary>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder SetFull();

    /// <summary>
    /// Attempts to compress the encoded payload (implementation specific).
    /// </summary>
    /// <returns>The current <see cref="ICodecEncoder"/> instance for chaining.</returns>
    ICodecEncoder Compress();

    /// <summary>
    /// Builds and returns the final binary representation of the encoded payload.
    /// </summary>
    /// <returns>Byte array containing the encoded payload.</returns>
    byte[] Build();
}
internal class CodecEncoder : ICodecEncoder
{
    public enum CodecStatus
    {
        Empty,
        WithData,
        Full
    }

    protected CodecStatus Status = CodecStatus.Empty;
    private List<TElement> Elements { get; set; } = [];

    private static byte[]? GetExtraDataBytes(string? extra) => string.IsNullOrEmpty(extra) ? null : Encoding.UTF8.GetBytes(extra);

    public ICodecEncoder Add(byte num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_BYTE, InternalEncoder.TAG_NUMBERS_WHITDATA_BYTE, num, [num], data, GetExtraDataBytes(data));
    public ICodecEncoder Add(ushort num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_USHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_USHORT, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(short num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_SHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_SHORT, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(uint num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_UINT, InternalEncoder.TAG_NUMBERS_WHITDATA_UINT, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(int num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_INT, InternalEncoder.TAG_NUMBERS_WHITDATA_INT, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(ulong num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_ULONG, InternalEncoder.TAG_NUMBERS_WHITDATA_ULONG, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(long num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_LONG, InternalEncoder.TAG_NUMBERS_WHITDATA_LONG, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(float num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_FLOAT, InternalEncoder.TAG_NUMBERS_WHITDATA_FLOAT, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(double num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_DOUBLE, InternalEncoder.TAG_NUMBERS_WHITDATA_DOUBLE, num, BitConverter.GetBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(decimal num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_DECIMAL, InternalEncoder.TAG_NUMBERS_WHITDATA_DECIMAL, num, InternalEncoder.DecimalToBytes(num), data, GetExtraDataBytes(data));
    public ICodecEncoder Add(Guid num, string? data = null) => InternalAdd(InternalEncoder.TAG_NUMBERS_GUID, InternalEncoder.TAG_NUMBERS_WHITDATA_GUID, num, num.ToByteArray(), data, GetExtraDataBytes(data));

    public ICodecEncoder AddRange(IEnumerable<byte> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_BYTE, InternalEncoder.TAG_NUMBERS_WHITDATA_BYTE, values.Select(num => new TElement(0x00, num, [num], null, null)));
    public ICodecEncoder AddRange(IEnumerable<ushort> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_USHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_USHORT, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<short> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_SHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_SHORT, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<uint> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_UINT, InternalEncoder.TAG_NUMBERS_WHITDATA_UINT, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<int> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_INT, InternalEncoder.TAG_NUMBERS_WHITDATA_INT, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<ulong> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_ULONG, InternalEncoder.TAG_NUMBERS_WHITDATA_ULONG, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<long> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_LONG, InternalEncoder.TAG_NUMBERS_WHITDATA_LONG, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<float> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_FLOAT, InternalEncoder.TAG_NUMBERS_WHITDATA_FLOAT, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<double> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_DOUBLE, InternalEncoder.TAG_NUMBERS_WHITDATA_DOUBLE, values.Select(num => new TElement(0x00, num, BitConverter.GetBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<decimal> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_DECIMAL, InternalEncoder.TAG_NUMBERS_WHITDATA_DECIMAL, values.Select(num => new TElement(0x00, num, InternalEncoder.DecimalToBytes(num), null, null)));
    public ICodecEncoder AddRange(IEnumerable<Guid> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_GUID, InternalEncoder.TAG_NUMBERS_WHITDATA_GUID, values.Select(num => new TElement(0x00, num, num.ToByteArray(), null, null)));

    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<byte, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_BYTE, InternalEncoder.TAG_NUMBERS_WHITDATA_BYTE, values.Select(num => new TElement(0x00, num.Key, [num.Key], num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<ushort, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_USHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_USHORT, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<short, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_SHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_SHORT, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<uint, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_UINT, InternalEncoder.TAG_NUMBERS_WHITDATA_UINT, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<int, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_INT, InternalEncoder.TAG_NUMBERS_WHITDATA_INT, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<ulong, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_ULONG, InternalEncoder.TAG_NUMBERS_WHITDATA_ULONG, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<long, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_LONG, InternalEncoder.TAG_NUMBERS_WHITDATA_LONG, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<float, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_FLOAT, InternalEncoder.TAG_NUMBERS_WHITDATA_FLOAT, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<double, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_DOUBLE, InternalEncoder.TAG_NUMBERS_WHITDATA_DOUBLE, values.Select(num => new TElement(0x00, num.Key, BitConverter.GetBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<decimal, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_DECIMAL, InternalEncoder.TAG_NUMBERS_WHITDATA_DECIMAL, values.Select(num => new TElement(0x00, num.Key, InternalEncoder.DecimalToBytes(num.Key), num.Value, GetExtraDataBytes(num.Value))));
    public ICodecEncoder AddRange(IEnumerable<KeyValuePair<Guid, string?>> values) => InternalAddRange(InternalEncoder.TAG_NUMBERS_GUID, InternalEncoder.TAG_NUMBERS_WHITDATA_GUID, values.Select(num => new TElement(0x00, num.Key, num.Key.ToByteArray(), num.Value, GetExtraDataBytes(num.Value))));


    private CodecEncoder InternalAdd(byte code, byte codeWithData, object num, byte[] numBytes, string? extra = null, byte[]? extraBytes = null)
    {
        Status = CodecStatus.WithData;
        var withData = !string.IsNullOrEmpty(extra);
        Elements.Add(new TElement(withData ? codeWithData : code, num, numBytes, withData ? extra : null, withData ? extraBytes : null));
        return this;
    }

    private CodecEncoder InternalAddRange(byte code, byte codeWithData, IEnumerable<TElement> values)
    {
        Status = CodecStatus.WithData;
        foreach (var value in values)
        {
            var withData = value.ExtraData != null;
            if (!withData && value.ExtraData is string str)
                withData = !string.IsNullOrWhiteSpace(str);

            Elements.Add(new TElement(withData ? codeWithData : code, value.Number, value.NumberBytes, withData ? value.ExtraData : null, withData ? value.ExtraDataBytes : null));
        }
        return this;
    }

    public ICodecEncoder SetEmpty()
    {
        Status = CodecStatus.Empty;
        Elements.Clear();
        return this;
    }

    public ICodecEncoder SetFull()
    {
        Status = CodecStatus.Full;
        Elements.Clear();
        return this;
    }

    public ICodecEncoder Compress()
    {
        static bool HasTag(byte code, byte tag1, byte tag2) => (code == tag1 || code == tag2);
        static void Change<T>(ref TElement ele, byte newCode, T newNumber, Func<T, byte[]> newNumberBytes)
        {
            ele.Number = newNumber!;
            ele.NumberBytes = newNumberBytes(newNumber);
            ele.Code = newCode;
        }

        for (var i = 0; i < Elements.Count; i++)
        {
            var ele = Elements[i];
            // numeros con signo
            if (HasTag(ele.Code, InternalEncoder.TAG_NUMBERS_LONG, InternalEncoder.TAG_NUMBERS_WHITDATA_LONG) && ((long)ele.Number <= int.MaxValue))
                Change<int>(ref ele, InternalEncoder.TAG_NUMBERS_INT, Convert.ToInt32(ele.Number), n => BitConverter.GetBytes(n));
            if (HasTag(ele.Code, InternalEncoder.TAG_NUMBERS_INT, InternalEncoder.TAG_NUMBERS_WHITDATA_INT) && ((int)ele.Number <= short.MaxValue))
                Change<short>(ref ele, InternalEncoder.TAG_NUMBERS_SHORT, Convert.ToInt16(ele.Number), n => BitConverter.GetBytes(n));
            if (HasTag(ele.Code, InternalEncoder.TAG_NUMBERS_SHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_SHORT) && ((short)ele.Number <= byte.MaxValue && (short)ele.Number >= 0))
                Change<byte>(ref ele, InternalEncoder.TAG_NUMBERS_BYTE, Convert.ToByte(ele.Number), n => [n]);

            // numeros sin signo
            if (HasTag(ele.Code, InternalEncoder.TAG_NUMBERS_ULONG, InternalEncoder.TAG_NUMBERS_WHITDATA_ULONG) && ((ulong)ele.Number <= int.MaxValue))
                Change<uint>(ref ele, InternalEncoder.TAG_NUMBERS_UINT, Convert.ToUInt32(ele.Number), n => BitConverter.GetBytes(n));
            if (HasTag(ele.Code, InternalEncoder.TAG_NUMBERS_UINT, InternalEncoder.TAG_NUMBERS_WHITDATA_UINT) && ((uint)ele.Number <= short.MaxValue))
                Change<ushort>(ref ele, InternalEncoder.TAG_NUMBERS_USHORT, Convert.ToUInt16(ele.Number), n => BitConverter.GetBytes(n));
            if (HasTag(ele.Code, InternalEncoder.TAG_NUMBERS_USHORT, InternalEncoder.TAG_NUMBERS_WHITDATA_USHORT) && ((ushort)ele.Number <= byte.MaxValue))
                Change<byte>(ref ele, InternalEncoder.TAG_NUMBERS_BYTE, Convert.ToByte(ele.Number), n => [n]);
        }
        return this;
    }

    public byte[] Build()
    {
        if (Status == CodecStatus.Empty) return [];
        if (Status == CodecStatus.Full) return [InternalEncoder.TAG_FULL];

        return [
            InternalEncoder.TAG_WITHDATA,
            .. Elements
                .Select(p => p.Code)
                .Distinct()
                .SelectMany(code => code < InternalEncoder.TAG_NUMBERS_WHITDATA_PREFIX ? BuildSingle(code) : BuildWithData(code))
        ];
    }

    private IEnumerable<byte> BuildSingle(byte code)
    {
        var values = Elements.Where(p => p.Code == code).SelectMany(p => p.NumberBytes).ToArray();
        var cant = (ushort)Elements.Count(p => p.Code == code);
        return [code, .. BitConverter.GetBytes(cant), .. values];
    }

    private IEnumerable<byte> BuildWithData(byte code)
    {
        return Elements
            .Where(p => p.Code == code)
            .SelectMany(p => (byte[])[code, .. p.NumberBytes, .. BitConverter.GetBytes((ushort)(p.ExtraDataBytes?.Length ?? 0)), .. (p.ExtraDataBytes ?? [])]);
    }

}