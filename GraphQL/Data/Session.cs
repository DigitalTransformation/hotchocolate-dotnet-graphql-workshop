using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(400)]
        public string? Abstract { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan Duration =>
            this.EndTime?.Subtract(this.StartTime ?? this.EndTime ?? DateTimeOffset.MinValue)
            ?? TimeSpan.Zero;

        public int? TrackId { get; set; }

        public ICollection<SessionSpeaker> SessionSpeakers { get; set; } =
            new List<SessionSpeaker>();

        public ICollection<SessionAttendee> SessionAttendees { get; set; } =
            new List<SessionAttendee>();

        public Track? Track { get; set; }
    }
}