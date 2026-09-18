namespace BeeQ.Codec.BinaryCodec;

/// <summary>
/// Entry point for the BinaryCodec library. Provides helpers to create encoder and decoder instances.
/// </summary>
public static class BinaryCodec
{
    /// <summary>
    /// Creates a new encoder instance to build binary codec payloads.
    /// </summary>
    /// <returns>A new <see cref="ICodecEncoder"/> instance.</returns>
    public static ICodecEncoder Encoder() => new CodecEncoder();

    /// <summary>
    /// Creates a new decoder instance for the provided binary content.
    /// </summary>
    /// <param name="content">Binary content previously produced by an encoder.</param>
    /// <returns>A new <see cref="ICodecDecoder"/> instance that can decode the provided content.</returns>
    public static ICodecDecoder Decoder(byte[] content) => new CodecDecoder(content);
}
