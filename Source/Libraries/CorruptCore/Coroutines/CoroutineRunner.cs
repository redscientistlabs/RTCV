using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.Coroutines
{
    /// <summary>
    /// Used to implement coroutines
    /// </summary>
    public class CoroutineRunner
    {

        LinkedList<Coroutine> subCoroutines = new LinkedList<Coroutine>();

        public void StopAll()
        {
            foreach (var cor in subCoroutines)
            {
                cor.Stop();
            }
            subCoroutines.Clear();
        }

        public bool RemoveCoroutine(Coroutine coroutine)
        {
            return subCoroutines.Remove(coroutine);
        }
        public void AddCoroutineLast(Coroutine coroutine)
        {
            subCoroutines.AddLast(coroutine);
        }

        /// <summary>
        /// Starts a coroutine. IMPORTANT: coroutines always execute to the first yield when starting, add a "yield return null;" to skip this
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(IEnumerator<Yielder> enumerator)
        {
            Coroutine res = new Coroutine(enumerator);
            subCoroutines.AddLast(res);
            return res;
        }

        public void Update()
        {
            var curCoroutineNode = subCoroutines.First;
            while (curCoroutineNode != null)
            {
                Coroutine curCoroutine = curCoroutineNode.Value;
                curCoroutine.DoCycle();
                var nextNode = curCoroutineNode.Next;
                if (curCoroutine.IsComplete)
                {
                    curCoroutineNode.Value = null;
                    subCoroutines.Remove(curCoroutineNode);
                }
                curCoroutineNode = nextNode;
            }
        }

    }
}
