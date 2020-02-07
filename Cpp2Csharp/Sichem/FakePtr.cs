using System;
using System.Linq;

namespace Zeus
{
    public struct FakeIntPtr
    {
        public static FakePtr<object>[] fakes = new FakePtr<object>[2];
        private static int ptr_static;
        public static readonly FakeIntPtr Zero = new FakeIntPtr(0);
        public int ptr;

        public void Alloc()
        {
            //if(fakes==null)
            //{

            //}
            if (ptr >= fakes.Length)
            {
                Array.Resize(ref fakes, ptr + 1);
            }
        }
        public FakeIntPtr(int v)
        {
            ptr = v;
            Alloc();
        }
        public FakeIntPtr(long v)
        {
            ptr = (int)v;
            Alloc();
        }
        public FakeIntPtr(object v)
        {
            ptr = ++ptr_static;
            Alloc();
        }
        public static bool operator ==(FakeIntPtr a, FakeIntPtr b)
        {
            return a.ptr == b.ptr;
        }
        public static bool operator !=(FakeIntPtr a, FakeIntPtr b)
        {
            return a.ptr != b.ptr;
        }
        public static explicit operator int(FakeIntPtr value)
        {
            return value.ptr;
        }
        public static explicit operator long(FakeIntPtr value)
        {
            return value.ptr;
        }
        public static explicit operator FakePtr<object>(FakeIntPtr value)
        {
            if (value.ptr <= 0)
            {
                return null;
            }
            return fakes[value.ptr - 1];
        }

        public static explicit operator FakeIntPtr(int value)
        {
            return new FakeIntPtr(value);
        }
        public static explicit operator FakeIntPtr(long value)
        {
            return new FakeIntPtr(value);
        }

        internal int ToInt32()
        {
            return ptr;
        }
        internal long ToInt64()
        {
            return ptr;
        }
    }
    public class FakeString : FakePtr<char>
    {
        public FakeString(string str)
        {
            this._array = str.ToArray();
        }
        public int GetLength()
        {
            return _array.Length - _offset;
        }
        public string GetString(int offset, int len)
        {
            char[] chars = new char[len];
            Array.Copy(this._array, this._offset, chars, 0, len);
            return new string(chars);
        }
        public string GetString()
        {
            return GetString(_offset, _array.Length - _offset);
        }

    }
    public class FakePtr<T>
    {
        public FakePtr()
        {
            _array = new T[0];

        }

        private static FakePtr<T> fs;
        public static ref FakePtr<T> Alloc()
        {
            if (fs == null)
            {
                fs = FakePtr<T>.Create();
            }
            return ref fs;
        }
        public /*readonly*/ T[] _array;
        public int _offset;



        public void Resize(int size)
        {
            Array.Resize(ref _array, size);
        }
        public T[] array_start => _array;
        public ref T Get(int d)
        {
            if (_offset + d >= _array.Length)
            {
                Array.Resize(ref _array, _offset + 1 + d);
            }
            return ref _array[_offset + d];
        }

        public T this[int index]
        {
            get
            {
                return _array[_offset + index];
            }

            set
            {
                if (_offset + index >= _array.Length)
                {
                    Array.Resize(ref _array, _offset + 1 + index);
                }
                _array[_offset + index] = value;
            }
        }

        public T this[long index]
        {
            get
            {
                return _array[_offset + index];
            }

            set
            {
                _array[_offset + index] = value;
            }
        }

        public T Value
        {
            get
            {
                return this[0];
            }

            set
            {
                this[0] = value;
            }
        }

        public FakePtr(T[] data)
        {
            _array = data;
        }

        public FakePtr(T value)
        {
            _array = new T[1];
            _array[0] = value;
        }
        public void memcpy_local(FakePtr<T> ps, int count)
        {
            Array.Copy(ps._array, ps._offset, _array, _offset, count);

        }
        public static FakePtr<T> operator +(FakePtr<T> p, int offset)
        {
            if (offset + p._offset >= p._array.Length)
            {
                Array.Resize(ref p._array, offset + p._offset + 1);
            }
            return new FakePtr<T>(p._array) { _offset = p._offset + offset };
        }

        public static FakePtr<T> operator -(FakePtr<T> p, int offset)
        {
            return p + -offset;
        }

        public static FakePtr<T> operator +(FakePtr<T> p, uint offset)
        {

            return p + (int)offset;
        }

        public static FakePtr<T> operator -(FakePtr<T> p, uint offset)
        {
            return p - (int)offset;
        }

        public static FakePtr<T> operator +(FakePtr<T> p, long offset)
        {
            return p + (int)offset;
        }

        public static FakePtr<T> operator -(FakePtr<T> p, long offset)
        {
            return p - (int)offset;
        }

        public static FakePtr<T> operator ++(FakePtr<T> p)
        {
            return p + 1;
        }

        public static bool operator ==(FakePtr<T> p1, FakePtr<T> p2)
        {
            if (p1 is null && p2 is null)
            {
                return true;
            }

            if (p1 is null)
            {
                return false;
            }

            if (p2 is null)
            {
                return false;
            }

            return p1._array == p2._array && p1._offset == p2._offset;
        }

        public static bool operator !=(FakePtr<T> p1, FakePtr<T> p2)
        {
            return !(p1 == p2);
        }

        public static FakePtr<T> CreateWithSize(int size)
        {
            var result = new FakePtr<T>(new T[size]);

            for (int i = 0; i < size; ++i)
            {
                result[i] = default(T);
            }

            return result;
        }

        public static FakePtr<T> CreateWithSize(long size)
        {
            return CreateWithSize((int)size);
        }

        public static FakePtr<T> Create()
        {
            return CreateWithSize(1);
        }

        public static void memcpy(FakePtr<T> target, FakePtr<T> source, int count)
        {

            for (int i = 0; i < count; i++) { target[i] = source[i]; }
        }
        public static void memset(FakePtr<T> target, int count)
        {
            for (int i = 0; i < count; i++) { target[i] = default(T); }
        }
        public static FakePtr<T> malloc(int count)
        {
            try { return new FakePtr<T>(new T[count]); }
            catch (OutOfMemoryException) { return new FakePtr<T>(new T[1]); }
        }
        //public static void free(FakePtr p) { } // no-op

        public static void assert(bool condition) { }
        public static void assert(int condition) { }

        public override string ToString()
        {

            return base.ToString();
        }
    }

}
