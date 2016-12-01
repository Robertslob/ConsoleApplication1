using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication1
{
    public abstract class Locks
    {
        protected bool islocked = false;
        public abstract void LockList(Object o, int i);
        public abstract void LockTel(Object o, int i);
        public abstract void LockHash(Object o, int i);

    }

    public class OwnLock : Locks
    {
        int c = 0;

        public override void LockList(Object o, int i)
        {
            while (0 != Interlocked.CompareExchange(ref c, 1, 0))
                ;
            Checker.ListWriteIncr(i);
            Interlocked.Exchange(ref c, 0);
        }

        public override void LockHash(object o, int i)
        {
            while (0 != Interlocked.CompareExchange(ref c, 1, 0))
                ;
            Checker.HashWriteIncr(i);
            Interlocked.Exchange(ref c, 0);
        }

        public override void LockTel(object o, int i)
        {
            while (0 != Interlocked.CompareExchange(ref c, 1, 0))
                ;
            Checker.TelAdd(i);
            Interlocked.Exchange(ref c, 0);
        }
    }

    public class CSharpLock : Locks
    {
        public override void LockList(Object o, int i)
        {
            lock (o)
                Checker.ListWriteIncr(i);
        }

        public override void LockHash(object o, int i)
        {
            lock (o)
                Checker.HashWriteIncr(i);
        }

        public override void LockTel(object o, int i)
        {
            lock(o)
                Checker.TelAdd(i);
        }
    }
}
