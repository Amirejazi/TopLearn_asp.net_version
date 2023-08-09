﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        bool ActiveAccount(string activeCode);
        User GetUserByEmail(string email);
        User GetUserByUserName(string userName);
        User GetUserByActiveCode(string activeCode);
        void UpdateUser(User user);
        #region UserPanel

        InformationUserViewModel GetUserInformation(string userName); 
        SideBarUserPanelViewModel GetSideBarUserPanel(string userName);
        #endregion
    }
}
