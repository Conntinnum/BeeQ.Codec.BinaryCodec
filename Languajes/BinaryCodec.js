/**
 * BinaryCodec.js - JavaScript decoder for BeeQ.Codec.BinaryCodec payloads.
 *
 * The BinaryCodec class exposes a single static method: decode(bytes).
 * All constants and helpers are private to the class scope.
 * Supports string keys in addition to numeric types.
 */

class BinaryCodec {
    // Private constants
    static #TAG_FULL = 0xff;
    static #TAG_WITHDATA = 0x00;

    static #TAG_NUMBERS_BYTE = 0x01;
    static #TAG_NUMBERS_USHORT = 0x02;
    static #TAG_NUMBERS_SHORT = 0x03;
    static #TAG_NUMBERS_UINT = 0x04;
    static #TAG_NUMBERS_INT = 0x05;
    static #TAG_NUMBERS_ULONG = 0x06;
    static #TAG_NUMBERS_LONG = 0x07;
    static #TAG_NUMBERS_FLOAT = 0x08;
    static #TAG_NUMBERS_DOUBLE = 0x09;
    static #TAG_NUMBERS_DECIMAL = 0x0A;
    static #TAG_NUMBERS_GUID = 0x0B;
    static #TAG_NUMBERS_STRING = 0x0C;

    static #TAG_NUMBERS_WHITDATA_PREFIX = 0x80;
    static #TAG_NUMBERS_WHITDATA_BYTE = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_BYTE;
    static #TAG_NUMBERS_WHITDATA_USHORT = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_USHORT;
    static #TAG_NUMBERS_WHITDATA_SHORT = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_SHORT;
    static #TAG_NUMBERS_WHITDATA_UINT = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_UINT;
    static #TAG_NUMBERS_WHITDATA_INT = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_INT;
    static #TAG_NUMBERS_WHITDATA_ULONG = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_ULONG;
    static #TAG_NUMBERS_WHITDATA_LONG = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_LONG;
    static #TAG_NUMBERS_WHITDATA_FLOAT = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_FLOAT;
    static #TAG_NUMBERS_WHITDATA_DOUBLE = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_DOUBLE;
    static #TAG_NUMBERS_WHITDATA_DECIMAL = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_DECIMAL;
    static #TAG_NUMBERS_WHITDATA_GUID = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_GUID;
    static #TAG_NUMBERS_WHITDATA_STRING = BinaryCodec.#TAG_NUMBERS_WHITDATA_PREFIX + BinaryCodec.#TAG_NUMBERS_STRING;

    // Private helper methods
    static #ensureUint8Array(input) {
        if (input instanceof Uint8Array) return input;
        if (Array.isArray(input)) return new Uint8Array(input);
        if (input instanceof ArrayBuffer) return new Uint8Array(input);
        throw new TypeError('Unsupported input type for decode; expected Uint8Array, Array or ArrayBuffer.');
    }

    static #readUInt16LE(bytes, offset) {
        return bytes[offset] | (bytes[offset + 1] << 8);
    }

    static #readInt16LE(bytes, offset) {
        const v = bytes[offset] | (bytes[offset + 1] << 8);
        return v & 0x8000 ? v - 0x10000 : v;
    }

    static #readUInt32LE(bytes, offset) {
        return (bytes[offset] | (bytes[offset + 1] << 8) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 24)) >>> 0;
    }

    static #readInt32LE(bytes, offset) {
        return bytes[offset] | (bytes[offset + 1] << 8) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 24);
    }

    static #readFloatLE(bytes, offset) {
        const dv = new DataView(bytes.buffer, bytes.byteOffset + offset, 4);
        return dv.getFloat32(0, true);
    }

    static #readDoubleLE(bytes, offset) {
        const dv = new DataView(bytes.buffer, bytes.byteOffset + offset, 8);
        return dv.getFloat64(0, true);
    }

    static #readUInt64LEAsNumber(bytes, offset) {
        const lo = BinaryCodec.#readUInt32LE(bytes, offset);
        const hi = BinaryCodec.#readUInt32LE(bytes, offset + 4);
        const val = hi * 0x100000000 + lo;
        if (val > Number.MAX_SAFE_INTEGER) throw new RangeError('Decoded 64-bit unsigned integer exceeds Number.MAX_SAFE_INTEGER');
        return val;
    }

    static #readInt64LEAsNumber(bytes, offset) {
        const lo = BinaryCodec.#readUInt32LE(bytes, offset);
        const hi = BinaryCodec.#readInt32LE(bytes, offset + 4);
        const val = hi * 0x100000000 + lo;
        if (val > Number.MAX_SAFE_INTEGER || val < -Number.MAX_SAFE_INTEGER) throw new RangeError('Decoded 64-bit integer exceeds Number.MAX_SAFE_INTEGER');
        return val;
    }

    static #readDecimalLE(bytes, offset) {
        const lo = BinaryCodec.#readUInt32LE(bytes, offset);
        const mid = BinaryCodec.#readUInt32LE(bytes, offset + 4);
        const hi = BinaryCodec.#readUInt32LE(bytes, offset + 8);
        const flags = BinaryCodec.#readInt32LE(bytes, offset + 12);

        const isNegative = (flags & 0x80000000) !== 0;
        const scale = (flags >>> 16) & 0xff;

        let value = (BigInt(hi) << 64n) | (BigInt(mid) << 32n) | BigInt(lo);
        if (isNegative) value = -value;

        const divisor = 10n ** BigInt(scale);
        const intPart = value / divisor;
        const rem = value % divisor;
        if (rem === 0n) return Number(intPart);

        const sign = value < 0n ? -1 : 1;
        const absInt = BigInt(intPart < 0n ? -intPart : intPart);
        const absRem = BigInt(rem < 0n ? -rem : rem);
        const fracStr = absRem.toString().padStart(scale, '0');
        const num = Number(`${absInt.toString()}.${fracStr}`);
        return sign * num;
    }

    static #readGuidLE(bytes, offset) {
        const b = bytes.subarray(offset, offset + 16);
        const hex = [];
        for (let i = 0; i < 16; i++) hex.push(b[i].toString(16).padStart(2, '0'));
        const d1 = hex[3] + hex[2] + hex[1] + hex[0];
        const d2 = hex[5] + hex[4];
        const d3 = hex[7] + hex[6];
        const d4 = hex[8] + hex[9];
        const d5 = hex.slice(10, 16).join('');
        return `${d1}-${d2}-${d3}-${d4}-${d5}`;
    }

    /**
     * Decodes a byte array produced by the .NET BinaryCodec encoder.
     * @param {Uint8Array|number[]|ArrayBuffer} bytesInput - The input bytes.
     * @returns {Array<{number: number|string, data?: string}>} Decoded items.
     */
    static decode(bytesInput) {
        const bytes = BinaryCodec.#ensureUint8Array(bytesInput);
        const totalCount = bytes.length;
        if (totalCount === 0 || bytes[0] === BinaryCodec.#TAG_FULL) return [];
        if (totalCount < 2) throw new Error('Invalid code format');

        const result = [];
        let index = 1;

        const decoder = new TextDecoder();

        function decodeBlock(tag) {
            switch (tag) {
                case BinaryCodec.#TAG_NUMBERS_BYTE: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++) result.push({ number: bytes[ptr + i] });
                    index = ptr + cnt;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_USHORT: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 2) result.push({ number: BinaryCodec.#readUInt16LE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_SHORT: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 2) result.push({ number: BinaryCodec.#readInt16LE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_UINT: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 4) result.push({ number: BinaryCodec.#readUInt32LE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_INT: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 4) result.push({ number: BinaryCodec.#readInt32LE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_ULONG: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 8) result.push({ number: BinaryCodec.#readUInt64LEAsNumber(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_LONG: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 8) result.push({ number: BinaryCodec.#readInt64LEAsNumber(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_FLOAT: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 4) result.push({ number: BinaryCodec.#readFloatLE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_DOUBLE: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 8) result.push({ number: BinaryCodec.#readDoubleLE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_DECIMAL: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 16) result.push({ number: BinaryCodec.#readDecimalLE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_GUID: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++, ptr += 16) result.push({ number: BinaryCodec.#readGuidLE(bytes, ptr) });
                    index = ptr;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_STRING: {
                    const cnt = BinaryCodec.#readUInt16LE(bytes, index + 1);
                    let ptr = index + 3;
                    for (let i = 0; i < cnt; i++) {
                        const len = BinaryCodec.#readUInt16LE(bytes, ptr);
                        ptr += 2;
                        const value = decoder.decode(bytes.subarray(ptr, ptr + len));
                        result.push({ number: value });
                        ptr += len;
                    }
                    index = ptr;
                    return;
                }

                // with data variants
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_BYTE: {
                    let ptr = index + 1;
                    const num = bytes[ptr];
                    ptr += 1;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_USHORT: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_SHORT: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readInt16LE(bytes, ptr);
                    ptr += 2;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_UINT: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readUInt32LE(bytes, ptr);
                    ptr += 4;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_INT: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readInt32LE(bytes, ptr);
                    ptr += 4;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_ULONG: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readUInt64LEAsNumber(bytes, ptr);
                    ptr += 8;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_LONG: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readInt64LEAsNumber(bytes, ptr);
                    ptr += 8;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_FLOAT: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readFloatLE(bytes, ptr);
                    ptr += 4;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_DOUBLE: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readDoubleLE(bytes, ptr);
                    ptr += 8;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_DECIMAL: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readDecimalLE(bytes, ptr);
                    ptr += 16;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_GUID: {
                    let ptr = index + 1;
                    const num = BinaryCodec.#readGuidLE(bytes, ptr);
                    ptr += 16;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }
                case BinaryCodec.#TAG_NUMBERS_WHITDATA_STRING: {
                    let ptr = index + 1;
                    const len = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const num = decoder.decode(bytes.subarray(ptr, ptr + len));
                    ptr += len;
                    const size = BinaryCodec.#readUInt16LE(bytes, ptr);
                    ptr += 2;
                    const text = decoder.decode(bytes.subarray(ptr, ptr + size));
                    result.push({ number: num, data: text });
                    index = ptr + size;
                    return;
                }

                default:
                    throw new Error(`Invalid section format: 0x${tag.toString(16)}`);
            }
        }

        do {
            const tag = bytes[index];
            decodeBlock(tag);
        } while (index < totalCount);

        return result;
    }
}

export { BinaryCodec };
