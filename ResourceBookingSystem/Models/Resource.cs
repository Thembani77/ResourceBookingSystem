using System.ComponentModel.DataAnnotations;

namespace ResourceBookingSystem.Models
{
    public class Resource
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public int? Capacity { get; set; }

        public bool IsAvailable { get; set; }

        // 🔥 Navigation property to Bookings
        public virtual ICollection<Booking>? Bookings { get; set; }
    }

}
