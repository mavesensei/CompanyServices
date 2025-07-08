using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Routing;

public class RandomUserDTO
{
    public string Gender { get; set; }
    public Name name { get; set; }
    public Location location { get; set; }
    public string Email { get; set; }

    public class Name
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }

    }

    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }
    }
}