﻿using backend.Dtos;
using backend.Models;

namespace backend.Services
{
    public interface IFlightService
    {
        public Task<List<FlightResponse>> GetAllFlights();
        public Task<Flight> CreateFlight(FlightCreationRequest flightCreationRequest);
        Task<List<FlightResponse>> GetFlightsByDepartureDestinationAndDepartureDate(int departureAirportId, int destinationAirportId, DateOnly departureDate);
    }
}
