﻿using AutoMapper;
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class FlightService(
        IFlightRepository flightRepository,
        IMapper mapper,
        IDistanceApiService distanceApiService,
        IAirportRepository airportRepository
            ) : IFlightService
    {
        private readonly IFlightRepository _flightRepository = flightRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IDistanceApiService _distanceApiService = distanceApiService;
        private readonly IAirportRepository _airportRepository = airportRepository;

        public async Task<List<FlightResponse>> GetAllFlights()
        {
            var flights = await _flightRepository.GetAll();
            var mappedFlights = _mapper.Map<List<FlightResponse>>(flights);
            return mappedFlights;
        }
        public async Task<Flight> CreateFlight(FlightCreationRequest flightCreationRequest)
        {
            Flight flight = _mapper.Map<Flight>(flightCreationRequest);
            flight.FlightCode = "123FLIGHTCODE";
            var airports = await _airportRepository.FindByIds(flight.DeparturePort, flight.ArrivalPort);
            Airport? originAirport = airports.Find((airport => airport.Id == flight.DeparturePort));
            Airport? arrivalAirport = airports.Find((airport => airport.Id == flight.ArrivalPort));
            if (originAirport == null || arrivalAirport == null) {
                throw new InvalidDataException("Could not find origin airport or arrival airport.");
            }
            (int distance, int duration) = await GetDistanceAndTravelTime(originAirport.Name, arrivalAirport.Name);
            flight.Kilometers = distance;
            flight.TravelTime = (int)duration;
            Flight createdFlight = await _flightRepository.Create(flight);
            return createdFlight;

        }

        public async Task<List<FlightResponse>> GetFlightsByDepartureDestinationAndDepartureDate(int departureAirportId, int destinationAirportId, DateOnly departureDate)
        {
            var flights = await _flightRepository.GetFlightsByDepartureDestinationAndDepartureDate(departureAirportId, destinationAirportId, departureDate);
            var mappedFlights = _mapper.Map<List<FlightResponse>>(flights);
            return mappedFlights;
        }

        private async Task<(int, int)> GetDistanceAndTravelTime(string origin, string destination)
        {
            double? distance = await _distanceApiService.GetDistanceData(origin, destination);
            if (distance == null)
            {
                throw new ArgumentNullException("Missing distance when trying to create a flight.");
            }
            double? kilometers = distance / 1000;
            int averageFlightSpeedInKilometersPerHour = 800;
            double? durationInMinutes = (kilometers / averageFlightSpeedInKilometersPerHour) * 60;
            return ((int)kilometers, (int)durationInMinutes);
        }
    }
}
