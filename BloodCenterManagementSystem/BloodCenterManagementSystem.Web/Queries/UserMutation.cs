using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagmentSystem.GraphQL.Types;

namespace BloodCenterManagementSystem.Web.Queries
{
    public class UserMutation
    {
        private readonly IUserRepository _userRepository;

        public UserMutation(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string CreateUser(CreateUserInput input)
        {
            try
            {
                _userRepository.Add(new Models.UserModel()
                {
                    Id = 0,
                    Email = input.Email,
                    Password = input.Password,
                    FirstName = input.FirstName,
                    Surname = input.Surname,
                    Role = input.Role,
                    EmailConfirmed = input.EmailConfirmed
                });

                _userRepository.SaveChanges();
            }
            catch
            {
                return "Failed to add user";
            }

            return "ok";
        }

        public string DeleteUser(DeleteUserInput input)
        {
            var user = _userRepository.GetById(input.Id);
            
            if(user == null)
            {
                return "User not found";
            }
            try
            {
                _userRepository.Delete(user);

                _userRepository.SaveChanges();
            }
            catch
            {
                return "Failed to delete user";
            }
            return "ok";
        }

    }
}
