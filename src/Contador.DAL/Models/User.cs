namespace Contador.DAL.Models
{
    /// <summary>
    /// User info.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id of this user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of this user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email of this user.
        /// </summary>
        public string Email { get; set; }
    }
}
