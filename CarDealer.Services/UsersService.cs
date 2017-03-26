namespace CarDealer.Services
{
    using CarDealer.Models;
    using CarDealer.Models.BindingModels;
    using System.Linq;

    public class UsersService : Service
    {
        public void RegisterUser(RegisterUserBindingModel model)
        {
            User user = new User()
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public void LoginUser(LoginUserBindingModel model, string sessionSessionId)
        {
            if (!this.context.Logins.Any(login => login.SessionId == sessionSessionId))
            {
                this.context.Logins.Add(new Login() { SessionId = sessionSessionId });
                this.context.SaveChanges();
            }

            Login mylogin = this.context.Logins.FirstOrDefault(login => login.SessionId == sessionSessionId);
            mylogin.IsActive = true;
            User user =
                this.context.Users.FirstOrDefault(
                    u => u.Username == model.Username && u.Password == model.Password);

            mylogin.User = user;
            this.context.SaveChanges();
        }

        public bool UserExists(LoginUserBindingModel model)
        {
            if (this.context.Users.Any(user => user.Username == model.Username && user.Password == model.Password))
            {
                return true;
            }

            return false;
        }
    }
}
