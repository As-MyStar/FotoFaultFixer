using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_Tester.Common
{
    class ValuePrediction
    {
        [ColumnName("Score")]
        public float ColorValue { get; set;  }
    }
}
