using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BAL_Service_User_Mgmt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        UM_DataModel db = new UM_DataModel();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        #region User Management

        public List<VW_USERS> GetUserDetails(string NtLogin)
        {
            List<VW_USERS> objResult = db.Database.SqlQuery<VW_USERS>("select * from VW_Users where ntlogin = '" + NtLogin + "'").ToList();

            return objResult;
        }

        public List<VW_USERS> GetUserTeamDetails(string TeamName)
        {

            var userDtls = from user in db.VW_USERS
                           where user.TEAM_NAME == TeamName && user.ACTIVE == "True"
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }

        public List<VW_USERS> GetUserRegionDetails(string RegionName)
        {

            var userDtls = from user in db.VW_USERS
                           where user.SUPERREGION == RegionName && user.ACTIVE == "True"
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }


        public List<VW_USERS> GetInActiveUserTeamDetails(string TeamName)
        {
            var userDtls = from user in db.VW_USERS
                           where user.TEAM_NAME == TeamName && user.ACTIVE == "False"
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }

        public bool RegisterNewUser(TBL_USERS oUser)
        {
            bool result = false;

            try
            {
                db.Database.ExecuteSqlCommand("exec usp_CreateNewUser oUser.USERNAME, oUser.ROLE_ID, oUser.TEAM_ID, oUser.NTLOGIN, oUser.SUPERREGION, oUser.COUNTRY, oUser.EMAIL, oUser.FULLNAME, oUser.PROFILE_PIC", null);
                //db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                ServiceException exception = new ServiceException();

                exception.Result = false;
                exception.ErrorMessage = "Error Occured";
                exception.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exception, ex.ToString());
            }

            return result;
        }

        public bool UpdateUserDetails(TBL_USERS oUser)
        {
            bool result = false;

            try
            {

                db.Database.ExecuteSqlCommand("exec usp_UpdateUser oUser.USER_ID, oUser.NTLOGIN, oUser.USERNAME, oUser.TEAM_ID, oUser.ROLE_ID, oUser.EMAIL, oUser.FULLNAME, oUser.PROFILE_PIC", null);

                //db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<VW_USERS> GetUsersByRegion(string Region)
        {
            List<VW_USERS> oresult = db.VW_USERS.Where(p => p.SUPERREGION == Region).Distinct().ToList();

            return oresult;
        }

        public string getEmailIDByName(string name)
        {
            List<VW_USERS> oresult = db.VW_USERS.Where(p => p.FULLNAME == name).Distinct().ToList();
            string Manageremail = oresult[0].EMAIL.ToString();
            return Manageremail;
        }

        public List<Tbl_Unmapped_Users> GetUnMappedUsersByRegion(string Region)
        {
            List<Tbl_Unmapped_Users> oresult = db.Database.SqlQuery<Tbl_Unmapped_Users>("exec Usp_Unmapped_Users_Summary '" + Region + "'").ToList();

            return oresult;
        }
        public List<Tbl_Unmapped_Orders_By_Region_Function> GetUnMappedUsersByRegionFunction(string Region, string BusinessFunction)
        {
            List<Tbl_Unmapped_Orders_By_Region_Function> oresult = new List<Tbl_Unmapped_Orders_By_Region_Function>();
            oresult = db.Database.SqlQuery<Tbl_Unmapped_Orders_By_Region_Function>("exec Usp_Unmapped_User_DetailsByFunction '" + Region + "','" + BusinessFunction + "','").ToList();

            return oresult;
        }

        public List<Tbl_Country_Sorg_Orig> GetCountryByRegion(string Region)
        {
            List<Tbl_Country_Sorg_Orig> oresult = db.Tbl_Country_Sorg_Orig.Where(p => p.Region == Region).Distinct().ToList();

            return oresult;
        }

        public bool ActiveDeactiveUser(TBL_USERS oUser)
        {
            bool result = false;

            try
            {

                db.Database.ExecuteSqlCommand("exec usp_ActiveDeactiveUser '" + oUser.USER_ID + "','");

                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<TBL_TEAM_STRUCTURE> GetManagerByTeamID(int TeamID)
        {

            var userDtls = db.TBL_TEAM_STRUCTURE.Where(p => p.TEAM_ID == TeamID).ToList();
            List<TBL_TEAM_STRUCTURE> objResult = userDtls.ToList();

            return objResult;
        }

        public List<TBL_ROLE> GetRoleMaster()
        {
            return db.TBL_ROLE.ToList();
        }

        public List<TBL_TEAM_STRUCTURE> GetTeamMaster(string Region)
        {
            if (Region == "WW" || Region == "")
            {
                return db.TBL_TEAM_STRUCTURE.ToList();
            }
            else
            {
                List<TBL_TEAM_STRUCTURE> lst_results = new List<TBL_TEAM_STRUCTURE>();

                var unique_Teams = (from t1 in db.VW_USERS
                                    where t1.SUPERREGION == Region
                                    select t1.TEAM_NAME).Distinct();
                foreach (string Team in unique_Teams)
                {
                    TBL_TEAM_STRUCTURE tmp_Team = (from t2 in db.TBL_TEAM_STRUCTURE where t2.TEAM_NAME == Team select t2).FirstOrDefault();
                    lst_results.Add(tmp_Team);
                }

                return lst_results;
            }
        }


        public List<VW_USERS> GetNTLoginBySAPName(string SAPName)
        {
            List<VW_USERS> oresult = db.VW_USERS.Where(p => p.SAP_User_Name == SAPName).Distinct().ToList();

            return oresult;
        }

        public bool UpdateUserInSApandBBZTRD(string OrderOwner, string CreatedBy)
        {
            bool result = false;

            try
            {
                db.Database.ExecuteSqlCommand("exec usp_UpdateUserInSAPZTRD '" + OrderOwner + "','" + CreatedBy);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        #endregion
    }
}
