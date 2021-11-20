using System;
using System.Collections.Generic;
using System.Text;
using Travel.Application.Dtos.User;

namespace Travel.Application.Common.Interfaces
{
    public interface IUserService
    {
        AuthenticationResponse Authenticate(AuthenticationRequest request);
        Domain.Entities.User GetById(int id);
    }
}
