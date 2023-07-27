namespace IdentityAuthWithJWT.Interfaces
{
	public interface IMailService
	{
		public Task<bool> SendWelcomeEmailAsync(string toEmail, string userName);
	}
}
