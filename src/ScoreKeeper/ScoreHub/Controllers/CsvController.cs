using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScoreHub.Controllers
{
    public class CsvController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            string headers = "BlueTeamName,WhiteTeamName,BlueTeamGoals,WhiteTeamGoals,Rounds,CurrentRound,TimeMin,TimeSec,Extra";
            string data = $"{GameHub.Instance.CurrentGame?.BlueTeamShortName},{GameHub.Instance.CurrentGame?.WhiteTeamShortName},{GameHub.Instance.CurrentGame?.BlueTeamGoals},{GameHub.Instance.CurrentGame?.WhiteTeamGoals},{GameHub.Instance.CurrentGame?.Rounds},{GameHub.Instance.CurrentGame?.CurrentRound},{GameHub.Instance.CurrentGame?.TimeLeft.Minutes:00},{GameHub.Instance.CurrentGame?.TimeLeft.Seconds:00},{GameHub.Instance.CurrentGame?.Extra}";
            
            result.Content = new StringContent($"{headers}\r\n{data}");
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "score.csv" };
            return result;
        }
    }
}