using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTTriggerFunction
{
    public class TelemetryMessage
    {
        public string deviceId { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
    }

}
