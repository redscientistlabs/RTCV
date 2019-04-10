using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.CorruptCore
{
    //Should contain everything to manage the workflow of the generator

    public interface IChainable
    {

    }

    public static class WorkflowEngine
    {
        public static EngineConfig CurrentConfig
        {
            get { return _currentConfig; }
        }
        private static EngineConfig _currentConfig = null;

        public static void UpdateConfig(EngineConfig CurrentConfig)
        {
            //Set or update config here
        }

        public static BlastLayer Execute()
        {
            if (_currentConfig == null)
                throw new Exception("NO CONFIG LOADED");

            return null;
        }

    }


}
