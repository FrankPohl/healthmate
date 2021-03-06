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
    public enum Measurement
    {
        BloodPressure,
        Glucose,
        Pulse,
        Temperature,
        NotSet
    };
    public class MeasuredItem
    {
        public  MeasuredItem()
        {
            MeasurementType = Models.Measurement.NotSet;
            MeasuredValue = 0;
            SysValue = 0;
            DiaValue = 0;
            MeasurementDateTime = DateTime.MinValue;
        }
        public string Id { get; set; }
        public Measurement MeasurementType { get; set; }
        public DateTime MeasurementDateTime { get; set; }
        public double MeasuredValue { get; set; }
        public double SysValue { get; set; }
        public double DiaValue { get; set; }
    }
}