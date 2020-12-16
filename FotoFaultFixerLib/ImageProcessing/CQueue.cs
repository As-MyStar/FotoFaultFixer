namespace FotoFaultFixerLib.ImageProcessing
{
    class CQueue
    {
        private int _input;
        private int _output;
        private int _len;
        private int[] _arr;
        private bool _isFull;

        public CQueue(int length)
        {
            _len = length;
            _input = 0;
            _output = 0;
            _arr = new int[_len];
        }

        public void Put(int V)
        {
            if (_input == (_len - 1))
            {
                _isFull = true;
                return;
            }

            _arr[_input] = V;
            _input++;
        }

        public int Get()
        {
            if (IsEmpty())
            {
                return -1;
            }

            int i = _arr[_output];
            if (_output == (_len - 1))
            {
                _output = 0;
            }
            else
            {
                _output++;
            }

            if (_isFull == true)
            {
                _isFull = false;
            }

            return i;
        }

        public void Reset()
        {
            _input = _output = 0;
        }

        public bool IsEmpty()
        {
            return ((_input == _output) && !_isFull);
        }
    }
}
