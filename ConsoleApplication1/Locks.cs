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
        public abstract void LockList(int i);
        public abstract void LockTel(int i);
        public abstract void LockHash(int i);
    }

    public class OwnLock : Locks
    {
        int c = 0;

        public override void LockList(int i)
        {
            while (0 != Interlocked.CompareExchange(ref c, 1, 0))
                ;
            Checker.ListWriteIncr(i);
            c = 0;
        }

        public override void LockHash(int i)
        {
            while (0 != Interlocked.CompareExchange(ref c, 1, 0))
                ;
            Checker.HashWriteIncr(i);
            c = 0;
        }

        public override void LockTel(int i)
        {
            while (0 != Interlocked.CompareExchange(ref c, 1, 0))
                ;
            Checker.TelAdd(i);
            c = 0;
        }
    }

    public class CSharpLock : Locks
    {
        private Object o = new Object();

        public override void LockList(int i)
        {
            lock (o)
                Checker.ListWriteIncr(i);
        }

        public override void LockHash(int i)
        {
            lock (o)
                Checker.HashWriteIncr(i);
        }

        public override void LockTel(int i)
        {
            lock(o)
                Checker.TelAdd(i);
        }
    }
}
