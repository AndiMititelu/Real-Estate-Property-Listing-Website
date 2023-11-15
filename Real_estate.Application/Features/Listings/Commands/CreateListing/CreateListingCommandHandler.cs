﻿using MediatR;
using Real_estate.Application.Persistence;
using Real_estate.Domain.Entities;

namespace Real_estate.Application.Features.Listings.Commands.CreateListing
{
    public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, CreateListingCommandResponse>
    {
        private readonly IListingRepository listingRepository;
        private readonly IUserRepository userRepository;
        private readonly IPropertyRepository propertyRepository;

        public CreateListingCommandHandler(
            IListingRepository listingRepository,
            IUserRepository userRepository,
            IPropertyRepository propertyRepository)
        {
            this.listingRepository = listingRepository;
            this.userRepository = userRepository;
            this.propertyRepository = propertyRepository;
        }

        public async Task<CreateListingCommandResponse> Handle(CreateListingCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateListingCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateListingCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var user = await userRepository.FindByIdAsync(request.UserId);
            var property = await propertyRepository.FindByIdAsync(request.PropertyId);

            var listingResult = Listing.Create(request.Title, user.Value, property.Value, request.Description);

            if (!listingResult.IsSuccess)
            {
                return new CreateListingCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { listingResult.Error }
                };
            }

            await listingRepository.AddAsync(listingResult.Value);

            return new CreateListingCommandResponse
            {
                Success = true,
                Listing = new CreateListingDto
                {
                    ListingId = listingResult.Value.ListingId,
                    Title = listingResult.Value.Title,
                    UserId = listingResult.Value.User.UserId,
                    PropertyId = listingResult.Value.Property.PropertyId,
                    Description = listingResult.Value.Description
                }
            };
        }
    }
}