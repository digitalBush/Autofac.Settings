using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo {
    public class Worker {

        readonly WorkerSettings _settings;

        public Worker(WorkerSettings settings) {
            _settings = settings;
        }

        public void DoWork() {
            Console.WriteLine("Connecting to {0} on port {1}", _settings.Server, _settings.Port);
        }
    }
}
