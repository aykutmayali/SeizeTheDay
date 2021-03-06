﻿using SeizeTheDay.Core.DataAccess.Abstract;
using Xgteamc1XgTeamModel;

namespace SeizeTheDay.DataAccess.Dapper.Abstract.MySQL
{
    public interface IUserRoleDataMapper : IDataMapper<UserRole>
    {
        UserRole FindByRoleId(string id);
    }
}
