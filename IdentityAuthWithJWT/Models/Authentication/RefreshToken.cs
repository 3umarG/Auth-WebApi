﻿using Microsoft.EntityFrameworkCore;

namespace IdentityAuthWithJWT.Models.Authentication
{
	public class RefreshToken
	{
        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpiresOn;

        public DateTime CreatedOn { get; set; }

        public DateTime? RevokedOn { get; set; }

        public bool IsActive => !IsExpired && RevokedOn is null;

    }
}
