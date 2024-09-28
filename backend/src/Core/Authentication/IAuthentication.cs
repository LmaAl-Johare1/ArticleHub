using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Authentication
{
    public interface IAuthentication
    {
        string Generate(User user);
    }
}
