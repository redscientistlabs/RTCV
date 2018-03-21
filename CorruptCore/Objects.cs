using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.CorruptCore
{
    //Contains all the objects used in CorruptCore

    public class BlastLayer
    {

    }

    public class BlastUnit
    {
        public CC_Source source = null;
        public CC_Value value = null;
        public PostProcessor Transformer = null;
    }

    public abstract class CC_Source
    {

    }

    public abstract class CC_Value
    {

    }


}
