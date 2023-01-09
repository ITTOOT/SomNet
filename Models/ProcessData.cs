using CsvHelper.Configuration.Attributes;
using System.Collections;

namespace SomNet.Models
{
    //CSV values
    public class Sta70Data
    {
        [Index(0)]
        public string? EntryTime { get; set; } = null;

        [Index(1)]
        public string? OverAllResult { get; set; } = null;

        [Index(2)]
        public string? CycleTime { get; set; } = null;

        [Index(3)]
        public string? CapnutTypeResult { get; set; } = null;

        [Index(4)]
        public string? OMVSpringResult { get; set; } = null;

        [Index(5)]
        public string? NozzlePreLoadForce { get; set; } = null;

        [Index(6)]
        public string? NozzlePreLoadPosition { get; set; } = null;

        [Index(7)]
        public string? StackBuildResult { get; set; } = null;

        [Index(8)]
        public string? CapnutTorque { get; set; } = null;

        [Index(9)]
        public string? CapnutTorqueAngle { get; set; } = null;

        [Index(10)]
        public string? CapnutFinalAngle { get; set; } = null;

        [Index(11)]
        public string? CapnutAssemblyResult { get; set; } = null;
    }

    //Actual values
    public class Sta70DataDTO
    {
        [Index(0)]
        public DateTime? EntryTime { get; set; } = null;

        [Index(1)]
        public int? OverAllResult { get; set; } = null;

        [Index(2)]
        public double? CycleTime { get; set; } = null;

        [Index(3)]
        public int? CapnutTypeResult { get; set; } = null;

        [Index(4)]
        public int? OMVSpringResult { get; set; } = null;

        [Index(5)]
        public double? NozzlePreLoadForce { get; set; } = null;

        [Index(6)]
        public double? NozzlePreLoadPosition { get; set; } = null;

        [Index(7)]
        public int? StackBuildResult { get; set; } = null;

        [Index(8)]
        public double? CapnutTorque { get; set; } = null;

        [Index(9)]
        public double? CapnutTorqueAngle { get; set; } = null;

        [Index(10)]
        public double? CapnutFinalAngle { get; set; } = null;

        [Index(11)]
        public int? CapnutAssemblyResult { get; set; } = null;

        //Added by SoM
        [Index(12)]
        public int? Id { get; set; } = null;

        [Index(13)]
        public string? Label { get; set; } = null;

        [Index(14)]
        public DateTime? Timestamp { get; set; } = null;
    }
}
