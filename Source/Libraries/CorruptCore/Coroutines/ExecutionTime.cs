using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializeTest.Coroutines
{
    [Serializable]
    enum ExecutionTime
    {
        LOAD = 0,
        PRE_EXECUTE = 1,
        POST_EXECUTE = 2
    }
}
