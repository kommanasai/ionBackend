using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ionicbackend.Global;

namespace ionicbackend.Models.ModelInfo
{

    public class ModelInfo
    {
    }
    public class SearchString
    {
        public string strSearchString { get; set; }

    }

    public class ReturnJsonResponse
    {
        public string result { get; set; }

        public ReturnJsonResponse(string result)
        {
            this.result = result;
        }

        public static List<ReturnJsonResponse> CreateResponse(String result)
        {
            List<ReturnJsonResponse> MInfo = new List<ReturnJsonResponse>();
            try
            {
                MInfo.Add(new ReturnJsonResponse(result));
            }
            catch (Exception ex)
            {

            }
            return MInfo;
        }
    }
    public class GetLoginDetails
    {
        public static string RetriveLoginDetails(SearchString Info)
        {
            string strResult = string.Empty;
            try
            {
                using (var Context = new backendDataContext())
                {
                    strResult = Context.USP_TL_CheckLogin(Info.strSearchString).Select(s => s.JSON_F52E2B61_18A1_11d1_B105_00805F49916B).Aggregate((p, r) => p + r);
                }
            }
            catch (Exception ex)
            {
                strResult = "";
            }

            return strResult;

        }
    }

    

    

    public class CreateUserData
    {
        public string Id { get; set; }
        public string ErrorId { get; set; }

        public CreateUserData(string strId, string strError)
        {
            this.Id = strId;
            this.ErrorId = strError;
        }
        public static List<CreateUserData> CreateUpdateUsers(SearchString RInfo)
        {
            List<CreateUserData> Info = new List<CreateUserData>();
            try
            {
                using (var Context = new backendDataContext())
                {
                    int? iErrno = 0;
                    int? id = 0;

                    Context.USP_AddUser(RInfo.strSearchString, ref id, ref iErrno);

                    int ireqId = (int)id;
                    int ieNo = (int)iErrno;
                    Info.Add(new CreateUserData(Convert.ToString(ireqId), Convert.ToString(ieNo)));

                }
            }
            catch (Exception ex)
            {

            }
            return Info;
        }
    }
}