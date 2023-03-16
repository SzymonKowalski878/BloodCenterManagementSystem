using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using BloodCenterManagmentSystem.GraphQL.Types;
using GraphQL.Types;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Linq;

namespace BloodCenterManagementSystem.Web.Queries
{
    public class UserQuery
    {
        private IUserRepository _userRepository;

        public UserQuery(IUserRepository repository)
        {
            _userRepository = repository;
        }

        [UsePaging(SchemaType = typeof(UserType))]
        [UseFiltering]
        public List<UserModel> GetAllUsers => _userRepository.GetAll().ToList();
    }
}
