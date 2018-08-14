using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EasyTranslate.Extentions
{
    public static class WebRequestExtentions
    {
        public static async Task<WebResponse> GetResponseAsync(
            this WebRequest request, CancellationToken token)
        {
            using (token.Register(request.Abort, false))
            {
                WebResponse response = await request.GetResponseAsync();
                token.ThrowIfCancellationRequested();
                return response;
            }
        }
    }
}