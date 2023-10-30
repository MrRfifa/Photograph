using System.Text.Json.Serialization;

namespace Backend.Models.enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UsersRoles
    {
        admin,
        owner
    }
}