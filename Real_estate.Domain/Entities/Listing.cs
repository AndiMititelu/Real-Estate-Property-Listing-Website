﻿using Real_estate.Domain.Common;
using static Real_estate.Domain.Enums.Enums;
namespace Real_estate.Domain.Entities
{
    public class Listing : AuditableEntity
    {
        private Listing()
        {
            // EF Core needs this constructor
        }
        private Listing(string title, User user, Property property, string description) : this()
        {
            ListingId = Guid.NewGuid();
            Title = title;
            User = user;
            Property = property;
            Description = description;
        }

        public Guid ListingId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public User User { get; private set; }
        public Property Property { get; private set; }
        public string Description { get; private set; }

        public static Result<Listing> Create(string title, User user, Property property, string description)
        {
            if (user == null || property == null)
            {
                return Result<Listing>.Failure("User and Property are required.");
            }

            Role userRole = user.UserRole;

            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<Listing>.Failure("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<Listing>.Failure("Description is required.");
            }

            if (userRole != Role.User)
            {
                return Result<Listing>.Failure("Listing creator must be logged in");
            }
            return Result<Listing>.Success(new Listing(title, user, property, description));
        }
        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(newTitle));
            }

            // Additional business rules can be enforced here

            Title = newTitle;
        }

        // Method to update the description of the listing
        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentException("Description cannot be empty.", nameof(newDescription));
            }

            // Additional business rules can be enforced here

            Description = newDescription;
        }


    }
}
