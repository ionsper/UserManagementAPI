namespace UserManagementAPI.Models
{
    /// <summary>
    /// Represents a user in the UserManagementAPI.
    /// Contains basic user properties for CRUD operations.
    /// </summary>
    public class User
    {
        required public string Name { get; set; } = string.Empty;
        public int? Age { get; set; }
    }
}