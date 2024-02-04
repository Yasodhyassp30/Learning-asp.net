using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace AllInOne.Models{
    public class UserModel{
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20,MinimumLength =8,ErrorMessage = "Password must be at least 8 characters long")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Level is required")]
        public required string Level { get; set; }

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Token { get; set; }
    }
}