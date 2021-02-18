﻿namespace BoroServer.MvcFramework
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BoroServer.HTTP;

    public static class Host
    {
        public static async Task RunAsync(IMvcApplication application, int port = 80)
        {
            //IServiceCollection serviceCollection = new ServiceCollection();
            ICollection<Route> routeTable = new HashSet<Route>();

            //application.ConfigureServices(serviceCollection);
            application.Configure(routeTable);

            //AutoRegisterRoutes(routeTable, application, serviceCollection);

            IHttpServer server = new HttpServer(routeTable);
            await server.StartAsync(port);
        }
        private static void AutoRegisterRoutes(ICollection<Route> routeTable, IMvcApplication application, IServiceCollection serviceCollection)
        {
            var controllerTypes = application.GetType().Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));
            foreach (var controllerType in controllerTypes)
            {
                var methods = controllerType.GetMethods()
                    .Where(x => x.IsPublic && !x.IsStatic && x.DeclaringType == controllerType
                    && !x.IsAbstract && !x.IsConstructor && !x.IsSpecialName);
                foreach (var method in methods)
                {
                    var url = "/" + controllerType.Name.Replace("Controller", string.Empty)
                        + "/" + method.Name;

                    var attribute = method.GetCustomAttributes(false)
                        .Where(x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .FirstOrDefault() as BaseHttpAttribute;

                    var httpMethod = HttpMethod.GET;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (!string.IsNullOrEmpty(attribute?.Url))
                    {
                        url = attribute.Url;
                    }

                    routeTable.Add(new Route(url, httpMethod, request => ExecuteAction(request, controllerType, method, serviceCollection)));
                }
            }
        }

        private static HttpResponse ExecuteAction(HttpRequest request, Type controllerType, MethodInfo action, IServiceCollection serviceCollection)
        {
            var instance = serviceCollection.CreateInstance(controllerType) as Controller;
            instance.Request = request;
            var arguments = new List<object>();
            var parameters = action.GetParameters();
            foreach (var parameter in parameters)
            {
                var httpParamerValue = GetParameterFromRequest(request, parameter.Name);
                var parameterValue = Convert.ChangeType(httpParamerValue, parameter.ParameterType);
                if (parameterValue == null &&
                    parameter.ParameterType != typeof(string)
                    && parameter.ParameterType != typeof(int?))
                {
                    // complex type
                    parameterValue = Activator.CreateInstance(parameter.ParameterType);
                    var properties = parameter.ParameterType.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyHttpParamerValue = GetParameterFromRequest(request, property.Name);
                        var propertyParameterValue = Convert.ChangeType(propertyHttpParamerValue, property.PropertyType);
                        property.SetValue(parameterValue, propertyParameterValue);
                    }
                }

                arguments.Add(parameterValue);
            }

            var response = action.Invoke(instance, arguments.ToArray()) as HttpResponse;
            return response;
        }

        private static string GetParameterFromRequest(HttpRequest request, string parameterName)
        {
            parameterName = parameterName.ToLower();
            if (request.FormData.Any(x => x.Key.ToLower() == parameterName))
            {
                return request.FormData
                    .FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }

            if (request.QueryData.Any(x => x.Key.ToLower() == parameterName))
            {
                return request.QueryData
                    .FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }

            return null;
        }
    }
}
