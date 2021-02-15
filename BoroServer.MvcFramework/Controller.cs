using BoroServer.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BoroServer.MvcFramework
{
    public abstract class Controller
    {
        public HttpResponse View(string viewPath)
        {
            var body = File.ReadAllBytes(viewPath);
            var response = new HttpResponse(body);

            return response;
        }
    }
}
