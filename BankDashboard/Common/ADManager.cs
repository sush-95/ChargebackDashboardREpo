using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using BankDashboard.LogFile;

namespace BankDashboard.Common
{
    public class ADManager
    {
        string Admin = string.Empty, Checker = string.Empty, Maker = string.Empty, ParameterManager = string.Empty,ApplicationUser=string.Empty;
        public ADManager()
        {
            Admin = ConfigurationManager.AppSettings["Admin"].ToString();
            Checker = ConfigurationManager.AppSettings["Checker"].ToString();
            Maker = ConfigurationManager.AppSettings["Maker"].ToString();
            ParameterManager = ConfigurationManager.AppSettings["Parameter"].ToString();
            ApplicationUser = ConfigurationManager.AppSettings["ApplicationUser"].ToString();
        }
        public bool ChcekLogin(string username, string pwd, ref string Groupname)
        {
            string domain = ConfigurationManager.AppSettings["Domain"].ToString();
            //WriteToLogFile.writeMessage("Domain = "+domain.ToString());
            bool authentic = false;
            try
            {
                //WriteToLogFile.writeMessage("Verifying User Name and Password UserName = "+username.ToString());

                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain,username, pwd);
                object nativeObject = entry.NativeObject;
                Groupname = CheckGroup(domain, entry.Username);
                authentic = true;
                //WriteToLogFile.writeMessage(string.IsNullOrEmpty(Groupname)?"Group Name is Null": Groupname.ToString());
                //WriteToLogFile.writeMessage("Verifying Completed - User Verfication Successfull");
            }
            catch (DirectoryServicesCOMException e) 
            {
                //WriteToLogFile.writeMessage("Verifying Completed - User Verfication UnSuccessfull with error" + e.Message.ToString() + "Returned False");
            }
            return authentic;

        }

        public string CheckGroup(string domain, string username)
        {
            string Resultgroup = string.Empty, groups = string.Empty;
            groups = GetGroups(username, domain);
            if (groups.Contains(Admin.Trim()))
            {
                Resultgroup = Constants.UserGroups.Admin;
            }
            else if (groups.Contains(Checker.Trim()))
            {
                Resultgroup = Constants.UserGroups.Checker;
            }
            else if (groups.Contains(Maker.Trim()))
            {
                Resultgroup = Constants.UserGroups.Maker;
            }
            else if (groups.Contains(ParameterManager.Trim()))
            {
                Resultgroup = Constants.UserGroups.ParameterManager;
            }
            else if (groups.Contains(ApplicationUser.Trim()))
            {
                Resultgroup = Constants.UserGroups.ApplicationUserManagement;
            }
            else
            {
                Resultgroup = string.Empty;
            }
            return Resultgroup;

        }

        public string GetGroups(string userName, string domain)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain, domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userName);
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();
                foreach (Principal p in groups)
                {
                    if (p is GroupPrincipal)
                    {
                        result.Add((GroupPrincipal)p);
                    }
                }
            }
            string GroupNames = string.Empty;
            foreach (var item in result)
            {
                GroupNames += item.Name + Environment.NewLine;
            }
            return GroupNames;
        }

    }
}