namespace FotoFaultFixerLib.ImageProcessor
{
    class CQueue
    {
        private int input;
        private int output;
        private int Len;
        private int[] Array;
        private bool full;

        public CQueue(int len)
        {
            Len = len;
            input = 0;
            output = 0;
            Array = new int[Len];
        }

        public void Put(int V)
        {
            if (input == (Len - 1))
            {
                full = true;
                return;
            }

            Array[input] = V;
            input++;
        }

        public int Get()
        {
            if (IsEmpty())
            {
                return -1;
            }

            int i = Array[output];
            if (output == (Len - 1))
            {
                output = 0;
            }
            else
            {
                output++;
            }

            if (full == true)
            {
                full = false;
            }

            return i;
        }

        public void Reset()
        {
            input = output = 0;
        }

        public bool IsEmpty()
        {
            return ((input == output) && !full);

        }
    }
}
