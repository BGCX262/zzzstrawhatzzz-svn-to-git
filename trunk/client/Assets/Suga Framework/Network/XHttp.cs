using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System;
using System.Threading;

namespace Framework.Network
{
	
	public class XHttp 
	{
		public string RequestSync(string url, string userAgent, string postData, int timeout, bool keepAlive)
		{
			//Header
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Method = "POST";
			webRequest.ServicePoint.Expect100Continue = false;
			webRequest.KeepAlive = keepAlive;
			webRequest.Timeout = timeout;
			webRequest.ContentType = "application/json";
			webRequest.UserAgent = userAgent;
			webRequest.ContentLength = postData.Length;
			
			//Request
			using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
			{
				requestWriter.Write(postData);
				requestWriter.Flush();
				requestWriter.Close();
			}
			
			//Response
			using (HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse())
			using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
			{
				return reader.ReadToEnd();
			}
		}

        RegisteredWaitHandle handle = null; 

		public void RequestAsync(string url, string userAgent, string postData, int timeout, bool keepAlive, Action<string,string> requestCallBack)
		{
			//Header
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/json";
			webRequest.UserAgent = userAgent;
			webRequest.ContentLength = postData.Length;
			
			//Async Request
			webRequest.BeginGetRequestStream((req) => {
				
				try{
					//UnityEngine.Debug.Log("BeginGetRequestStream callback");
					HttpWebRequest request = (HttpWebRequest)req.AsyncState;
					
					StreamWriter requestWriter = new StreamWriter(request.EndGetRequestStream(req));
					requestWriter.Write(postData);
					requestWriter.Flush();
					requestWriter.Close();
					
					//Async Response
					IAsyncResult aResult = webRequest.BeginGetResponse( (res) => {

                        try
                        {
                            //UnityEngine.Debug.Log("BeginGetResponse callback");

                            HttpWebRequest request2 = (HttpWebRequest)res.AsyncState;
                            // End the operation
                            HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(res);

                            StreamReader reader = new StreamReader(response.GetResponseStream());

                            string result = reader.ReadToEnd();
                            requestCallBack(result,"OK");
                        }
                        catch (Exception e)
                        {
                            requestCallBack(null, e.Message);
                        }
						
						},request);

                    handle = ThreadPool.RegisterWaitForSingleObject(aResult.AsyncWaitHandle, TimeoutCallback, webRequest, timeout, true);
				}
				catch(Exception e)
                {
                    requestCallBack(null, e.Message);
				}
				
			},webRequest);
		}

        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
               UnityEngine.Debug.LogError("Timeout CMNR");
                HttpWebRequest req = (state as HttpWebRequest);
                if (req != null)
                {
                    req.Abort();
                }
            }
            else
            {
                handle.Unregister(null);
            }
        }
	}
}