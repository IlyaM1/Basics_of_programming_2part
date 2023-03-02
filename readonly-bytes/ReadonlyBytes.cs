using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
        private readonly byte[] bytes;
        private int _hash = -1; // Not counted hash
        public int Length { get { return bytes.Length; } }

        public ReadonlyBytes(params byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException();
            this.bytes = bytes;
            CountHash();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReadonlyBytes) || obj.GetType().IsSubclassOf(GetType()))
                return false;

            var bytes = obj as ReadonlyBytes;
            if (bytes.Length != Length) 
                return false;

            for (var i = 0; i < Length; i++)
                if (this.bytes[i] != bytes[i]) 
                    return false;

            return true;
        }

        private void CountHash()
        {
            unchecked // ensures overflow exceptions are thrown when an arithmetic operation results in an overflow
            {
                const int fnvOffsetBasis = unchecked((int)0x811c9dc5);
                const int fnvPrime = 16777619;

                for (int i = 0; i < bytes.Length; i++)
                    _hash = (_hash * fnvPrime) ^ bytes[i];
                _hash ^= fnvOffsetBasis;
            }
        }

        public override int GetHashCode() => _hash;

        public override string ToString()
        {
            if (Length == 0)
                return "[]";

            var str = new StringBuilder("[");
            for (var i = 0; i < Length - 1; i++)
            {
                str.Append(bytes[i]);
                str.Append(", ");
            }
            str.Append(bytes[Length - 1]);
            str.Append("]");

            return str.ToString();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var byteElement in bytes)
                yield return byteElement;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Length) throw new IndexOutOfRangeException();
                return bytes[index];
            }
        }
    }
}