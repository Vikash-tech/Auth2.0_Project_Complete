
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace IMS.Service
{
    public class SCEmail
    {
        // MAil for SC Ims   // Iteam Creation MAIL
        public static int fnSCEmail(string sMsg, string sSub, string sFromEmail, int iEmailTyp, int iSiteId)
        {
            string sToEmail = string.Empty;
            int iLoop;
            if (iEmailTyp > 0 && iSiteId > 0)
            {
                DataTable dtToDtl = getEmail(iEmailTyp, iSiteId);
                sToEmail = "";
                for (iLoop = 0; iLoop < dtToDtl.Rows.Count; iLoop++)
                {
                    if (Convert.ToInt32(sToEmail.Length.ToString()) <= 0 || sToEmail == string.Empty)
                    {
                        sToEmail = dtToDtl.Rows[iLoop]["EmailId"].ToString();
                    }
                    else
                    {
                        sToEmail = sToEmail + "," + dtToDtl.Rows[iLoop]["EmailId"].ToString();
                    }

                }
            }
            // Email Code 
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.EnableSsl = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ims_system@balajeegroup.com", "@Bicpl123@");
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp3.netcore.co.in";

                mail.From = new MailAddress("ims_system@balajeegroup.com");

                //mail.To.Add("atmishra243@gmail.com");
                mail.To.Add(sFromEmail); //Replace  the txtemail.Text with the user email id by fetching it from user
                mail.Subject = sSub;
                mail.IsBodyHtml = true;
                mail.Body = sMsg;
                SmtpServer.Send(mail);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static DataTable getEmail(int iEmailTyp, int iSiteId)
        {
            string conStr = "Server=BICPLSRV01;Initial Catalog=IMS_R;Persist Security Info=True;User ID=ims;Password=Im_4_bicpl;trustServerCertificate=true";
            using (SqlConnection conEmail = new SqlConnection(conStr))
            {
                string sqlTxt = "Select EmailId, PersonName From tSiteEmail With (NoLock) Where SiteId=" + iSiteId + " and EmailType = " + iEmailTyp + " and ActStatus = 1;";
                using (SqlDataAdapter sdaEmail = new SqlDataAdapter(sqlTxt, conEmail))
                {
                    DataTable dtEmail = new DataTable();
                    sdaEmail.SelectCommand.CommandType = CommandType.Text;
                    sdaEmail.Fill(dtEmail);
                    conEmail.Close();
                    return dtEmail;
                }
            }
        }

        // Iteam Creation APPROVAL Mail
        public int fnMISEmail(string sMsg, string sSub, string sFromEmail, int iEmailTyp, int iSiteId)
        {
            string sToEmail = string.Empty;
            int iLoop;
            DataTable dtToDtl = getEmail(iEmailTyp, iSiteId);
            sToEmail = "";
            for (iLoop = 0; iLoop < dtToDtl.Rows.Count; iLoop++)
            {
                if (Convert.ToInt32(sToEmail.Length.ToString()) <= 0 || sToEmail == string.Empty)
                {
                    sToEmail = dtToDtl.Rows[iLoop]["EmailId"].ToString();
                }
                else
                {
                    sToEmail = sToEmail + "," + dtToDtl.Rows[iLoop]["EmailId"].ToString();
                }

            }
            // Email Code 
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.EnableSsl = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ims_system@balajeegroup.com", "@Bicpl123@");
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp3.netcore.co.in";
                mail.From = new MailAddress("ims_system@balajeegroup.com");
                mail.To.Add("vgautam@balajeegroup.com");
                mail.CC.Add(""); //Replace  the txtemail.Text with the user email id by fetching it from user
                mail.Subject = sSub;
                mail.IsBodyHtml = true;
                mail.Body = sMsg;
                SmtpServer.Send(mail);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // SC LOI Preparation Approval Mail -- To Email ID is HO SC Team, email type=8, CC Email id is - Site Accounts, Email Type = 4
        public int fnSCLOIApprvEmail(string sMsg, string sSub, string sFromEmail, string iEmailTyp, string iCCEmailTyp, int iSiteId)
        {
            string sToEmail = string.Empty;
            string sCCEmail = string.Empty;
            int iLoop, jLoop;
            DataTable dtToDtl = getMultiMailId(iEmailTyp, iSiteId);
            sToEmail = "";
            for (iLoop = 0; iLoop < dtToDtl.Rows.Count; iLoop++)
            {
                if (Convert.ToInt32(sToEmail.Length.ToString()) <= 0 || sToEmail == string.Empty)
                {
                    sToEmail = dtToDtl.Rows[iLoop]["EmailId"].ToString();
                }
                else
                {
                    sToEmail = sToEmail + "," + dtToDtl.Rows[iLoop]["EmailId"].ToString();
                }

            }
            DataTable dtCCDtl = getMultiMailId(iCCEmailTyp, iSiteId);
            sCCEmail = "";
            for (iLoop = 0; iLoop < dtCCDtl.Rows.Count; iLoop++)
            {
                if (Convert.ToInt32(sCCEmail.Length.ToString()) <= 0 || sCCEmail == string.Empty)
                {
                    sCCEmail = dtCCDtl.Rows[iLoop]["EmailId"].ToString();
                }
                else
                {
                    sCCEmail = sCCEmail + "," + dtCCDtl.Rows[iLoop]["EmailId"].ToString();
                }

            }

            // Email Code 
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.EnableSsl = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ims_system@balajeegroup.com", "@Bicpl123@");
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp3.netcore.co.in";
                mail.From = new MailAddress("ims_system@balajeegroup.com");
                // mail.To.Add(sToEmail);
                mail.To.Add("vgautam@balajeegroup.com");
                mail.Subject = sSub;
                mail.IsBodyHtml = true;
                mail.Body = sMsg;
                SmtpServer.Send(mail);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // for 2 Email
        public DataTable getMultiMailId(string iEmailTyp, int iSiteId)
        {
            string conStr = "Server=BICPLSRV01;Initial Catalog=IMS_R;Persist Security Info=True;User ID=ims;Password=Im_4_bicpl;trustServerCertificate=true";

            using (SqlConnection conEmail = new SqlConnection(conStr))
            {
                string sqlTxt = "Select EmailId, PersonName From tSiteEmail With (NoLock) Where SiteId=" + iSiteId + " and EmailType in ( " + iEmailTyp + ")  and ActStatus = 1 ;";
                using (SqlDataAdapter sdaEmail = new SqlDataAdapter(sqlTxt, conEmail))
                {
                    DataTable dtEmail = new DataTable();
                    sdaEmail.SelectCommand.CommandType = CommandType.Text;
                    sdaEmail.Fill(dtEmail);
                    conEmail.Close();
                    return dtEmail;
                }
            }
        }
      
    }
}
