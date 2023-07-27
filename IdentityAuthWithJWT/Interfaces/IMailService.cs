namespace IdentityAuthWithJWT.Interfaces
{
	public interface IMailService
	{
		public Task<string> SendWelcomeEmailAsync(string toEmail, string userName);
	}
}
