namespace EncryptLibrary
{
    
    public class HashBasedAlgorithm : IEncryptingAlgorithm
    {
        /// <summary>
        /// Constant value for encryption algorithm
        /// </summary>
        private const long _pFactor = 31;

        /// <summary>
        /// Modulo used for encryption algorithm. Algorithm requires prime modulo, this is closeset for byte bound.
        /// </summary>
        private const long _modulo = 241;

        public object Key
        {
            get
            {
                return _key;
            }

            set
            {
                // predict negative values also
                _key = ((value.GetHashCode() % _modulo) + _modulo) % _modulo;
            }
        }
        private long _key = 0;


        private long _step = 1;
        private long _inverse = 1;

        private long _inv_multiplier;
        public HashBasedAlgorithm() : this(0) { }

        public HashBasedAlgorithm(object key)
        {
            Key = key;
            _inv_multiplier = MathExtension.BinaryExponentialModulo(_pFactor, _modulo - 2, _modulo);
        }

        public void ClearCache()
        {
            _step = 1;
            _inverse = 1;
        }

        public byte DecryptByte(byte b)
        {
            byte answer = (byte)((b ^ _key) * _step % _modulo);

            _step = _step * _pFactor % _modulo;

            return answer;
        }

        public byte EncryptByte(byte b)
        {
            byte answer = (byte)((b * _inverse % _modulo) ^ _key);

            _inverse = _inverse * _inv_multiplier % _modulo;

            return answer;
        }

        public object Clone()
        {
            HashBasedAlgorithm clone = new HashBasedAlgorithm();
            clone._key = _key;
            clone._inv_multiplier = _inv_multiplier;
            clone._step = _step;
            clone._inverse = _inverse;
            return clone;
        }
    }
}
