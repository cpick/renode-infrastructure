//
// Copyright (c) 2010-2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using Antmicro.Renode.Core;
using Antmicro.Renode.Logging;

namespace Antmicro.Renode.Peripherals.Network
{
    public class Quectel_BC66 : QuectelModem
    {
        public Quectel_BC66(Machine machine, string imeiNumber = DefaultImeiNumber,
            string softwareVersionNumber = DefaultSoftwareVersionNumber,
            string serialNumber = DefaultSerialNumber) : base(machine, imeiNumber, softwareVersionNumber, serialNumber)
        {
        }

        // QCFG - System Configuration
        [AtCommand("AT+QCFG", CommandType.Write)]
        protected override Response Qcfg(string function, int value)
        {
            switch(function)
            {
                case "dsevent":
                    deepSleepEventEnabled = value != 0;
                    break;
                case "activetimer": // whether to use the value of active timer or not
                case "atlocktime": // Sleep Lock duration after AT command
                case "autopdn": // PDN auto activation option
                case "combinedattach": // combined attach
                case "epco": // extended protocol configuration options
                case "initlocktime": // initial Sleep Lock duration after reboot or deep sleep wake up
                case "multidrb": // enable multi-DRB
                case "ripin": // default output level for the RI pin
                case "up": // enable user plane function
                case "upopt": // enable user plane optimization
                case "urc/ri/mask": // whether to trigger RI pin behavior when URC is reported
                case "vbattimes": // voltage detection cycle for the AT+QVBATT command
                    this.Log(LogLevel.Warning, "Config value '{0}' set to {1}, not implemented", function, value);
                    break;
                default:
                    return base.Qcfg(function, value);
            }
            return Ok;
        }

        protected override bool IsValidContextId(int id)
        {
            return id == 1;
        }

        protected override string Vendor => "Quectel_Ltd";
        protected override string ModelName => "Quectel_BC66";
        protected override string Revision => "Revision: MTK_2625";
        protected override string ManufacturerRevision => "BC66NBR01A01";
        protected override string SoftwareRevision => "01.002.01.002";

        private const string DefaultImeiNumber = "866818039921444";
        private const string DefaultSoftwareVersionNumber = "31";
        private const string DefaultSerialNumber = "<serial number>";
    }
}