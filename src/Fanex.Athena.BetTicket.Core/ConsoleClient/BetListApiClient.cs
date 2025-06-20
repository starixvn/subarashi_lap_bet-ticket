using Fanex.Athena.BetTicket.Core.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Text;
using Newtonsoft.Json;

namespace Fanex.Athena.BetTicket.Core.ConsoleClient
{
    public class BetListApiClient : BaseApiClient, IBetListApiClient
    {
        private readonly IContextService _contextService;
        private readonly HttpClient _httpClient;

        public BetListApiClient(HttpClient client, IContextService contextService) : base(client)
        {
            _contextService = contextService;
            _httpClient = client;
        }

        protected override string ApiName => "Bet List";

        protected override async Task PrepareHeaders(HttpRequestHeaders headers)
        {
            headers.Add("X-SiteDBServer", _contextService.SiteDBServer);
            headers.Authorization = new AuthenticationHeaderValue("Bearer", await _contextService.AccessToken);
        }

        public virtual async Task<string> PostString(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json,
                async (response) =>
                {
                    using (response)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                using (var textWriter = new StringWriter())
                                {
                                    const int chunkLength = 4096;
                                    var buffer = new char[chunkLength];
                                    int readChunkLength;
                                    do
                                    {
                                        readChunkLength = streamReader.ReadBlock(buffer, 0, chunkLength);
                                        textWriter.Write(buffer, 0, readChunkLength);
                                    } while (readChunkLength > 0);

                                    return textWriter.ToString();
                                }
                            }
                        }
                    }
                },
                timeOut: timeOut
            );
        }

        public async Task<Stream> PostStream(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json, async (response) =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Stream emptyStream = new MemoryStream();
                    return emptyStream;
                }
                return await response.Content.ReadAsStreamAsync();
            }
          , false, timeOut).ConfigureAwait(false);
        }

        public virtual async Task<ApiResponse<string>> PostStringWithStatus(string path, object value, string language, int timeOut = 100)
        {
            using (var cancelToken = new CancellationTokenSource(new TimeSpan(0, 0, timeOut)))
            {
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_httpClient.BaseAddress + path)))
                {
                    await PrepareHeaders(httpRequest.Headers);
                    const string LANGUAGEKEY = "Language";
                    const string JsonContentType = "application/json";

                    httpRequest.Headers.Add(LANGUAGEKEY, language);
                    HttpResponseMessage response = null;

                    try
                    {
                        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
                        using (var requestContent = new StringContent(JsonConvert.SerializeObject(value, new JsonSerializerSettings
                        {
                            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                            NullValueHandling = NullValueHandling.Ignore,
                        }), Encoding.UTF8, JsonContentType))
                        {
                            httpRequest.Content = requestContent;
                            response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead,
                                cancelToken.Token).ConfigureAwait(false);
                        }

                        int statusCode = response != null ? (int)response.StatusCode : 500;
                        string responseContent = null;

                        if (response != null && response.Content != null)
                        {
                            try
                            {
                                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                                {
                                    using (var streamReader = new StreamReader(stream))
                                    {
                                        using (var textWriter = new StringWriter())
                                        {
                                            const int chunkLength = 4096;
                                            var buffer = new char[chunkLength];
                                            int readChunkLength;
                                            do
                                            {
                                                readChunkLength = streamReader.ReadBlock(buffer, 0, chunkLength);
                                                textWriter.Write(buffer, 0, readChunkLength);
                                            } while (readChunkLength > 0);

                                            responseContent = textWriter.ToString();
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // In case of any error reading the content, return the status code with null content
                            }
                        }

                        return new ApiResponse<string>(responseContent, statusCode);
                    }
                    catch (Exception ex)
                    {
                        throw new ServiceException(ex.Message, ApiName, ex);
                    }
                    finally
                    {
                        response?.Dispose();
                    }
                }
            }
        }
    }

    public class ApiResponse<T>
    {
        public T Content { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse(T content, int statusCode)
        {
            Content = content;
            StatusCode = statusCode;
        }
    }
}