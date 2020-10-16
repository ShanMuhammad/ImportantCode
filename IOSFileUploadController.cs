public class FileUploadController : ApiController
    {
        public Task<HttpResponseMessage> PostUploadFile()
        {
            return UploadFileAsync().ContinueWith<HttpResponseMessage>((tsk) =>
                {
                    HttpResponseMessage response = null;

                    if (tsk.IsCompleted)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.Created);
                    }
                    else if (tsk.IsFaulted || tsk.IsCanceled)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }

                    return response;
                });
        }

        public Task UploadFileAsync()
        {
            return this.Request.Content.ReadAsStreamAsync().ContinueWith((tsk) => { SaveToFile(tsk.Result); },
                                                                         TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void SaveToFile(Stream requestStream)
        {
            using (FileStream targetStream = File.Create("C:\\file" + DateTime.Now.ToFileTime() + ".png"))
            {
                using (requestStream)
                {
                    requestStream.CopyTo(targetStream);
                }
            }
        }
    }
