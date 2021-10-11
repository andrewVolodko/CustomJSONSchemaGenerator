using System;
using System.Diagnostics.CodeAnalysis;

namespace TestPart.Model.Meeting.DataBaseMeeting.RequiredAttendeesArray
{
    public class RequiredAttendee : IComparable<RequiredAttendee>
    {
        public NameEmailObj NameEmailObj { get; set; }

        public int CompareTo([DisallowNull] RequiredAttendee other)
        {
            return NameEmailObj.CompareTo(other.NameEmailObj);
        }
    }
}