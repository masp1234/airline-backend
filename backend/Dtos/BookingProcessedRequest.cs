﻿using backend.Models;
namespace backend.Dtos
{
    public class BookingProcessedRequest
    {
        public string Email { get; set; }

        public int UserId { get; set; }

        public string ConfirmationNumber { get; set; }

        public List<TicketProcessedRequest> Tickets { get; set; }
    }

    public class TicketProcessedRequest
    {
        public PassengerCreationRequest Passenger { get; set; }

        public int FlightId { get; set; }

        public int FlightClassId { get; set; }

        public decimal FlightPrice { get; set; }

        public FlightClassName FlightClassName { get; set; }

        public string TicketNumber { get; set; }
    }
}
