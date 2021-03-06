using System;
using System.IO;
using System.Text;

public class EndiannessAwareBinaryReader : BinaryReader
{
	private readonly Endianness _endianness = Endianness.Little;
	private readonly Encoding _encoding = Encoding.UTF8;

	public EndiannessAwareBinaryReader(Stream input) : base(input)
	{
	}

	public EndiannessAwareBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
	{
		_encoding = encoding;
	}

	public EndiannessAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
	{
		_encoding = encoding;
	}

	public EndiannessAwareBinaryReader(Stream input, Endianness endianness) : base(input)
	{
		_endianness = endianness;
	}

	public EndiannessAwareBinaryReader(Stream input, Encoding encoding, Endianness endianness) : base(input, encoding)
	{
		_endianness = endianness;
		_encoding = encoding;
	}

	public EndiannessAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness) : base(input, encoding, leaveOpen)
	{
		_endianness = endianness;
		_encoding = encoding;
	}

	public override short ReadInt16() => ReadInt16(_endianness);

	public override int ReadInt32() => ReadInt32(_endianness);

	public override long ReadInt64() => ReadInt64(_endianness);

	public override ushort ReadUInt16() => ReadUInt16(_endianness);

	public override uint ReadUInt32() => ReadUInt32(_endianness);

	public override ulong ReadUInt64() => ReadUInt64(_endianness);

	public override float ReadSingle() => ReadSingle(_endianness);

	public override double ReadDouble() => ReadDouble(_endianness);

	public override string ReadString() => ReadString(_endianness);

	public short ReadInt16(Endianness endianness) => BitConverter.ToInt16(ReadForEndianness(sizeof(short), endianness), 0);

	public int ReadInt32(Endianness endianness) => BitConverter.ToInt32(ReadForEndianness(sizeof(int), endianness), 0);

	public long ReadInt64(Endianness endianness) => BitConverter.ToInt64(ReadForEndianness(sizeof(long), endianness), 0);

	public ushort ReadUInt16(Endianness endianness) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort), endianness), 0);

	public uint ReadUInt32(Endianness endianness) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint), endianness), 0);

	public ulong ReadUInt64(Endianness endianness) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong), endianness), 0);

	public float ReadSingle(Endianness endianness) => BitConverter.ToSingle(ReadForEndianness(sizeof(float), endianness), 0);

	public double ReadDouble(Endianness endianness) => BitConverter.ToDouble(ReadForEndianness(sizeof(double), endianness), 0);

	public string ReadString(Endianness endianness) => _encoding.GetString(ReadBytes(ReadInt16(endianness)));

	private byte[] ReadForEndianness(int bytesToRead, Endianness endianness)
	{
		var bytesRead = ReadBytes(bytesToRead);

		if ((endianness == Endianness.Little && !BitConverter.IsLittleEndian)
			|| (endianness == Endianness.Big && BitConverter.IsLittleEndian))
		{
			Array.Reverse(bytesRead);
		}

		return bytesRead;
	}
}
