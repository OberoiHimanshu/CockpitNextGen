using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.Protocols;

namespace Cockpit_NextGenMVC.Models
{
    public class LDAP_Model 
    {
        private LdapConnection con;
        Dictionary<String, String> objDicLdap = new Dictionary<string, string>();

        public void AgilentLDAP()
        {
            try
            {
                con = new LdapConnection(new LdapDirectoryIdentifier("wcosldp5-new.cos.agilent.com", true, false));
                //con = new LdapConnection(new LdapDirectoryIdentifier("ldap.it.agilent.com", true, false));            
                con.Credential = new System.Net.NetworkCredential("cn=LSCA_ACCESS_AUDIT, ou=Applications, o=agilent.com", "apriWA&S");
                con.AuthType = AuthType.Basic;
                con.Bind();
            }
            catch (Exception ex)
            {
                con.Dispose();
                con = new LdapConnection(new LdapDirectoryIdentifier("wbbnldp1-new.germany.agilent.com", true, false));
                con.Credential = new System.Net.NetworkCredential("cn=LSCA_ACCESS_AUDIT, ou=Applications, o=agilent.com", "apriWA&S");
                con.AuthType = AuthType.Basic;
                con.Bind();
                //throw ex;
            }
        }

        public object FetchUserDataByNtlogin(String EmployeeNtLogin, Boolean IsAgilentPermanentEmployee)
        {
            AgilentLDAP();
            String srDN = string.Empty;
            String srFilter = string.Empty;
            if (IsAgilentPermanentEmployee)
            {
                srDN = "o=agilent.com";
                srFilter = "ntuserdomainid=agilent:" + EmployeeNtLogin;
            }
            else
            {
                srDN = "ou=Non-HP, o=agilent.com";
                srFilter = "ntuserdomainid=" + EmployeeNtLogin;
            }
            if (EmployeeNtLogin != null)
            {
                try
                {
                    SearchRequest sr = new SearchRequest();
                    sr.DistinguishedName = (srDN);
                    sr.Filter = (srFilter);
                    SearchResponse _SearchResponse;
                    _SearchResponse = (SearchResponse)con.SendRequest(sr);
                    SearchResultAttributeCollection serAtrib = _SearchResponse.Entries[0].Attributes;
                    //if (serAtrib.Count > 0)
                    //{
                    //    HttpContext.Current.Session["UserNTLogin"] = serAtrib[""].ToString();
                    //}

                    String Results = string.Empty;
                    object[] objArray = new object[100];
                    serAtrib.AttributeNames.CopyTo(objArray, 0);


                    foreach (String Attrib in objArray)
                    {
                        if (Attrib != null)
                        {


                            if (Attrib == "cn")
                            {
                                String[] vals = (String[])serAtrib[Attrib].GetValues(typeof(System.String));
                                if (vals.Length > 0)
                                    objDicLdap.Add("FullName",vals[0].ToString());
                            }

                            if (Attrib == "aghostcountryname")
                            {
                                String[] vals = (String[])serAtrib[Attrib].GetValues(typeof(System.String));
                                if (vals.Length > 0)
                                    objDicLdap.Add("CountryName", vals[0].ToString());
                            }


                            if (Attrib == "mail")
                            {
                                String[] vals = (String[])serAtrib[Attrib].GetValues(typeof(System.String));
                                if (vals.Length > 0)
                                    objDicLdap.Add("Email", vals[0].ToString());
                               
                            }
                            if (Attrib == "agsupvemployeenumber")
                            {
                                String[] vals = (String[])serAtrib[Attrib].GetValues(typeof(System.String));
                                if (vals.Length > 0)
                                {
                                    string ManagerID = vals[0].ToString();
                                    FetchUserData(ManagerID, true);
                                }
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Exception ex = new Exception("Employee ID cannot be null");
                throw ex;
            }
           
            return objDicLdap;
        }

        public object FetchUserData(String EmployeeID, Boolean IsAgilentPermanentEmployee)
        {
            AgilentLDAP();
            String srDN;
            String srFilter;
            if (IsAgilentPermanentEmployee)
            {
                srDN = "ou=employees, o=agilent.com";
                srFilter = "employeenumber=" + EmployeeID;
            }
            else
            {
                srDN = "ou=Non-HP, o=agilent.com";
                srFilter = "nonhpuniqueid=" + EmployeeID;
            }
            
            if (EmployeeID != null)
            {
                try
                {
                    SearchRequest sr = new SearchRequest();
                    sr.DistinguishedName = (srDN);
                    sr.Filter = (srFilter);
                    SearchResponse _SearchResponse;
                    _SearchResponse = (SearchResponse)con.SendRequest(sr);
                    SearchResultAttributeCollection serAtrib = _SearchResponse.Entries[0].Attributes;
                    String Results = string.Empty;
                    object[] objArray = new object[100];
                    serAtrib.AttributeNames.CopyTo(objArray, 0);
                    

                    foreach (String Attrib in objArray)
                    {
                        if (Attrib != null)
                        {
                            if (Attrib == "cn")
                            {
                                String[] vals = (String[])serAtrib[Attrib].GetValues(typeof(System.String));
                                if (vals.Length > 0)
                                        objDicLdap.Add("ManagerName", vals[0].ToString());
                                    
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Exception ex = new Exception("Employee ID cannot be null");
                throw ex;
            }
            
            return objDicLdap;
        }

        ~ LDAP_Model()
        {
            con.Dispose();
        }
    }
}