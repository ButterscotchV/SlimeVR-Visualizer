using System;
using System.IO;
using System.Text;

public class EndiannessAwareBinaryWriter : BinaryWriter
{
	private readonly Endianness _endianness = Endianness.Little;
	private readonly Encoding _encoding = Encoding.UTF8;

	public EndiannessAwareBinaryWriter(Stream output) : base(output)
	{
	}

	public EndiannessAwareBinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
	{
		_encoding = encoding;
	}

	public EndiannessAwareBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
	{
		_encoding = encoding;
	}

	public EndiannessAwareBinaryWriter(Stream output, Endianness endianness) : base(output)
	{
		_endianness = endianness;
	}

	public EndiannessAwareBinaryWriter(Stream output, Encoding encoding, Endianness endianness) : base(output, encoding)
	{
		_endianness = endianness;
		_encoding = encoding;
	}

	public EndiannessAwareBinaryWriter(Stream output, Encoding encoding, bool leaveOpen, Endianness endianness) : base(output, encoding, leaveOpen)
	{
		_endianness = endianness;
		_encoding = encoding;
	}

	public override void Write(short val) => Write(val, _endianness);

	public override void Write(int val) => Write(val, _endianness);

	public override void Write(long val) => Write(val, _endianness);

	public override void Write(ushort val) => Write(val, _endianness);

	public override void Write(uint val) => Write(val, _endianness);

	public override void Write(ulong val) => Write(val, _endianness);

	public override void Write(float val) => Write(val, _endianness);

	public override void Write(double val) => Write(val, _endianness);

	public override void Write(string val) => Write(val, _endianness);

	public void Write(short val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(int val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(long val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(ushort val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(uint val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(float val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(double val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(ulong val, Endianness endianness) => WriteForEndianness(BitConverter.GetBytes(val), endianness);

	public void Write(string val, Endianness endianness)
	{
		byte[] bytes = _encoding.GetBytes(val);
		Write((short)bytes.Length, endianness);
		Write(bytes);
	}

	private void WriteForEndianness(byte[] bytesToWrite, Endianness endianness)
	{
		if ((endianness == Endianness.Little && !BitConverter.IsLittleEndian)
			|| (endianness == Endianness.Big && BitConverter.IsLittleEndian))
		{
			Array.Reverse(bytesToWrite);
		}

		Write(bytesToWrite);
	}
}
