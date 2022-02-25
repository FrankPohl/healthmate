using System;

namespace HealthMate.Models
{

    public enum Intent
    {
        BloodPressure,
        Glucose,
        Pulse,
        Temperature,
        Cancel,
        None
    };
    public class MeasuredItem
    {
        public string Id { get; set; }
        public Intent MeasurementType { get; set; }
        public DateTime MeasurementDateTime { get; set; }
        public double Measurement { get; set; }
        public double SysValue { get; set; }
        public double DiaValue { get; set; }
    }
}