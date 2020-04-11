using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class SaveData : MonoBehaviour
{

    public Button submitBtn;
    int indexer;

 
    void checkIndex()
    {
        indexer = PlayerPrefs.GetInt("dataIndex");

        indexer++;

        PlayerPrefs.SetInt("dataIndex", (indexer));

        PlayerPrefs.SetString("Sewerage" + indexer, StaticInfo.Sewerage);
        PlayerPrefs.SetString("Fever" + indexer, StaticInfo.Fever);

        PlayerPrefs.SetString("latitude" + indexer, StaticInfo.latitude.ToString());
        PlayerPrefs.SetString("longitude" + indexer, StaticInfo.longitude.ToString());
        PlayerPrefs.SetString("altitude" + indexer, StaticInfo.altitude.ToString());
        PlayerPrefs.SetString("horizontalAccuracy" + indexer, StaticInfo.horizontalAccuracy.ToString());
        PlayerPrefs.SetString("verticalAccuracy" + indexer, StaticInfo.verticalAccuracy.ToString());
        PlayerPrefs.SetString("timestamp" + indexer, StaticInfo.timestamp.ToString());




        //print(indexer);

    }
     void Start()
    {
      // PlayerPrefs.DeleteAll();
       // checkIndex();
    }

      
 
    public void sendData()
    {
        submitBtn.interactable= (false);
         checkIndex();
        StartCoroutine(SendDailyReportRounds());
    }



    IEnumerator SendDailyReportRounds()
    {

        //string desktopPath = Application.persistentDataPath;//.GetFolderPath (System.Environment.SpecialFolder.DesktopDirectory);
        string desktopPath = Application.persistentDataPath;//.GetFolderPath (System.Environment.SpecialFolder.DesktopDirectory);

        string pa =Application.persistentDataPath;


        string ruta = pa + "/"  + "NetstarCovid19.csv";

         if (File.Exists(ruta))
        {
            //File.Delete(ruta);
        }

 
        string datosCSV = "NetstarCovid19 Report" + System.Environment.NewLine;
        datosCSV += System.Environment.NewLine + (System.DateTime.Now.Day).ToString() + "."
            + (System.DateTime.Now.Month).ToString() + "."
            + (System.DateTime.Now.Year).ToString() +
            System.Environment.NewLine + System.Environment.NewLine;
        datosCSV += "All records,"+
            "Time, Sewerge result, Fever, latitude,  longitude,  altitude,  horizontalAccuracy,  verticalAccuracy," + System.Environment.NewLine;

        indexer = PlayerPrefs.GetInt("dataIndex");

        for (int i = 1; i <= indexer; i++)
        {
            string timestamp = PlayerPrefs.GetString("timestamp" + i);
            //print(timestamp);
           // DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.Parse(timestamp)).ToLocalTime();
            //string formattedDate = dt.ToString("dd-MM-yyyy");
 

             string sewerge = PlayerPrefs.GetString("Sewerage" + i);
            string fever = PlayerPrefs.GetString("Fever" + i);
            string latitude = PlayerPrefs.GetString("latitude" + i);
            string longitude = PlayerPrefs.GetString("longitude" + i);
            string altitude = PlayerPrefs.GetString("altitude" + i);
            string horizontalAccuracy = PlayerPrefs.GetString("horizontalAccuracy" + i);
            string verticalAccuracy = PlayerPrefs.GetString("verticalAccuracy" + i);

 

            datosCSV += i + ",";
            datosCSV += timestamp + ',';
            datosCSV += sewerge + ",";
            datosCSV += fever + ",";
            datosCSV += latitude + ",";
            datosCSV += longitude + ",";
            datosCSV += altitude + ",";
            datosCSV += horizontalAccuracy + ",";
            datosCSV += verticalAccuracy + ",";
             

            datosCSV += System.Environment.NewLine;

        }
        datosCSV += System.Environment.NewLine;


       

        //sr.Write(datosCSV);
        if (File.Exists(ruta))
        {
            File.Delete(ruta);
        }
        StreamWriter outStream = System.IO.File.CreateText(ruta);
        outStream.WriteLine(datosCSV);
        outStream.Close();

        //FileInfo fInfo = new FileInfo(ruta);
        //fInfo.IsReadOnly = false;

        //sr.Close();            

        yield return new WaitForSeconds(0.1f);

        //Application.OpenURL(ruta);



        SmtpClient SmtpServer = new SmtpClient("smtp.ipage.com");


        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("");
        // mail.To.Add("unfpa@onthelinesoftworks.com");
        /* mail.CC.Add("hasan.gharaibeh@onthelinesoftworks.com");*/

        mail.To.Add("davidn@netstaraus.com.au");
        mail.Bcc.Add("hassangh_1988@yahoo.com");
 


        mail.Subject = "Covid-19 report of " + (System.DateTime.Now.Day).ToString() + "-" +
            System.DateTime.Now.Month + "-" +
            System.DateTime.Now.Year;
        mail.Body = "Hi David, \nKindly note the covid-19 report." +
                        "\n\n\n\n\nThanks,\n David and Hasan Team";
        SmtpServer.Port = 587; //  
        SmtpServer.Credentials = new System.Net.NetworkCredential("", "") as ICredentialsByHost;
        SmtpServer.EnableSsl = true;

        mail.Attachments.Add(new Attachment(ruta));
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                return true;
            };
        SmtpServer.Send(mail);
        Debug.Log("mail Sent...");
        submitBtn.interactable = (true);



    }

     

    


}
