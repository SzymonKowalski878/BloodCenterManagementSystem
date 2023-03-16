using BloodCenterManagementSystem.Models;
using HotChocolate.Types;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagmentSystem.GraphQL.Types
{
    public class UserType: ObjectType<UserModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<IdType>();
            descriptor.Field(x => x.FirstName).Type<StringType>();
            descriptor.Field(x => x.Surname).Type<StringType>();
            descriptor.Field(x => x.Role).Type<StringType>();
            descriptor.Field(x => x.EmailConfirmed).Type<BooleanType>();
        }

        public UserType()
        {
            
        }
    }
}
