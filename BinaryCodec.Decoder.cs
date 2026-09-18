namespace BeeQ.Codec.BinaryCodec;

/// <summary>
/// Defines the decoder API for parsing binary codec payloads produced by <see cref="ICodecEncoder"/>.
/// </summary>
public interface ICodecDecoder
{
    /// <summary>
    /// Returns true when the decoded payload represents a full indicator.
    /// </summary>
    /// <returns><see langword="true"/> if payload is full; otherwise <see langword="false"/>.</returns>
    bool IsFull();

    /// <summary>
    /// Returns true when the decoded payload is empty.
    /// </summary>
    /// <returns><see langword="true"/> if payload is empty; otherwise <see langword="false"/>.</returns>
    bool IsEmpty();

    /// <summary>
    /// Returns true when the decoded payload contains associated data values.
    /// </summary>
    /// <returns><see langword="true"/> if payload has data; otherwise <see langword="false"/>.</returns>
    bool HasData();

    /// <summary>
    /// Returns all decoded items as a sequence of <see cref="CodecItem"/>.
    /// </summary>
    /// <returns>Sequence of decoded <see cref="CodecItem"/> instances.</returns>
    IEnumerable<CodecItem> Decode();

    /// <summary>
    /// Returns decoded items filtered by the requested item subtype.
    /// </summary>
    /// <typeparam name="T">The codec item subtype to filter.</typeparam>
    /// <returns>Sequence of items of type <typeparamref name="T"/>.</returns>
    IEnumerable<T> OfType<T>() where T : CodecItem;

    /// <summary>
    /// Returns all decoded items converted to <see cref="CodecItemLong"/> when possible.
    /// </summary>
    /// <returns>Sequence of <see cref="CodecItemLong"/> instances.</returns>
    IEnumerable<CodecItemLong> AsLong();

    /// <summary>
    /// Returns all decoded items converted to <see cref="CodecItemUlong"/> when possible.
    /// </summary>
    /// <returns>Sequence of <see cref="CodecItemUlong"/> instances.</returns>
    IEnumerable<CodecItemUlong> AsULong();

    /// <summary>
    /// Returns all decoded items converted to <see cref="CodecItemInt"/> when possible.
    /// </summary>
    /// <returns>Sequence of <see cref="CodecItemInt"/> instances.</returns>
    IEnumerable<CodecItemInt> AsInt();

    /// <summary>
    /// Returns all decoded items converted to <see cref="CodecItemUint"/> when possible.
    /// </summary>
    /// <returns>Sequence of <see cref="CodecItemUint"/> instances.</returns>
    IEnumerable<CodecItemUint> AsUInt();

    /// <summary>
    /// Returns all decoded items converted to <see cref="CodecItemShort"/> when possible.
    /// </summary>
    /// <returns>Sequence of <see cref="CodecItemShort"/> instances.</returns>
    IEnumerable<CodecItemShort> AsShort();

    /// <summary>
    /// Returns all decoded items converted to <see cref="CodecItemUshort"/> when possible.
    /// </summary>
    /// <returns>Sequence of <see cref="CodecItemUshort"/> instances.</returns>
    IEnumerable<CodecItemUshort> AsUShort();
}

internal class CodecDecoder : ICodecDecoder
{
    private byte[] Content { get; set; }
    private List<CodecItem> Elements { get; set; } = [];

    public CodecDecoder(byte[] content)
    {
        this.Content = content;
        Process(content);
    }

    private void Process(byte[] bytes)
    {
        this.Elements = InternalEncoder.Decode(bytes);
    }

    public bool IsEmpty() => Content.Length == 0;
    public bool IsFull() => Content.Length != 0 && Content[0] == InternalEncoder.TAG_FULL;
    public bool HasData() => Content.Length != 0 && Content[0] == InternalEncoder.TAG_WITHDATA;
    public IEnumerable<T> OfType<T>() where T : CodecItem => this.Elements.OfType<T>();

    public IEnumerable<CodecItem> Decode()
    {
        return this.Elements;
    }

    public IEnumerable<CodecItemLong> AsLong()
    {
        return [
            .. Elements.OfType<CodecItemLong>(),
            .. Elements.OfType<CodecItemInt>().Select(p => new CodecItemLong(){ Number = Convert.ToInt64(p.Number), Data = p.Data }).ToArray(),
            .. Elements.OfType<CodecItemShort>().Select(p => new CodecItemLong(){ Number = Convert.ToInt64(p.Number), Data = p.Data }).ToArray(),
            .. Elements.OfType<CodecItemByte>().Select(p => new CodecItemLong(){ Number = Convert.ToInt64(p.Number), Data = p.Data }).ToArray(),
        ];
    }
    public IEnumerable<CodecItemUlong> AsULong()
    {
        return [
            .. Elements.OfType<CodecItemUlong>(),
            .. Elements.OfType<CodecItemUint>().Select(p => new CodecItemUlong(){ Number = Convert.ToUInt64(p.Number), Data = p.Data }).ToArray(),
            .. Elements.OfType<CodecItemUshort>().Select(p => new CodecItemUlong(){ Number = Convert.ToUInt64(p.Number), Data = p.Data }).ToArray(),
            .. Elements.OfType<CodecItemByte>().Select(p => new CodecItemUlong(){ Number = Convert.ToUInt64(p.Number), Data = p.Data }).ToArray(),
        ];
    }
    public IEnumerable<CodecItemInt> AsInt()
    {
        return [
            .. Elements.OfType<CodecItemInt>(),
            .. Elements.OfType<CodecItemShort>().Select(p => new CodecItemInt(){ Number = Convert.ToInt32(p.Number), Data = p.Data }).ToArray(),
            .. Elements.OfType<CodecItemByte>().Select(p => new CodecItemInt(){ Number = Convert.ToInt32(p.Number), Data = p.Data }).ToArray(),
        ];
    }
    public IEnumerable<CodecItemUint> AsUInt()
    {
        return [
            .. Elements.OfType<CodecItemUint>(),
            .. Elements.OfType<CodecItemUshort>().Select(p => new CodecItemUint(){ Number = Convert.ToUInt32(p.Number), Data = p.Data }).ToArray(),
            .. Elements.OfType<CodecItemByte>().Select(p => new CodecItemUint(){ Number = Convert.ToUInt32(p.Number), Data = p.Data }).ToArray(),
        ];
    }
    public IEnumerable<CodecItemShort> AsShort()
    {
        return [
            .. Elements.OfType<CodecItemShort>(),
            .. Elements.OfType<CodecItemByte>().Select(p => new CodecItemShort(){ Number = Convert.ToInt16(p.Number), Data = p.Data }).ToArray(),
        ];
    }
    public IEnumerable<CodecItemUshort> AsUShort()
    {
        return [
            .. Elements.OfType<CodecItemUshort>(),
            .. Elements.OfType<CodecItemByte>().Select(p => new CodecItemUshort(){ Number = Convert.ToUInt16(p.Number), Data = p.Data }).ToArray(),
        ];
    }
}
