using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BAL_Service_User_Mgmt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        #region User Management Module classes

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUserDetails(string NtLogin);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUserTeamDetails(string TeamName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUserRegionDetails(string RegionName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetInActiveUserTeamDetails(string TeamName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool RegisterNewUser(TBL_USERS oUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUserDetails(TBL_USERS oUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUsersByRegion(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Unmapped_Users> GetUnMappedUsersByRegion(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ActiveDeactiveUser(TBL_USERS oUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Country_Sorg_Orig> GetCountryByRegion(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_TEAM_STRUCTURE> GetManagerByTeamID(int TeamID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_ROLE> GetRoleMaster();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_TEAM_STRUCTURE> GetTeamMaster(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Unmapped_Orders_By_Region_Function> GetUnMappedUsersByRegionFunction(string Region, string BusinessFunction);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetNTLoginBySAPName(string SAPName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUserInSApandBBZTRD(string OrderOwner, string CreatedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string getEmailIDByName(string Name);

        #endregion
    }

    [DataContract]
    public class ServiceException
    {
        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ErrorDetails { get; set; }
    }
}
