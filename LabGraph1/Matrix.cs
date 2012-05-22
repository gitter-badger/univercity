using System;

namespace LabGraph1
{
    class Matrix
    {
        private readonly double[,] _mass;
        readonly byte _sizeX;
        readonly byte _sizeY;

        public Matrix(byte sizeX, byte sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _mass = new double[_sizeX, _sizeY];
        }
        public Matrix(byte sizeX, byte sizeY, params double[] list)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _mass = new double[_sizeX, _sizeY];
            if (list.Length == sizeY * sizeX)
            {
                for (byte i = 0; i < sizeX; i++)
                {
                    for (byte j = 0; j < sizeY; j++)
                    {
                        _mass[i, j] = list[i * sizeY + j];
                    }
                }
            }
            else new ArgumentException("Arguments do not match");
        }

        public void Filling(params double[] list)
        {
            for (byte i = 0; i < _sizeX; i++)
            {
                for (byte j = 0; j < _sizeY; j++)
                {
                    _mass[i, j] = list[i*_sizeY+j];
                }
            }
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left._sizeY != right._sizeX) throw new ArgumentException("Matrix not multiplied!");
            var result = new Matrix(left._sizeX, right._sizeY);
            for (byte i = 0; i < left._sizeX; i++)
            {
                for (byte j = 0; j < right._sizeY; j++)
                {
                    double s = 0;
                    for (byte k = 0; k < left._sizeY; k++)
                    {
                        s += left._mass[i, k] * right._mass[k, j];
                    }
                    result._mass[i, j] = s;
                }
            }
            return result;
        }
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left._sizeX != right._sizeX || left._sizeY != right._sizeY)
                throw new ArgumentException("Matrix not valid!");
            var _out = new Matrix(left._sizeX, left._sizeY);
            for (byte i = 0; i < left._sizeX; i++)
            {
                for (byte j = 0; j < left._sizeY; j++)
                {
                    _out._mass[i, j] = left._mass[i, j] + right._mass[i, j];
                }
            }
            return _out;
        }

        public void Equally(Matrix _in)
        {
            if (_in._sizeX ==_sizeX && _in._sizeY == _sizeY)
            {
                for (byte i = 0; i < _sizeX; i++)
                {
                    for (byte j = 0; j < _sizeY; j++)
                    {
                        _mass[i, j] = _in._mass[i, j];
                    }
                }
            }
            else throw new ArgumentException("Matrix not valid!");
        }

        public byte SizeX
        {
            get { return _sizeX; }
        }
        public byte SizeY
        {
            get { return _sizeY; }
        }

        public float GetItem(byte x, byte y)
        {
            return Convert.ToSingle(_mass[x, y]);
        }
        public void SetItem(byte x, byte y, double val)
        {
            _mass[x, y] = val;
        }
    }
}