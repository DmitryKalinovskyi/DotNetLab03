namespace EncryptLibrary
{
    public class MathExtension
    {
        public static long BinaryExponential(long value, long step)
        {
            if (step == 0) return 1;

            if (step % 2 == 0) return BinaryExponential(value * value, step / 2);
            return BinaryExponential(value, step -1 ) * value;
        }

        public static long BinaryExponentialModulo(long value, long step, long modulo)
        {
            if (step == 0) return 1;

            if (step % 2 == 0) return BinaryExponentialModulo(value * value % modulo, step / 2, modulo);
            return BinaryExponentialModulo(value, step - 1, modulo) * value % modulo;
        }

        public static long ModuloInverse(long value, long modulo)
        {
            return BinaryExponentialModulo(value, modulo - 2, modulo);
        }
    }
}
