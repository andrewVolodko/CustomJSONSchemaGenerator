using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TestPart.Model.Meeting.DataBaseMeeting.RequiredAttendeesArray;

namespace TestPart.Model.Meeting.DataBaseMeeting
{
    public class DataBaseMeeting : BaseMeeting, IComparable<DataBaseMeeting>
    {
        public string EventId { get; set; }

        public string Source { get; set; }

        public RequiredAttendee[] RequiredAttendees { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }


        public DataBaseMeeting(string eventId, ItemId itemId, string locationName, string subject, RequiredAttendee[] requiredAttendees, DateTime start, DateTime end) : this(itemId, locationName, subject, requiredAttendees, start, end)
        {
            EventId = eventId;
        }

        public DataBaseMeeting(ItemId itemId, string locationName, string subject, RequiredAttendee[] requiredAttendees, DateTime start, DateTime end) : base(itemId, locationName, subject)
        {
            RequiredAttendees = requiredAttendees;
            Start = start;
            End = end;
        }

        public DataBaseMeeting(string locationName, string subject, RequiredAttendee[] requiredAttendees,
            DateTime start, DateTime end) : base(locationName, subject)
        {
            RequiredAttendees = requiredAttendees;
            Start = start;
            End = end;
        }

        public DataBaseMeeting AddSource(string source)
        {
            Source = source;
            return this;
        }

        public string GetRoomEmail(IEnumerable<NameEmailObj> allFreeRooms)
        {
            return allFreeRooms.Single(room => room.Name == LocationName).Email;
        }

        public int GetDuration()
        {
            return (int)(End - Start).TotalMinutes;
        }


        public int CompareTo([DisallowNull] DataBaseMeeting other)
        {
            return Start.CompareTo(other.Start);
        }
    }
}



            
