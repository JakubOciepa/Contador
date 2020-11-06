using System;

using Contador.Core.Models;

namespace Contador.DAL.Models
{
    /// <summary>
    /// User info.
    /// </summary>
    public class UserDto : User
    {
        /// <summary>
        /// Date when the User has been created.
        /// </summary>
        public DateTime CreatedDate { get; }

        /// <summary>
        /// Date when the User has been edited last time.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Initializes instance of the <see cref="UserDto"/> class.
        /// </summary>
        public UserDto()
        {
        }
    }
}
