using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data;
using Newtonsoft.Json;
using ionicbackend.Models.ModelInfo;
using System.Web;
using System.IO;
using System.Xml;

namespace ionicbackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class SecurityController : ApiController
    {
        [Route("api/CheckLogin")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CheckLoginData(SearchString Info)
        {
            string strInfo = string.Empty;


            try
            {
                strInfo = GetLoginDetails.RetriveLoginDetails(Info);
                if (strInfo == "[{\"ResultData\":[]}]")
                {
                    strInfo = "[{ResultData:[{Status:0}]}]";
                    DataTable tester = (DataTable)JsonConvert.DeserializeObject(strInfo, (typeof(DataTable)));
                    return Request.CreateResponse(HttpStatusCode.OK, tester);
                }
                else
                {

                    DataTable tester = (DataTable)JsonConvert.DeserializeObject(strInfo, (typeof(DataTable)));
                    return Request.CreateResponse(HttpStatusCode.OK, tester);
                }
            }
            catch (Exception ex)
            {
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);

        }

        public bool SaveImage(string ImgStr, string ImgName)
        {

            String path =HttpContext.Current.Server.MapPath("~/Images/Profiles"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }
            //set the image path
            string imgPath = Path.Combine(path, ImgName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }

        [Route("api/AddUpdateUsers")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddUsers(SearchString Info)
        {
            string strInfo = string.Empty;
            List<CreateUserData> Forms = new List<CreateUserData>();

            try
            {
                XmlNodeList xmlnode;
                string xmlContent = Info.strSearchString;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                xmlnode = doc.GetElementsByTagName("Info");
                var random = new Random(System.DateTime.Now.Millisecond);
                int randomNumber = random.Next(1, 500000);
                string rndNumber = randomNumber.ToString();
                string imageData = xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
                if (imageData != "")
                {
                    var strimage = imageData.Substring(imageData.LastIndexOf(',') + 1);
                    SaveImage(strimage, rndNumber + ".jpg");
                }
                Forms = CreateUserData.CreateUpdateUsers(Info);
            }
            catch (Exception ex)
            {
            }
            HttpResponseMessage response;
            if (string.IsNullOrEmpty(strInfo))
                response = Request.CreateResponse(HttpStatusCode.OK, Forms);
            else
                response = Request.CreateResponse(strInfo);
            return response;

        }
    }
}
