using System.Buffers.Binary;
using System.Text;

namespace BeeQ.Codec.BinaryCodec;

internal static class InternalEncoder
{
    internal const byte TAG_FULL = 0xff;
    internal const byte TAG_WITHDATA = 0x00;

    internal const byte TAG_NUMBERS_BYTE = 0x01;
    internal const byte TAG_NUMBERS_USHORT = 0x02;
    internal const byte TAG_NUMBERS_SHORT = 0x03;
    internal const byte TAG_NUMBERS_UINT = 0x04;
    internal const byte TAG_NUMBERS_INT = 0x05;
    internal const byte TAG_NUMBERS_ULONG = 0x06;
    internal const byte TAG_NUMBERS_LONG = 0x07;
    internal const byte TAG_NUMBERS_FLOAT = 0x08;
    internal const byte TAG_NUMBERS_DOUBLE = 0x09;
    internal const byte TAG_NUMBERS_DECIMAL = 0x0A;
    internal const byte TAG_NUMBERS_GUID = 0x0B;

    internal const byte TAG_NUMBERS_WHITDATA_PREFIX = 0x10; // asegurarse que sea mayor que el máximo de los valores x tipo de dato
    internal const byte TAG_NUMBERS_WHITDATA_BYTE = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_BYTE;
    internal const byte TAG_NUMBERS_WHITDATA_USHORT = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_USHORT;
    internal const byte TAG_NUMBERS_WHITDATA_SHORT = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_SHORT;
    internal const byte TAG_NUMBERS_WHITDATA_UINT = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_UINT;
    internal const byte TAG_NUMBERS_WHITDATA_INT = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_INT;
    internal const byte TAG_NUMBERS_WHITDATA_ULONG = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_ULONG;
    internal const byte TAG_NUMBERS_WHITDATA_LONG = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_LONG;
    internal const byte TAG_NUMBERS_WHITDATA_FLOAT = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_FLOAT;
    internal const byte TAG_NUMBERS_WHITDATA_DOUBLE = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_DOUBLE;
    internal const byte TAG_NUMBERS_WHITDATA_DECIMAL = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_DECIMAL;
    internal const byte TAG_NUMBERS_WHITDATA_GUID = TAG_NUMBERS_WHITDATA_PREFIX + TAG_NUMBERS_GUID;

    #region Encode Simple

    public static byte[] Encode(params byte[] nums) => InternalEncode([.. nums.Select(p => new byte[] { p })], TAG_NUMBERS_BYTE);
    public static byte[] Encode(params ushort[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_USHORT);
    public static byte[] Encode(params short[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_SHORT);
    public static byte[] Encode(params uint[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_UINT);
    public static byte[] Encode(params int[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_INT);
    public static byte[] Encode(params ulong[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_ULONG);
    public static byte[] Encode(params long[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_LONG);
    public static byte[] Encode(params float[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_FLOAT);
    public static byte[] Encode(params double[] nums) => InternalEncode([.. nums.Select(p => BitConverter.GetBytes(p))], TAG_NUMBERS_DOUBLE);
    public static byte[] Encode(params decimal[] nums) => InternalEncode([.. nums.Select(p => DecimalToBytes(p))], TAG_NUMBERS_DECIMAL);
    public static byte[] Encode(params Guid[] nums) => InternalEncode([.. nums.Select(p => p.ToByteArray())], TAG_NUMBERS_GUID);

    private static byte[] InternalEncode(List<byte[]> list, byte code)
    {
        if (list.Count > ushort.MaxValue)
            throw new Exception($"Máximo {ushort.MaxValue} números por bloque simple");
        var count = BitConverter.GetBytes((ushort)list.Count);
        var data = list.SelectMany(p => p);

        return [code, ..count, ..data];
    }

    #endregion

    #region Encode With Data

    public static byte[] EncodeWithData(byte num, string data) => InternalEncodeWithData([num], data, TAG_NUMBERS_WHITDATA_BYTE);
    public static byte[] EncodeWithData(ushort num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_USHORT);
    public static byte[] EncodeWithData(short num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_SHORT);
    public static byte[] EncodeWithData(uint num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_UINT);
    public static byte[] EncodeWithData(int num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_INT);
    public static byte[] EncodeWithData(ulong num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_ULONG);
    public static byte[] EncodeWithData(long num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_LONG);
    public static byte[] EncodeWithData(float num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_FLOAT);
    public static byte[] EncodeWithData(double num, string data) => InternalEncodeWithData(BitConverter.GetBytes(num), data, TAG_NUMBERS_WHITDATA_DOUBLE);
    public static byte[] EncodeWithData(decimal num, string data) => InternalEncodeWithData(DecimalToBytes(num), data, TAG_NUMBERS_WHITDATA_DECIMAL);
    public static byte[] EncodeWithData(Guid num, string data) => InternalEncodeWithData(num.ToByteArray(), data, TAG_NUMBERS_WHITDATA_GUID);

    public static byte[] InternalEncodeWithData(byte[] num, string data, byte code)
    {
        var _data = Encoding.UTF8.GetBytes(data);
        if (_data.Length > ushort.MaxValue)
            throw new Exception($"Máximo {ushort.MaxValue} caracteres por bloque");
        var dataSize = (ushort)_data.Length;
        var _size = BitConverter.GetBytes(dataSize);

        return [code, .. num, .. _size, .. _data];
    }

    #endregion

    #region Decode

    public static List<CodecItem> Decode(byte[] bytes) 
    {
        var totalCount = bytes.Length;
        if (totalCount == 0 || bytes[0] == 0xff) return [];
        if (totalCount < 2)
            throw new Exception("Formato de codigo con valores inválido");

        var result = new List<CodecItem>();
        var index = 1;
        do
        {
            var value = (IEnumerable<CodecItem>)(bytes[index] switch
            {
                // numeros simples 
                TAG_NUMBERS_BYTE => InternalDecodeByte(bytes, ref index),
                TAG_NUMBERS_USHORT => InternalDecodeUshort(bytes, ref index),
                TAG_NUMBERS_SHORT => InternalDecodeShort(bytes, ref index),
                TAG_NUMBERS_UINT => InternalDecodeUint(bytes, ref index),
                TAG_NUMBERS_INT => InternalDecodeInt(bytes, ref index),
                TAG_NUMBERS_ULONG => InternalDecodeUlong(bytes, ref index),
                TAG_NUMBERS_LONG => InternalDecodeLong(bytes, ref index),
                TAG_NUMBERS_FLOAT => InternalDecodeFloat(bytes, ref index),
                TAG_NUMBERS_DOUBLE => InternalDecodeDouble(bytes, ref index),
                TAG_NUMBERS_DECIMAL => InternalDecodeDecimal(bytes, ref index),
                TAG_NUMBERS_GUID => InternalDecodeGuid(bytes, ref index),

                // numeros con valores
                TAG_NUMBERS_WHITDATA_BYTE => [InternalDecodeByteWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_USHORT => [InternalDecodeUshortWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_SHORT => [InternalDecodeShortWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_UINT => [InternalDecodeUintWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_INT => [InternalDecodeIntWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_ULONG => [InternalDecodeUlongWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_LONG => [InternalDecodeLongWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_FLOAT => [InternalDecodeFloatWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_DOUBLE => [InternalDecodeDoubleWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_DECIMAL => [InternalDecodeDecimalWithData(bytes, ref index)],
                TAG_NUMBERS_WHITDATA_GUID => [InternalDecodeGuidWithData(bytes, ref index)],

                _ => throw new Exception("Formato de sección inválido")
            });
            result.AddRange(value);
        } while (index < totalCount);
        return result;
    }

    private static IEnumerable<CodecItemByte> InternalDecodeByte(byte[] bytes, ref int index)
    {
        var typesize = 1;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));
        var pointer = index + 1 + 2;
        var results = bytes[pointer..(pointer + (cant * typesize))];

        index = pointer + (cant * typesize); // tag + cant + content
        return results.Select(num => new CodecItemByte() { Number = num });
    }
    private static IEnumerable<CodecItemUshort> InternalDecodeUshort(byte[] bytes, ref int index)
    {
        var typesize = 2;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<ushort>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToUInt16(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemUshort() { Number = num });
    }
    private static IEnumerable<CodecItemShort> InternalDecodeShort(byte[] bytes, ref int index)
    {
        var typesize = 2;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<short>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToInt16(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemShort() { Number = num });
    }
    private static IEnumerable<CodecItemUint> InternalDecodeUint(byte[] bytes, ref int index)
    {
        var typesize = 4;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<uint>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToUInt32(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemUint() { Number = num });
    }
    private static IEnumerable<CodecItemInt> InternalDecodeInt(byte[] bytes, ref int index)
    {
        var typesize = 4;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<int>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToInt32(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemInt() { Number = num });
    }
    private static IEnumerable<CodecItemUlong> InternalDecodeUlong(byte[] bytes, ref int index)
    {
        var typesize = 8;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<ulong>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToUInt64(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemUlong() { Number = num });
    }
    private static IEnumerable<CodecItemLong> InternalDecodeLong(byte[] bytes, ref int index)
    {
        var typesize = 8;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<long>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToInt64(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemLong() { Number = num });
    }
    private static IEnumerable<CodecItemFloat> InternalDecodeFloat(byte[] bytes, ref int index)
    {
        var typesize = 4;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<float>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToSingle(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemFloat() { Number = num });
    }
    private static IEnumerable<CodecItemDouble> InternalDecodeDouble(byte[] bytes, ref int index)
    {
        var typesize = 8;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<double>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BitConverter.ToDouble(bytes.AsSpan(pointer, typesize)));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemDouble() { Number = num });
    }
    private static IEnumerable<CodecItemDecimal> InternalDecodeDecimal(byte[] bytes, ref int index)
    {
        var typesize = 16;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<decimal>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            results.Add(BytesToDecimal(bytes.AsSpan(pointer, typesize).ToArray()));
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemDecimal() { Number = num });
    }
    private static IEnumerable<CodecItemGuid> InternalDecodeGuid(byte[] bytes, ref int index)
    {
        var typesize = 16;
        var cant = BitConverter.ToUInt16(bytes.AsSpan(index + 1, 2));

        var results = new List<Guid>();
        var pointer = index + 1 + 2;
        for (ushort i = 0; i < cant; i++)
        {
            var guid = new Guid(bytes.AsSpan(pointer, typesize));
            results.Add(guid);
            pointer += typesize;
        }

        index = pointer; // tag + cant + content
        return results.Select(num => new CodecItemGuid() { Number = num });
    }

    private static CodecItemByte InternalDecodeByteWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemByte();
        var typesize = 1;
        int pointer = index + 1; // numero
        result.Number = bytes[pointer];
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemUshort InternalDecodeUshortWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemUshort();
        var typesize = 2;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToUInt16(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemShort InternalDecodeShortWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemShort();
        var typesize = 2;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToInt16(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemUint InternalDecodeUintWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemUint();
        var typesize = 4;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToUInt32(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemInt InternalDecodeIntWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemInt();
        var typesize = 4;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToInt32(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemUlong InternalDecodeUlongWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemUlong();
        var typesize = 8;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToUInt64(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemLong InternalDecodeLongWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemLong();
        var typesize = 8;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToInt64(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemFloat InternalDecodeFloatWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemFloat();
        var typesize = 4;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToSingle(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemDouble InternalDecodeDoubleWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemDouble();
        var typesize = 8;
        int pointer = index + 1; // numero
        result.Number = BitConverter.ToDouble(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemDecimal InternalDecodeDecimalWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemDecimal();
        var typesize = 16;
        int pointer = index + 1; // numero
        result.Number = BytesToDecimal(bytes.AsSpan(pointer, typesize).ToArray());
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }
    private static CodecItemGuid InternalDecodeGuidWithData(byte[] bytes, ref int index)
    {
        var result = new CodecItemGuid();
        var typesize = 16;
        int pointer = index + 1; // numero
        result.Number = new Guid(bytes.AsSpan(pointer, typesize));
        pointer += typesize; // data size
        var dataSize = BitConverter.ToUInt16(bytes.AsSpan(pointer, 2));
        pointer += 2; // data
        result.Data = Encoding.UTF8.GetString(bytes.AsSpan(pointer, dataSize));
        index = pointer + dataSize;
        return result;
    }

    #endregion

    #region Helpers

    internal static byte[] DecimalToBytes(decimal value)
    {
        var bits = decimal.GetBits(value); // Devuelve int[4] siempre
        var bytes = new byte[16];

        for (int i = 0; i < 4; i++)
            BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(i * 4), bits[i]);

        return bytes;
    }

    internal static decimal BytesToDecimal(byte[] bytes)
    {
        if (bytes == null || bytes.Length != 16)
            throw new ArgumentException("El array debe contener exactamente 16 bytes.");

        var bits = new int[4];
        for (int i = 0; i < 4; i++)
            bits[i] = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(i * 4));

        return new decimal(bits); // Constructor oficial para serialización
    }

    #endregion
}
