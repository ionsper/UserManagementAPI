namespace UserManagementAPI.Models
{
    /// <summary>
    /// Represents a user in the UserManagementAPI.
    /// Contains basic user properties for CRUD operations.
    /// </summary>
    public class User
    {
        public string Name { get; set; } = string.Empty; // Required
        public int? Age { get; set; } // Optional
    }
}