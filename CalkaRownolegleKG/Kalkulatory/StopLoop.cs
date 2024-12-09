using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalkaRownolegleKG.Kalkulatory
{
    public class StopLoop
    {
        private CancellationTokenSource _cts;
        public StopLoop()
        {
            _cts = new CancellationTokenSource();
            
        }
        public CancellationToken Token { get { return _cts.Token; } }
        public void Stop() { _cts.Cancel(); }

        public void Reset()
        {
            if(_cts.IsCancellationRequested)
            {
                _cts.Dispose();
                _cts = new CancellationTokenSource();
            }
        }

        
    }
}
