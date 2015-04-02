/*
 * Created by SharpDevelop.
 * User: Frenix
 * Date: 3/26/2015
 * Time: 10:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net.Mail;

namespace OHWebService.Authentication
{
	/// <summary>
	/// Description of SendMail.
	/// </summary>
	public class SendMail
	{
		public SendMail()
		{
		}
		
		public static int Send()
		{
			try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                               
                mail.From = new MailAddress("proprtyfindr@gmail.com");
                //mail.To.Add("efren.duranjr@gmail.com");
                mail.To.Add("proprtyfindr@gmail.com");
                mail.Subject = "Password Recovery ";
                mail.Body += " <html>";
                mail.Body += "<body>";
                mail.Body += "<table>";

                mail.Body += "<tr>";
                mail.Body += "<td>User Name : </td><td> HAi </td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>Password : </td><td>aaaaaaaaaa</td>";
                mail.Body += "</tr>";

                mail.Body += "</table>";
                mail.Body += "</body>";
                mail.Body += "</html>";

                mail.IsBodyHtml = true;

                ////System.Net.Mail.Attachment attachment;
                ////attachment = new System.Net.Mail.Attachment(@"D:\bkup\krishna.mdb");
                ////mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new      
                System.Net.NetworkCredential("proprtyfindr@gmail.com", "01tryst02");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return 1;              

            }
            catch (Exception err)
            {
            	Console.WriteLine(err.ToString());
            	return 0;
           
            }
		}
	}
}
