using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoModule {
    public class Worker {

        readonly WorkerSettings _settings;

        public Worker(WorkerSettings settings) {
            _settings = settings;
        }

        public void DoWork() {
            Console.WriteLine("DemoModule Worker Connecting to {0} on port {1}", _settings.Server, _settings.Port);
        }
    }
}
