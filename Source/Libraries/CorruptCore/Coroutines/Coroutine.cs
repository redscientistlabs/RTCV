using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.Coroutines
{
    //using IEnumerator = IEnumerator<CoroutineConditional>;

    public class Coroutine
    {
        //False of movenext means done;
        public bool IsComplete { get; protected set; } = false;
        public bool Paused { get; protected set; } = false;
        CoroutineRunner system;
        IEnumerator<Yielder> coroutine;
        Yielder currentConditional;
        public Coroutine(IEnumerator<Yielder> coroutine, CoroutineRunner sys)
        {
            this.coroutine = coroutine;
            this.system = sys;
            Paused = false;
            currentConditional = coroutine.Current;
        }

        public void Pause(bool pause = true)
        {
            Paused = pause;
        }

        public void Unpause()
        {
            Paused = false;
        }

        public void StopCoroutine()
        {
            IsComplete = true;
            system.RemoveCoroutine(this);
        }

        public void Stop()
        {
            currentConditional = null;
            IsComplete = true;
        }

        public void Restart()
        {
            coroutine.Reset();
            Paused = false;
            currentConditional = null;
            system.AddCoroutineLast(this);
        }

        public void DoCycle()
        {
            if(Paused)
            {
                //Do nothing
            }
            else if (currentConditional == null || currentConditional.Process())
            {
                IsComplete = !coroutine.MoveNext();
                currentConditional = coroutine.Current;
            }
        }
    }
}
