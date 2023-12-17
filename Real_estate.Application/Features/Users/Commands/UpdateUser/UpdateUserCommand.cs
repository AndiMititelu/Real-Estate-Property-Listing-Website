﻿using MediatR;
using static Real_estate.Domain.Enums.Enums;

namespace Real_estate.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UpdateUserCommandResponse>
    {
        public string UserId { get; set; }
        public string? Name { get; set; } 
        public string? Username { get; set; }
        public string? Email { get; set; } 
        public string? Password { get; set; } 
        public List<string?> UserRole { get; set; } 
        public string? PhoneNumber { get; set; } 
    }
}
