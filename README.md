# BeeQ.Codec.BinaryCodec

BeeQ.Codec.BinaryCodec is a small, dependency-free .NET library (distributed as a NuGet package) for compactly encoding and decoding numeric values with optional associated text data into a compact binary format.

The library exposes a simple encoder/decoder API that is ideal for scenarios where you need to serialize many numeric values (of different primitive types) into a compact byte array and later parse them back with minimal allocations.

Features
- Encode single values or ranges of values (byte, short, int, long, float, double, decimal, Guid, and their unsigned variants).
- Add optional UTF-8 string data associated with each numeric value.
- Build a compact byte[] payload suitable for storage or transport.
- Decode a payload into typed items and convert between compatible numeric types (for convenience).
- Minimal API surface and no external dependencies.

Installation

Install the NuGet package from nuget.org:

```bash
dotnet add package BeeQ.Codec.BinaryCodec
```

Or add the PackageReference to your project file:

```xml
<PackageReference Include="BeeQ.Codec.BinaryCodec" Version="*" />
```

Basic usage

Using the static entry point makes the API straightforward to use:

```csharp
using BeeQ.Codec.BinaryCodec;

// Create an encoder
var encoder = BinaryCodec.Encoder()
	.Add(42)                    // add int
	.Add(255, "flag")         // add int with associated string data
	.Add((ulong)123456789012)   // add ulong
	.Add(Guid.NewGuid(), "id")
	.Compress();                 // compress the content

// Build the final byte array
byte[] payload = encoder.Build();

// The payload can be stored, transmitted, or persisted

// Create a decoder for the same payload
var decoder = BinaryCodec.Decoder(payload);

// Check payload state
if (decoder.IsFull())
{
	Console.WriteLine("Payload is marked full");
}

// Enumerate decoded items
foreach (var item in decoder.Decode())
{
	Console.WriteLine(item.ToString());
}

// Convenience conversions
var longs = decoder.AsLong();     // get all numbers as long where possible
var ulongs = decoder.AsULong();   // get all numbers as ulong where possible
```

Encoding ranges and keyed pairs

```csharp
// Add a range of values
var bytes = new byte[] { 1, 2, 3, 4 };
var encoder2 = BinaryCodec.Encoder()
	.AddRange(bytes)
	.AddRange(new[] { 10, 20, 30 }); // adds ints

// Add key/value pairs (numeric + optional string)
encoder2.AddRange(new[] { new KeyValuePair<int, string>(5, "five") });

byte[] payload2 = encoder2.Build();
```

Notes on API
- BinaryCodec.Encoder() returns an ICodecEncoder. The interface supports Add / AddRange for many primitive types, SetEmpty / SetFull, Compress (implementation dependent) and Build.
- BinaryCodec.Decoder(byte[]) returns an ICodecDecoder. The interface supports IsEmpty / IsFull / HasData, Decode() to obtain IEnumerable<CodecItem>, OfType<T>() to filter by specific typed items and helper methods such as AsLong(), AsULong(), AsInt(), AsUInt(), AsShort() and AsUShort() to obtain converted sequences.
- CodecItem is the base type for decoded items. Typed subclasses exist for specific numeric types (CodecItemByte, CodecItemInt, CodecItemLong, CodecItemFloat, CodecItemDouble, CodecItemDecimal, CodecItemGuid, etc.). Each item exposes Number and optional Data properties.

When to use
- Compact on-the-wire representation of numeric metadata with optional small text fields.
- Lightweight telemetry, compact indexing metadata, or any scenario where a minimal custom binary format is desirable.

Limitations
- Maximum counts and maximum string lengths are enforced by the encoder (ushort limits for some fields) — callers should handle the exceptions thrown by the encoder when limits are exceeded.
- This library is intended for relatively small payloads and prioritizes compactness and simplicity over general-purpose serialization features.

Contributing
- Bug reports and pull requests are welcome. Follow the repository contribution guidelines and ensure unit tests (if added) pass on the supported target frameworks.

License
- See the LICENSE file in this repository for license details.

Contact
- For questions about the package or API, open an issue in the repository.
