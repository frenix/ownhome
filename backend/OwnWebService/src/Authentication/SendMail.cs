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
		
		public static int Send(string fullname, string email, string uuid)
		{
			try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                               
                mail.From = new MailAddress("proprtyfindr@gmail.com");
                //mail.To.Add("efren.duranjr@gmail.com");
                //mail.To.Add("proprtyfindr@gmail.com");
                mail.To.Add(email);
                
                mail.Subject = "[Ownhome.com] Email Confirmation ";
                mail.Body += " <html>";
                mail.Body += "<body>";
                mail.Body += "<table>";
                
				//http://localhost:8000/app/#/signup-success/authenticationkeyhere

	
//                mail.Body += "<tr>";
//                mail.Body += "<td> <h1><img src=" + D:\ownhome\backend\OwnWebService\src\Images\logo-text.png + "></h1></td>";
//                mail.Body += "</tr>";
                
				mail.Body += "<tr>";
                mail.Body += "<td>Hi " +  fullname  + ",</td>";
                mail.Body += "</tr>";
				
                mail.Body += "<tr><td></td></tr>";
                 
                mail.Body += "<tr>";
                mail.Body += "<td>You just signed up for Ownhome.com. Please follow this link to confirm that this is your e-mail address. </td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>http://localhost:8000/app/#/signup-success/" + uuid + " </td>";
                mail.Body += "</tr>";
				
                mail.Body += "<tr><td></td></tr>";
                
                mail.Body += "<tr>";
                mail.Body += "<td>Thanks, </td>";
                mail.Body += "</tr>";
                
                mail.Body += "<tr><td></td></tr>";
                
                mail.Body += "<tr>";
                mail.Body += "<td>The Ownhome Team </td>";
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
